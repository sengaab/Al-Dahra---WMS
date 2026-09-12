"use client";

import { useEffect, useMemo, useState } from "react";

import Button from "@/components/button";
import Status from "@/components/status";
import { getPurchaseOrders } from "@/lib/api/purchase-orders";
import type {
    PurchaseOrderDto,
    PurchaseOrderItemDto,
} from "@/types/purchase-order";
import AddOrder from "@/app/(dashboard)/purchase-orders/components/add-order";
import PODetails from "@/app/(dashboard)/purchase-orders/components/po-details";

const statuses = [
    { label: "All", value: "All" },
    { label: "Draft", value: "Draft" },
    { label: "Pending Approval", value: "PendingApproval" },
    { label: "Approved", value: "Approved" },
    { label: "Ordered", value: "Ordered" },
    { label: "Partially Received", value: "PartiallyReceived" },
    { label: "Received", value: "Received" },
    { label: "Cancelled", value: "Cancelled" },
];

function getStatusVariant(
    type: string
): "green" | "green-stroke" | "yellow" | "blue" | "grey" | "orange" | "red-stroke" {
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

const ROW_HEIGHT = 52;
const HEADER_HEIGHT = 40;
const MAX_ROWS = 12;

export default function PurchaseOrders() {
    const [addOrderOpen, setAddOrderOpen] = useState(false);

    const [purchaseOrders, setPurchaseOrders] = useState<
        PurchaseOrderDto[]
    >([]);

    const [selectedStatus, setSelectedStatus] =
        useState("All");

    const [loading, setLoading] =
        useState(true);

    const [error, setError] =
        useState<string | null>(null);

    // =========================
    // PO Details
    // =========================

    const [selectedPOId, setSelectedPOId] =
        useState<number | null>(null);

    // =========================
    // Edit PO
    // =========================

    const [selectedPurchaseOrder, setSelectedPurchaseOrder] =
        useState<PurchaseOrderDto | null>(null);

    const [selectedPurchaseOrderItems, setSelectedPurchaseOrderItems] =
        useState<PurchaseOrderItemDto[]>([]);

    const detailsOpen =
        selectedPOId !== null;

    const isEditing =
        selectedPurchaseOrder !== null;

    // =========================
    // Load Purchase Orders
    // =========================

    const loadPurchaseOrders = async () => {
        try {
            setLoading(true);
            setError(null);

            const data = await getPurchaseOrders({
                page: 1,
                pageSize: 1000,
            });

            setPurchaseOrders(data);
        } catch (err) {
            console.error(
                "Failed to load purchase orders:",
                err
            );

            setError(
                "Failed to load purchase orders."
            );
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadPurchaseOrders();
    }, []);

    // =========================
    // Filtering
    // =========================

    const filteredPurchaseOrders = useMemo(() => {
        if (selectedStatus === "All") {
            return purchaseOrders;
        }

        return purchaseOrders.filter(
            (po) =>
                po.status === selectedStatus
        );
    }, [
        purchaseOrders,
        selectedStatus,
    ]);

    // =========================
    // Helpers
    // =========================

    const formatDate = (
        date: string | null
    ) => {
        if (!date) return "—";

        return new Date(date).toLocaleDateString(
            "en-GB",
            {
                day: "2-digit",
                month: "short",
                year: "numeric",
            }
        );
    };

    const formatCurrency = (
        value: number
    ) => {
        return `E£ ${value.toLocaleString(
            "en-EG",
            {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2,
            }
        )}`;
    };

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

    // =========================
    // Open New PO
    // =========================

    const handleNewPO = () => {
        // Clear any previous edit data
        setSelectedPurchaseOrder(null);
        setSelectedPurchaseOrderItems([]);

        setAddOrderOpen(true);
    };

    // =========================
    // Edit PO
    // =========================

    const handleEditPO = (
        purchaseOrder: PurchaseOrderDto,
        items: PurchaseOrderItemDto[]
    ) => {
        setSelectedPurchaseOrder(
            purchaseOrder
        );

        setSelectedPurchaseOrderItems(
            items
        );

        // Close details panel
        setSelectedPOId(null);

        // Open Add/Edit Order popup
        setAddOrderOpen(true);
    };

    // =========================
    // Close Add/Edit PO
    // =========================

    const handleCloseAddOrder = () => {
        setAddOrderOpen(false);

        // Clear edit state
        setSelectedPurchaseOrder(null);
        setSelectedPurchaseOrderItems([]);
    };

    // =========================
    // After PO Saved
    // =========================

    const handleOrderSaved = async () => {
        handleCloseAddOrder();

        // Reload table so the changes appear immediately
        await loadPurchaseOrders();
    };

    return (
        <>
            <div
                className="page"
                style={{
                    display: "flex",
                    alignItems: "stretch",
                    gap: "var(--content-gap)",
                    height: "100%",
                    minHeight: 0,
                    overflow: "hidden",
                }}
            >
                {/* =========================
                    Purchase Orders Table
                ========================== */}

                <div
                    className="card column-container"
                    style={{
                        flex: detailsOpen
                            ? "1 1 0"
                            : "1 1 100%",

                        width: detailsOpen
                            ? "calc(100% - var(--sidedetails-width) - var(--content-gap))"
                            : "100%",

                        minWidth: 0,
                        minHeight: 0,
                        height: "100%",

                        transition:
                            "width 0.25s ease, flex 0.25s ease",

                        paddingInline: 0,
                    }}
                >
                    {/* =========================
                        Header
                    ========================== */}

                    <div
                        className="row-container"
                        style={{
                            width: "100%",
                            justifyContent:
                                "space-between",

                            paddingInline:
                                "var(--space-3)",

                            flexWrap: "wrap",

                            gap: "var(--space-2)",
                        }}
                    >
                        <p className="body-title">
                            Purchase Orders
                        </p>

                        <div
                            className="row-container"
                            style={{
                                flexWrap:
                                    "wrap",

                                gap:
                                    "var(--space-1)",
                            }}
                        >
                            {statuses.map(
                                (status) => {
                                    const hasOrders =
                                        status.value ===
                                        "All"
                                            ? purchaseOrders.length >
                                              0
                                            : purchaseOrders.some(
                                                  (
                                                      po
                                                  ) =>
                                                      po.status ===
                                                      status.value
                                              );

                                    return (
                                        <Button
                                            key={
                                                status.value
                                            }
                                            size="sm"
                                            variant={
                                                selectedStatus ===
                                                status.value
                                                    ? "secondary"
                                                    : "beige"
                                            }
                                            disabled={
                                                !hasOrders
                                            }
                                            onClick={() =>
                                                setSelectedStatus(
                                                    status.value
                                                )
                                            }
                                        >
                                            {
                                                status.label
                                            }
                                        </Button>
                                    );
                                }
                            )}

                            <Button
                                size="sm"
                                variant="secondary"
                                onClick={
                                    handleNewPO
                                }
                            >
                                + New PO
                            </Button>
                        </div>
                    </div>

                    {/* =========================
                        Error
                    ========================== */}

                    {error && (
                        <div
                            style={{
                                padding:
                                    "var(--space-3)",
                                color:
                                    "var(--blood-red)",
                            }}
                        >
                            {error}
                        </div>
                    )}

                    {/* =========================
                        Table
                    ========================== */}

                    <div
                        style={{
                            width: "100%",
                            maxHeight: `${
                                HEADER_HEIGHT +
                                ROW_HEIGHT *
                                    MAX_ROWS
                            }px`,

                            overflowY: "auto",
                            overflowX: "auto",
                        }}
                    >
                        <table
                            style={{
                                width: "100%",
                                borderCollapse:
                                    "separate",
                                borderSpacing: 0,
                            }}
                        >
                            <thead>
                                <tr
                                    style={{
                                        height: `${HEADER_HEIGHT}px`,
                                    }}
                                >
                                    <th
                                        style={{
                                            width: "10%",
                                            padding:
                                                "0 24px",
                                            textAlign:
                                                "left",
                                            verticalAlign:
                                                "middle",
                                            borderTop:
                                                "var(--border-default)",
                                            borderBottom:
                                                "var(--border-default)",
                                            backgroundColor:
                                                "var(--beige)",
                                            whiteSpace:
                                                "nowrap",
                                            position:
                                                "sticky",
                                            top: 0,
                                            zIndex: 2,
                                        }}
                                    >
                                        PO-Number
                                    </th>

                                    <th
                                        style={{
                                            width: "15%",
                                            padding:
                                                "0 24px",
                                            textAlign:
                                                "left",
                                            verticalAlign:
                                                "middle",
                                            borderTop:
                                                "var(--border-default)",
                                            borderBottom:
                                                "var(--border-default)",
                                            backgroundColor:
                                                "var(--beige)",
                                            position:
                                                "sticky",
                                            top: 0,
                                            zIndex: 2,
                                        }}
                                    >
                                        Supplier
                                    </th>

                                    <th
                                        style={{
                                            width: "15%",
                                            padding:
                                                "0 24px",
                                            textAlign:
                                                "left",
                                            verticalAlign:
                                                "middle",
                                            borderTop:
                                                "var(--border-default)",
                                            borderBottom:
                                                "var(--border-default)",
                                            backgroundColor:
                                                "var(--beige)",
                                            position:
                                                "sticky",
                                            top: 0,
                                            zIndex: 2,
                                        }}
                                    >
                                        Expected Date
                                    </th>

                                    <th
                                        style={{
                                            width: "15%",
                                            padding:
                                                "0 24px",
                                            textAlign:
                                                "left",
                                            verticalAlign:
                                                "middle",
                                            borderTop:
                                                "var(--border-default)",
                                            borderBottom:
                                                "var(--border-default)",
                                            backgroundColor:
                                                "var(--beige)",
                                            position:
                                                "sticky",
                                            top: 0,
                                            zIndex: 2,
                                        }}
                                    >
                                        Value
                                    </th>

                                    <th
                                        style={{
                                            width: "15%",
                                            padding:
                                                "0 24px",
                                            textAlign:
                                                "left",
                                            verticalAlign:
                                                "middle",
                                            borderTop:
                                                "var(--border-default)",
                                            borderBottom:
                                                "var(--border-default)",
                                            backgroundColor:
                                                "var(--beige)",
                                            position:
                                                "sticky",
                                            top: 0,
                                            zIndex: 2,
                                        }}
                                    >
                                        Status
                                    </th>

                                    <th
                                        style={{
                                            width: "15%",
                                            padding:
                                                "0 24px",
                                            textAlign:
                                                "left",
                                            verticalAlign:
                                                "middle",
                                            borderTop:
                                                "var(--border-default)",
                                            borderBottom:
                                                "var(--border-default)",
                                            backgroundColor:
                                                "var(--beige)",
                                            position:
                                                "sticky",
                                            top: 0,
                                            zIndex: 2,
                                        }}
                                    >
                                        Created by
                                    </th>

                                    <th
                                        style={{
                                            width: "15%",
                                            padding:
                                                "0 24px",
                                            textAlign:
                                                "left",
                                            verticalAlign:
                                                "middle",
                                            borderTop:
                                                "var(--border-default)",
                                            borderBottom:
                                                "var(--border-default)",
                                            backgroundColor:
                                                "var(--beige)",
                                            position:
                                                "sticky",
                                            top: 0,
                                            zIndex: 2,
                                        }}
                                    >
                                        Approved by
                                    </th>
                                </tr>
                            </thead>

                            <tbody>
                                {loading ? (
                                    <tr>
                                        <td
                                            colSpan={7}
                                            style={{
                                                height: `${ROW_HEIGHT}px`,
                                                textAlign:
                                                    "center",
                                                borderBottom:
                                                    "var(--border-default)",
                                            }}
                                        >
                                            Loading purchase
                                            orders...
                                        </td>
                                    </tr>
                                ) : filteredPurchaseOrders.length ===
                                  0 ? (
                                    <tr>
                                        <td
                                            colSpan={7}
                                            style={{
                                                height: `${ROW_HEIGHT}px`,
                                                textAlign:
                                                    "center",
                                                borderBottom:
                                                    "var(--border-default)",
                                            }}
                                        >
                                            No purchase
                                            orders found.
                                        </td>
                                    </tr>
                                ) : (
                                    filteredPurchaseOrders.map(
                                        (po) => (
                                            <tr
                                                key={
                                                    po.purchaseOrderId
                                                }
                                                onClick={() =>
                                                    setSelectedPOId(
                                                        po.purchaseOrderId
                                                    )
                                                }
                                                style={{
                                                    cursor:
                                                        "pointer",
                                                    height: `${ROW_HEIGHT}px`,
                                                }}
                                                onMouseEnter={(
                                                    e
                                                ) => {
                                                    e.currentTarget.style.backgroundColor =
                                                        "var(--beige)";
                                                }}
                                                onMouseLeave={(
                                                    e
                                                ) => {
                                                    e.currentTarget.style.backgroundColor =
                                                        "transparent";
                                                }}
                                            >
                                                <td
                                                    style={{
                                                        height: `${ROW_HEIGHT}px`,
                                                        padding:
                                                            "0 24px",
                                                        borderBottom:
                                                            "var(--border-default)",
                                                        fontWeight:
                                                            500,
                                                    }}
                                                >
                                                    {
                                                        po.poNumber
                                                    }
                                                </td>

                                                <td
                                                    style={{
                                                        height: `${ROW_HEIGHT}px`,
                                                        padding:
                                                            "0 24px",
                                                        borderBottom:
                                                            "var(--border-default)",
                                                    }}
                                                >
                                                    {
                                                        po.supplierName
                                                    }
                                                </td>

                                                <td
                                                    style={{
                                                        height: `${ROW_HEIGHT}px`,
                                                        padding:
                                                            "0 24px",
                                                        borderBottom:
                                                            "var(--border-default)",
                                                    }}
                                                >
                                                    {formatDate(
                                                        po.expectedDate
                                                    )}
                                                </td>

                                                <td
                                                    style={{
                                                        height: `${ROW_HEIGHT}px`,
                                                        padding:
                                                            "0 24px",
                                                        borderBottom:
                                                            "var(--border-default)",
                                                    }}
                                                >
                                                    {formatCurrency(
                                                        po.totalValue
                                                    )}
                                                </td>

                                                <td
                                                    style={{
                                                        height: `${ROW_HEIGHT}px`,
                                                        padding:
                                                            "0 24px",
                                                        borderBottom:
                                                            "var(--border-default)",
                                                    }}
                                                >
                                                    <Status
                                                        text={getStatusLabel(
                                                            po.status
                                                        )}
                                                        variant={getStatusVariant(
                                                            po.status
                                                        )}
                                                    />
                                                </td>

                                                <td
                                                    style={{
                                                        height: `${ROW_HEIGHT}px`,
                                                        padding:
                                                            "0 24px",
                                                        borderBottom:
                                                            "var(--border-default)",
                                                    }}
                                                >
                                                    {po.creatorName ??
                                                        "—"}
                                                </td>

                                                <td
                                                    style={{
                                                        height: `${ROW_HEIGHT}px`,
                                                        padding:
                                                            "0 24px",
                                                        borderBottom:
                                                            "var(--border-default)",
                                                    }}
                                                >
                                                    {po.approverName ??
                                                        "—"}
                                                </td>
                                            </tr>
                                        )
                                    )
                                )}
                            </tbody>
                        </table>
                    </div>
                </div>

                {/* =========================
                    PO Details
                ========================== */}

                {detailsOpen && (
                    <div
                        style={{
                            flex: `0 0 var(--sidedetails-width)`,
                            width:
                                "var(--sidedetails-width)",
                            maxWidth:
                                "var(--sidedetails-width)",
                            minWidth: 0,
                            minHeight: 0,
                            height: "fit-content",
                            overflow: "hidden",
                        }}
                    >
                        <PODetails
                            poId={selectedPOId}
                            onClose={() =>
                                setSelectedPOId(null)
                            }
                            onEdit={
                                handleEditPO
                            }
                        />
                    </div>
                )}
            </div>

            {/* =========================
                Add / Edit PO Popup
            ========================== */}

            {addOrderOpen && (
                <div
                    style={{
                        position: "fixed",
                        inset: 0,
                        zIndex: 1000,
                        display: "flex",
                        alignItems: "center",
                        justifyContent:
                            "center",
                        backgroundColor:
                            "rgba(0, 0, 0, 0.35)",
                        padding:
                            "var(--content-padding)",
                        boxSizing: "border-box",
                    }}
                    onClick={
                        handleCloseAddOrder
                    }
                >
                    <div
                        onClick={(event) =>
                            event.stopPropagation()
                        }
                    >
                        <AddOrder
                            editOrder={
                                isEditing
                                    ? selectedPurchaseOrder ??
                                      undefined
                                    : undefined
                            }
                            editItems={
                                isEditing
                                    ? selectedPurchaseOrderItems
                                    : []
                            }
                            onClose={
                                handleOrderSaved
                            }
                        />
                    </div>
                </div>
            )}
        </>
    );
}