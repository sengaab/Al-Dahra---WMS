"use client";

import { useRouter } from "next/navigation";

import Button from "@/components/button";
import Status from "@/components/status";

import { useReceipts } from "@/hooks/use-receipts";
import PurchaseOrders from "../purchase-orders/page";

export default function Receiving() {
    const router = useRouter();

    const {
        receipts,
        loading,
        error,
    } = useReceipts();

    function getStatusLabel(status: string) {
        switch (status) {
            case "Pending":
                return "Pending";

            case "InProgress":
                return "In Progress";

            case "PartiallyReceived":
                return "Partially Received";

            case "Completed":
                return "Completed";

            case "Cancelled":
                return "Cancelled";

            default:
                return status;
        }
    }

    function getStatusVariant(status: string) {
        switch (status) {
            case "Pending":
                return "orange";

            case "InProgress":
                return "blue";

            case "PartiallyReceived":
                return "green-stroke";

            case "Completed":
                return "green";

            case "Cancelled":
                return "red-stroke";

            default:
                return "red";
        }
    }

    return (
        <div className="page">
            <div
                className="card column-container"
                style={{
                    width: "100%",
                    paddingInline: "0",
                }}
            >
                <div
                    className="row-container"
                    style={{
                        width: "100%",
                        justifyContent: "space-between",
                        alignItems: "center",
                        paddingInline: "var(--space-3)",
                    }}
                >
                    <p className="body-title">
                        Receiving Queue
                    </p>

                    <Button
                        variant="secondary"
                        size="sm"
                        onClick={() =>
                            router.push("/receiving/select-po")
                        }
                    >
                        + Receive Shipment
                    </Button>
                </div>

                <div
                    style={{
                        width: "100%",
                        overflowX: "auto",
                    }}
                >
                    <table
                        style={{
                            width: "100%",
                            borderCollapse: "separate",
                            borderSpacing: 0,
                        }}
                    >
                        <thead>
                            <tr
                                style={{
                                    height: "40px",
                                    backgroundColor: "var(--beige)",
                                }}
                            >
                                <th
                                    style={{
                                        width: "12%",
                                        padding: "0 24px",
                                        textAlign: "left",
                                        verticalAlign: "middle",
                                        borderTop:
                                            "var(--border-default)",
                                        borderBottom:
                                            "var(--border-default)",
                                        backgroundColor: "var(--beige)",
                                    }}
                                >
                                    Receipt #
                                </th>

                                <th
                                    style={{
                                        width: "12%",
                                        padding: "0 24px",
                                        textAlign: "left",
                                        verticalAlign: "middle",
                                        borderTop:
                                            "var(--border-default)",
                                        borderBottom:
                                            "var(--border-default)",
                                        backgroundColor: "var(--beige)",
                                    }}
                                >
                                    PO Number
                                </th>

                                <th
                                    style={{
                                        width: "16%",
                                        padding: "0 24px",
                                        textAlign: "left",
                                        verticalAlign: "middle",
                                        borderTop:
                                            "var(--border-default)",
                                        borderBottom:
                                            "var(--border-default)",
                                        backgroundColor: "var(--beige)",
                                    }}
                                >
                                    Supplier
                                </th>

                                <th
                                    style={{
                                        width: "13%",
                                        padding: "0 24px",
                                        textAlign: "left",
                                        verticalAlign: "middle",
                                        borderTop:
                                            "var(--border-default)",
                                        borderBottom:
                                            "var(--border-default)",
                                        backgroundColor: "var(--beige)",
                                    }}
                                >
                                    Expected Date
                                </th>

                                <th
                                    style={{
                                        width: "8%",
                                        padding: "0 24px",
                                        textAlign: "left",
                                        verticalAlign: "middle",
                                        borderTop:
                                            "var(--border-default)",
                                        borderBottom:
                                            "var(--border-default)",
                                        backgroundColor: "var(--beige)",
                                    }}
                                >
                                    Items
                                </th>

                                <th
                                    style={{
                                        width: "12%",
                                        padding: "0 24px",
                                        textAlign: "left",
                                        verticalAlign: "middle",
                                        borderTop:
                                            "var(--border-default)",
                                        borderBottom:
                                            "var(--border-default)",
                                        backgroundColor: "var(--beige)",
                                    }}
                                >
                                    Expected Qty
                                </th>

                                <th
                                    style={{
                                        width: "12%",
                                        padding: "0 24px",
                                        textAlign: "left",
                                        verticalAlign: "middle",
                                        borderTop:
                                            "var(--border-default)",
                                        borderBottom:
                                            "var(--border-default)",
                                        backgroundColor: "var(--beige)",
                                    }}
                                >
                                    Received Qty
                                </th>

                                <th
                                    style={{
                                        width: "10%",
                                        padding: "0 24px",
                                        textAlign: "left",
                                        verticalAlign: "middle",
                                        borderTop:
                                            "var(--border-default)",
                                        borderBottom:
                                            "var(--border-default)",
                                        backgroundColor: "var(--beige)",
                                    }}
                                >
                                    Status
                                </th>

                                <th
                                    style={{
                                        width: "10%",
                                        padding: "0 24px",
                                        textAlign: "right",
                                        verticalAlign: "middle",
                                        borderTop:
                                            "var(--border-default)",
                                        borderBottom:
                                            "var(--border-default)",
                                        backgroundColor: "var(--beige)",
                                    }}
                                >
                                    Action
                                </th>
                            </tr>
                        </thead>

                        <tbody>
                            {loading ? (
                                <tr>
                                    <td
                                        colSpan={9}
                                        style={{
                                            height: "100px",
                                            textAlign: "center",
                                        }}
                                    >
                                        Loading...
                                    </td>
                                </tr>
                            ) : error ? (
                                <tr>
                                    <td
                                        colSpan={9}
                                        style={{
                                            height: "100px",
                                            textAlign: "center",
                                        }}
                                    >
                                        {error}
                                    </td>
                                </tr>
                            ) : receipts.length === 0 ? (
                                <tr>
                                    <td
                                        colSpan={9}
                                        style={{
                                            height: "100px",
                                            textAlign: "center",
                                        }}
                                    >
                                        No receipts found
                                    </td>
                                </tr>
                            ) : (
                                receipts.map((receipt) => (
                                    <tr
                                        key={receipt.receiptId}
                                        style={{
                                            height: "44px",
                                        }}
                                    >
                                        <td
                                            style={{
                                                padding: "0 24px",
                                                borderBottom:
                                                    "var(--border-default)",
                                            }}
                                        >
                                            {receipt.receiptNumber}
                                        </td>

                                        <td
                                            style={{
                                                padding: "0 24px",
                                                borderBottom:
                                                    "var(--border-default)",
                                            }}
                                        >
                                            {receipt.poNumber}
                                        </td>

                                        <td
                                            style={{
                                                padding: "0 24px",
                                                borderBottom:
                                                    "var(--border-default)",
                                            }}
                                        >
                                            {receipt.supplierName}
                                        </td>

                                        <td
                                            style={{
                                                padding: "0 24px",
                                                borderBottom:
                                                    "var(--border-default)",
                                            }}
                                        >
                                            {receipt.expectedDate
                                                ? new Date(
                                                    receipt.expectedDate
                                                ).toLocaleDateString(
                                                    "en-US"
                                                )
                                                : "—"}
                                        </td>

                                        <td
                                            style={{
                                                padding: "0 24px",
                                                borderBottom:
                                                    "var(--border-default)",
                                            }}
                                        >
                                            {receipt.itemsCount}
                                        </td>

                                        <td
                                            style={{
                                                padding: "0 24px",
                                                borderBottom:
                                                    "var(--border-default)",
                                            }}
                                        >
                                            {receipt.expectedQty.toLocaleString(
                                                "en-US"
                                            )}
                                        </td>

                                        <td
                                            style={{
                                                padding: "0 24px",
                                                borderBottom:
                                                    "var(--border-default)",
                                            }}
                                        >
                                            {receipt.receivedQty.toLocaleString(
                                                "en-US"
                                            )}
                                        </td>

                                        <td
                                            style={{
                                                padding: "0 24px",
                                                borderBottom:
                                                    "var(--border-default)",
                                            }}
                                        >
                                            <Status
                                                text={getStatusLabel(
                                                    receipt.receiptStatus
                                                )}
                                                variant={getStatusVariant(
                                                    receipt.receiptStatus
                                                )}
                                            />
                                        </td>

                                        <td
                                            style={{
                                                padding: "0 24px",
                                                textAlign: "right",
                                                borderBottom:
                                                    "var(--border-default)",
                                            }}
                                        >
                                            <Button
                                                variant="outline"
                                                size="sm"
                                                onClick={() =>
                                                    router.push(
                                                        `/receiving/po-lines?poId=${receipt.poId}`
                                                    )
                                                }
                                            >
                                                Receive
                                            </Button>
                                        </td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
}