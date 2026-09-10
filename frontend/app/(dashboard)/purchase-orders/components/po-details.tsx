"use client";

import { useMemo } from "react";
import { useRouter } from "next/navigation";

import Card from "@/components/card";
import Status from "@/components/status";
import Button from "@/components/button";



import { usePurchaseOrder } from "@/hooks/usePurchaseOrder";
import { usePurchaseOrderActions } from "@/hooks/usePurchaseOrderActions";

import type {
    PurchaseOrderDto,
    PurchaseOrderItemDto,
} from "@/types/purchase-order";

interface POProps {
    poId: number;

    onClose?: () => void;

    /*
     * Parent should use these values
     * to open AddOrder in edit mode.
     */
    onEdit?: (
        purchaseOrder: PurchaseOrderDto,
        items: PurchaseOrderItemDto[]
    ) => void;
}

/*
 * ==========================================
 * FORMAT DATE
 * ==========================================
 */

const formatDate = (
    value?: string | null
) => {
    if (!value) {
        return "-";
    }

    return new Intl.DateTimeFormat(
        "en-GB",
        {
            day: "2-digit",
            month: "2-digit",
            year: "numeric",
        }
    ).format(new Date(value));
};

/*
 * ==========================================
 * FORMAT CURRENCY
 * ==========================================
 */

const formatCurrency = (
    value: number
) =>
    new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "EGP",
        currencyDisplay: "narrowSymbol",
        maximumFractionDigits: 2,
    })
        .format(value)
        .replace("EGP", "E£");

/*
 * ==========================================
 * COMPONENT
 * ==========================================
 */

export default function PODetails({
    poId,
    onClose,
    onEdit,
}: POProps) {
    const router = useRouter();

    /*
     * ==========================================
     * PURCHASE ORDER HOOK
     * ==========================================
     */

    const {
        purchaseOrder,
        items,
        loading,
        error,
        refreshPurchaseOrder,
    } = usePurchaseOrder(poId);

    const {
        actionLoading,
        actionError,
        handleSubmit,
        handleApprove,
        handleOrder,
    } = usePurchaseOrderActions(
        poId,
        purchaseOrder,
        items,
        refreshPurchaseOrder
    );

    /*
     * ==========================================
     * STATUS
     * ==========================================
     */

    const currentStatus =
        purchaseOrder?.status ||
        "Draft";

    const normalizedStatus =
        currentStatus
            .toLowerCase()
            .replace(/\s+/g, "");

    /*
     * ==========================================
     * TOTAL VALUE
     * ==========================================
     */

    const totalValue = useMemo(() => {
        return items.reduce(
            (total, item) =>
                total +
                Number(
                    item.totalPrice || 0
                ),
            0
        );
    }, [items]);

    /*
     * ==========================================
     * INFORMATION
     * ==========================================
     */

    const information = [
        [
            "Supplier",
            purchaseOrder?.supplierName ||
            "-",
        ],

        [
            "Deliver to",
            purchaseOrder?.siteName ||
            "-",
        ],

        [
            "Order Date",
            formatDate(
                purchaseOrder?.orderDate
            ),
        ],

        [
            "Exp Date",
            formatDate(
                purchaseOrder?.expectedDate
            ),
        ],

        [
            "Created By",
            purchaseOrder?.creatorName ||
            "-",
        ],

        [
            "Approved By",
            purchaseOrder?.approverName ||
            "-",
        ],
    ];

    /*
     * ==========================================
     * RECEIVE
     * ==========================================
     */

    const handleReceive = () => {
        router.push(
            `/receiving/po-lines?poId=${poId}`
        );
    };

    /*
     * ==========================================
     * EDIT
     * ==========================================
     */

    const handleEdit = () => {
        if (!purchaseOrder) {
            return;
        }

        if (!onEdit) {
            console.warn(
                "PODetails: onEdit callback was not provided."
            );

            return;
        }

        onEdit(
            purchaseOrder,
            items
        );
    };

    /*
     * ==========================================
     * EXPORT CSV
     * ==========================================
     */

    const handleExport = () => {
        if (!purchaseOrder) {
            return;
        }

        const headers = [
            "SKU",
            "Product",
            "Ordered",
            "Received",
            "Remaining",
            "Unit Price",
            "Total",
        ];

        const rows = items.map(
            (item) => [
                item.sku,

                item.productName,

                item.orderedQuantity,

                item.receivedQuantity,

                item.remainingQuantity,

                item.unitPrice,

                item.totalPrice,
            ]
        );

        const csv = [
            headers,
            ...rows,
        ]
            .map((row) =>
                row
                    .map(
                        (value) =>
                            `"${String(
                                value ?? ""
                            ).replace(
                                /"/g,
                                '""'
                            )}"`
                    )
                    .join(",")
            )
            .join("\n");

        const blob = new Blob(
            [csv],
            {
                type:
                    "text/csv;charset=utf-8;",
            }
        );

        const url =
            URL.createObjectURL(
                blob
            );

        const link =
            document.createElement(
                "a"
            );

        link.href = url;

        link.download =
            `${purchaseOrder.poNumber}.csv`;

        document.body.appendChild(
            link
        );

        link.click();

        document.body.removeChild(
            link
        );

        URL.revokeObjectURL(url);
    };

    /*
     * ==========================================
     * STATUS VARIANT
     * ==========================================
     */

    function getStatusVariant(
        type: string
    ):
        | "green"
        | "green-stroke"
        | "yellow"
        | "blue"
        | "grey"
        | "orange"
        | "red-stroke" {
        switch (type) {
            case "Draft":
                return "grey";

            case "PendingApproval":
                return "orange";

            case "Approved":
                return "green-stroke";

            case "Ordered":
                return "blue";

            case "PartiallyReceived":
                return "yellow";

            case "Received":
                return "green";

            case "Cancelled":
                return "red-stroke";

            default:
                return "green";
        }
    }

    /*
     * ==========================================
     * STATUS LABEL
     * ==========================================
     */

    const getStatusLabel = (
        status: string
    ) => {
        switch (status) {
            case "PendingApproval":
                return "Pending Approval";

            case "PartiallyReceived":
                return "Partially Received";

            default:
                return status;
        }
    };

    /*
     * ==========================================
     * RENDER
     * ==========================================
     */

    return (
        <Card
            title={
                loading
                    ? ""
                    : `${purchaseOrder?.poNumber ||
                    "-"
                    }`
            }
            header={
                <div
                    className="column-container"
                    style={{
                        width: "100%",
                        justifyContent:
                            "space-between",
                    }}
                >
                    <button
                        type="button"
                        onClick={onClose}
                        disabled={
                            actionLoading
                        }
                        style={{
                            marginLeft:
                                "auto",

                            fontSize:
                                "var(--icon-sm)",

                            fontWeight:
                                "var(--weight-regular)",

                            fontFamily:
                                "var(--font-roboto-serif)",

                            cursor:
                                actionLoading
                                    ? "not-allowed"
                                    : "pointer",

                            border: "none",

                            background:
                                "transparent",

                            opacity:
                                actionLoading
                                    ? 0.5
                                    : 1,
                        }}
                    >
                        X
                    </button>
                </div>
            }
            maxWidth="var(--sidedetails-width)"
        >
            <div
                className="column-container"
                style={{
                    width: "100%",
                    paddingInline:
                        "var(--space-2)",
                }}
            >
                {/* ==========================================
                    LOADING
                ========================================== */}

                {loading && (
                    <p
                        className="body"
                        style={{
                            paddingBlock:
                                "var(--space-4)",

                            textAlign:
                                "center",
                        }}
                    >
                        Loading purchase
                        order...
                    </p>
                )}

                {/* ==========================================
                    ERROR
                ========================================== */}

                {!loading &&
                    error && (
                        <p
                            className="body"
                            style={{
                                color:
                                    "var(--blood-red)",

                                paddingBlock:
                                    "var(--space-4)",

                                textAlign:
                                    "center",
                            }}
                        >
                            {error}
                        </p>
                    )}

                {/* ==========================================
                    CONTENT
                ========================================== */}

                {!loading &&
                    !error &&
                    purchaseOrder && (
                        <>
                            {/* STATUS */}

                            <Status
                                text={getStatusLabel(
                                    currentStatus
                                )}
                                variant={getStatusVariant(
                                    currentStatus
                                )}
                            />

                            {/* ==========================================
                                PO INFORMATION
                            ========================================== */}

                            <div
                                className="grid"
                                style={{
                                    width:
                                        "100%",

                                    gridTemplateColumns:
                                        "repeat(2, 1fr)",

                                    gap:
                                        "var(--space-2)",
                                }}
                            >
                                {information.map(
                                    ([
                                        label,
                                        value,
                                    ]) => (
                                        <div
                                            key={
                                                label
                                            }
                                            className="card column-container"
                                            style={{
                                                width:
                                                    "100%",

                                                gap: 0,

                                                backgroundColor:
                                                    "var(--beige)",

                                                padding:
                                                    "var(--space-2)",

                                                height:
                                                    "100%",
                                            }}
                                        >
                                            <p className="body">
                                                {
                                                    label
                                                }
                                            </p>

                                            <p className="body-title">
                                                {
                                                    value
                                                }
                                            </p>
                                        </div>
                                    )
                                )}
                            </div>

                            {/* ==========================================
                                LINE ITEMS
                            ========================================== */}

                            <p className="body-title">
                                Line Items
                            </p>

                            <div
                                className="card"
                                style={{
                                    width:
                                        "100%",

                                    overflowX:
                                        "auto",

                                    padding: 0,
                                }}
                            >
                                <div
                                    style={{
                                        minWidth:
                                            "750px",

                                        width:
                                            "100%",
                                    }}
                                >
                                    {/* TABLE HEADER */}

                                    <table
                                        style={{
                                            width:
                                                "100%",

                                            tableLayout:
                                                "fixed",

                                            borderCollapse:
                                                "collapse",
                                        }}
                                    >
                                        <thead>
                                            <tr
                                                style={{
                                                    backgroundColor:
                                                        "var(--beige)",

                                                    borderBottom:
                                                        "var(--border-default)",
                                                }}
                                            >
                                                <th
                                                    style={{
                                                        width:
                                                            "12%",

                                                        padding:
                                                            "var(--space-2)",

                                                        textAlign:
                                                            "left",
                                                    }}
                                                >
                                                    SKU
                                                </th>

                                                <th
                                                    style={{
                                                        width:
                                                            "20%",

                                                        padding:
                                                            "var(--space-2)",

                                                        textAlign:
                                                            "left",
                                                    }}
                                                >
                                                    Product
                                                </th>

                                                <th
                                                    style={{
                                                        width:
                                                            "13%",

                                                        padding:
                                                            "var(--space-2)",

                                                        textAlign:
                                                            "left",
                                                    }}
                                                >
                                                    Ordered
                                                </th>

                                                <th
                                                    style={{
                                                        width:
                                                            "13%",

                                                        padding:
                                                            "var(--space-2)",

                                                        textAlign:
                                                            "left",
                                                    }}
                                                >
                                                    Received
                                                </th>

                                                <th
                                                    style={{
                                                        width:
                                                            "13%",

                                                        padding:
                                                            "var(--space-2)",

                                                        textAlign:
                                                            "left",
                                                    }}
                                                >
                                                    Remaining
                                                </th>

                                                <th
                                                    style={{
                                                        width:
                                                            "14%",

                                                        padding:
                                                            "var(--space-2)",

                                                        textAlign:
                                                            "left",
                                                    }}
                                                >
                                                    Unit Price
                                                </th>

                                                <th
                                                    style={{
                                                        width:
                                                            "15%",

                                                        padding:
                                                            "var(--space-2)",

                                                        textAlign:
                                                            "left",
                                                    }}
                                                >
                                                    Total
                                                </th>
                                            </tr>
                                        </thead>
                                    </table>

                                    {/* SCROLLABLE ROWS */}

                                    <div
                                        style={{
                                            maxHeight:
                                                "calc(5 * 48px)",

                                            overflowY:
                                                "auto",

                                            overflowX:
                                                "hidden",
                                        }}
                                    >
                                        <table
                                            style={{
                                                width:
                                                    "100%",

                                                tableLayout:
                                                    "fixed",

                                                borderCollapse:
                                                    "collapse",
                                            }}
                                        >
                                            <tbody>
                                                {items.length ===
                                                    0 ? (
                                                    <tr>
                                                        <td
                                                            colSpan={
                                                                7
                                                            }
                                                            style={{
                                                                padding:
                                                                    "var(--space-4)",

                                                                textAlign:
                                                                    "center",

                                                                color:
                                                                    "var(--grey)",
                                                            }}
                                                        >
                                                            No
                                                            line
                                                            items
                                                            found.
                                                        </td>
                                                    </tr>
                                                ) : (
                                                    items.map(
                                                        (
                                                            item
                                                        ) => (
                                                            <tr
                                                                key={
                                                                    item.purchaseOrderItemId
                                                                }
                                                            >
                                                                {/* SKU */}

                                                                <td
                                                                    style={{
                                                                        width:
                                                                            "12%",

                                                                        padding:
                                                                            "var(--space-2)",

                                                                        borderBottom:
                                                                            "var(--border-default)",
                                                                    }}
                                                                >
                                                                    {
                                                                        item.sku
                                                                    }
                                                                </td>

                                                                {/* PRODUCT */}

                                                                <td
                                                                    style={{
                                                                        width:
                                                                            "20%",

                                                                        padding:
                                                                            "var(--space-2)",

                                                                        borderBottom:
                                                                            "var(--border-default)",
                                                                    }}
                                                                >
                                                                    {
                                                                        item.productName
                                                                    }
                                                                </td>

                                                                {/* ORDERED */}

                                                                <td
                                                                    style={{
                                                                        width:
                                                                            "13%",

                                                                        padding:
                                                                            "var(--space-2)",

                                                                        borderBottom:
                                                                            "var(--border-default)",
                                                                    }}
                                                                >
                                                                    {
                                                                        item.orderedQuantity
                                                                    }
                                                                </td>

                                                                {/* RECEIVED */}

                                                                <td
                                                                    style={{
                                                                        width:
                                                                            "13%",

                                                                        padding:
                                                                            "var(--space-2)",

                                                                        borderBottom:
                                                                            "var(--border-default)",
                                                                    }}
                                                                >
                                                                    {
                                                                        item.receivedQuantity
                                                                    }
                                                                </td>

                                                                {/* REMAINING */}

                                                                <td
                                                                    style={{
                                                                        width:
                                                                            "13%",

                                                                        padding:
                                                                            "var(--space-2)",

                                                                        borderBottom:
                                                                            "var(--border-default)",
                                                                    }}
                                                                >
                                                                    {
                                                                        item.remainingQuantity
                                                                    }
                                                                </td>

                                                                {/* UNIT PRICE */}

                                                                <td
                                                                    style={{
                                                                        width:
                                                                            "14%",

                                                                        padding:
                                                                            "var(--space-2)",

                                                                        borderBottom:
                                                                            "var(--border-default)",
                                                                    }}
                                                                >
                                                                    {formatCurrency(
                                                                        Number(
                                                                            item.unitPrice
                                                                        )
                                                                    )}
                                                                </td>

                                                                {/* TOTAL */}

                                                                <td
                                                                    style={{
                                                                        width:
                                                                            "15%",

                                                                        padding:
                                                                            "var(--space-2)",

                                                                        borderBottom:
                                                                            "var(--border-default)",
                                                                    }}
                                                                >
                                                                    {formatCurrency(
                                                                        Number(
                                                                            item.totalPrice
                                                                        )
                                                                    )}
                                                                </td>
                                                            </tr>
                                                        )
                                                    )
                                                )}
                                            </tbody>
                                        </table>
                                    </div>

                                    {/* FIXED FOOTER */}

                                    <table
                                        style={{
                                            width:
                                                "100%",

                                            tableLayout:
                                                "fixed",

                                            borderCollapse:
                                                "collapse",

                                            backgroundColor:
                                                "var(--beige)",
                                        }}
                                    >
                                        <tfoot>
                                            <tr
                                                style={{
                                                    borderTop:
                                                        "var(--border-default)",

                                                    borderBottom:
                                                        "var(--border-default)",
                                                }}
                                            >
                                                <td
                                                    colSpan={
                                                        6
                                                    }
                                                    style={{
                                                        padding:
                                                            "var(--space-2)",

                                                        textAlign:
                                                            "left",

                                                        fontWeight:
                                                            "var(--weight-medium)",
                                                    }}
                                                >
                                                    Total :

                                                    <span
                                                        style={{
                                                            marginLeft:
                                                                "var(--space-2)",

                                                            color:
                                                                "var(--dark-green)",
                                                        }}
                                                    >
                                                        {formatCurrency(
                                                            totalValue
                                                        )}
                                                    </span>
                                                </td>
                                            </tr>
                                        </tfoot>
                                    </table>
                                </div>
                            </div>

                            {/* ==========================================
                                ACTION ERROR
                            ========================================== */}

                            {actionError && (
                                <p
                                    className="body"
                                    style={{
                                        color:
                                            "var(--blood-red)",

                                        width:
                                            "100%",
                                    }}
                                >
                                    {
                                        actionError
                                    }
                                </p>
                            )}

                            {/* ==========================================
                                ACTIONS
                            ========================================== */}

                            <div
                                className="row-container"
                                style={{
                                    width:
                                        "100%",

                                    justifyContent:
                                        "stretch",

                                    gap:
                                        "var(--space-2)",

                                    paddingBottom:
                                        "var(--space-2)",
                                }}
                            >
                                {/* ==========================================
                                    APPROVE
                                ========================================== */}

                                {normalizedStatus ===
                                    "pendingapproval" && (
                                        <Button
                                            variant="secondary"
                                            onClick={
                                                handleApprove
                                            }
                                            disabled={
                                                actionLoading
                                            }
                                            size="sm"
                                        >
                                            {actionLoading
                                                ? "Approving..."
                                                : "Approve"}
                                        </Button>
                                    )}

                                {/* ==========================================
                                    RECEIVE
                                ========================================== */}

                                {(
                                    normalizedStatus ===
                                    "ordered" ||
                                    normalizedStatus ===
                                    "partiallyreceived"
                                ) && (
                                        <Button
                                            variant="secondary"
                                            onClick={
                                                handleReceive
                                            }
                                            disabled={
                                                actionLoading
                                            }
                                            size="sm"
                                        >
                                            Receive
                                        </Button>
                                    )}

                                {/* ==========================================
                                    DRAFT ACTIONS
                                ========================================== */}

                                {normalizedStatus ===
                                    "draft" && (
                                        <>
                                            <Button
                                                variant="secondary"
                                                onClick={
                                                    handleSubmit
                                                }
                                                disabled={
                                                    actionLoading
                                                }
                                                size="sm"
                                            >
                                                {actionLoading
                                                    ? "Submitting..."
                                                    : "Submit"}
                                            </Button>

                                            <Button
                                                variant="outline"
                                                onClick={
                                                    handleEdit
                                                }
                                                disabled={
                                                    actionLoading
                                                }
                                                size="sm"
                                            >
                                                Edit
                                            </Button>
                                        </>
                                    )}

                                {/* ==========================================
                                    ORDER
                                ========================================== */}

                                {normalizedStatus === "approved" && (
                                    <Button
                                        variant="secondary"
                                        onClick={handleOrder}
                                        disabled={actionLoading}
                                        size="sm"
                                    >
                                        {actionLoading ? "Ordering..." : "Order"}
                                    </Button>
                                )}

                                {/* ==========================================
                                    EXPORT
                                ========================================== */}

                                <Button
                                    variant="outline"
                                    onClick={
                                        handleExport
                                    }
                                    disabled={
                                        actionLoading
                                    }
                                    size="sm"
                                >
                                    Export
                                </Button>
                            </div>
                        </>
                    )}
            </div>
        </Card>
    );
}