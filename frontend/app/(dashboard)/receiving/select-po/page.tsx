"use client";

import { useEffect, useState } from "react";

import ReceiveStepper from "../components/receive-stepper";

import {
    getPurchaseOrderById,
    getPurchaseOrderItems,
} from "@/lib/api/purchase-orders";

import { getReceipts } from "@/lib/api/receipts";

import type {
    PurchaseOrderDto,
    PurchaseOrderItemDto,
} from "@/types/purchase-order";
import Button from "@/components/button";

import { useRouter } from "next/navigation";

export default function SelectPO() {
    const router = useRouter();

    const [purchaseOrders, setPurchaseOrders] = useState<PurchaseOrderDto[]>(
        []
    );

    const [poItems, setPoItems] = useState<
        Record<number, PurchaseOrderItemDto[]>
    >({});

    const [selectedPO, setSelectedPO] = useState<number | null>(null);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        const loadData = async () => {
            try {
                setLoading(true);
                setError("");

                /*
                 * Get purchase orders through receipts.
                 *
                 * Completed receipts are excluded because they
                 * should no longer be available for receiving.
                 */
                const receipts = await getReceipts();

                const activeReceipts = receipts.filter(
                    (receipt) =>
                        receipt.receiptStatus.toLowerCase() !== "completed"
                );

                /*
                 * Get unique PO IDs from the active receipts.
                 */
                const purchaseOrderIds = [
                    ...new Set(
                        activeReceipts.map(
                            (receipt) => receipt.purchaseOrderId
                        )
                    ),
                ];

                /*
                 * Get the actual purchase orders.
                 */
                const orders = await Promise.all(
                    purchaseOrderIds.map((purchaseOrderId) =>
                        getPurchaseOrderById(purchaseOrderId)
                    )
                );

                setPurchaseOrders(orders);

                /*
                 * Get items for each PO.
                 */
                const itemsResults = await Promise.all(
                    orders.map(async (order) => {
                        const items = await getPurchaseOrderItems(
                            order.purchaseOrderId
                        );

                        return {
                            purchaseOrderId: order.purchaseOrderId,
                            items,
                        };
                    })
                );

                const itemsMap: Record<
                    number,
                    PurchaseOrderItemDto[]
                > = {};

                itemsResults.forEach(
                    ({ purchaseOrderId, items }) => {
                        itemsMap[purchaseOrderId] = items;
                    }
                );

                setPoItems(itemsMap);
            } catch (err) {
                console.error("Failed to load receiving POs:", err);

                setError(
                    err instanceof Error
                        ? err.message
                        : "Failed to load purchase orders."
                );
            } finally {
                setLoading(false);
            }
        };

        loadData();
    }, []);

    const handleSelectPO = (purchaseOrderId: number) => {
        setSelectedPO((current) =>
            current === purchaseOrderId ? null : purchaseOrderId
        );
    };

    return (
        <div className="page-column">
            <ReceiveStepper />

            <div
                className="card column-container"
                style={{
                    width: "100%",
                    minHeight: 0,
                }}
            >
                {/* Header */}
                <div
                    className="column-container"
                    style={{
                        gap: "var(--space-1)",
                    }}
                >
                    <p className="nav-item">
                        Select Purchase Order
                    </p>

                    <p
                        className="body-title"
                        style={{
                            color: "var(--grey)",
                        }}
                    >
                        Select the purchase order to receive against
                    </p>
                </div>

                {/* Loading */}
                {loading && (
                    <div
                        className="column-container"
                        style={{
                            width: "100%",
                            alignItems: "center",
                            justifyContent: "center",
                            padding: "var(--space-3)",
                        }}
                    >
                        <p>Loading purchase orders...</p>
                    </div>
                )}

                {/* Error */}
                {!loading && error && (
                    <div
                        className="column-container"
                        style={{
                            width: "100%",
                            padding: "var(--space-3)",
                        }}
                    >
                        <p
                            className="body-title"
                            style={{
                                color: "var(--red)",
                            }}
                        >
                            {error}
                        </p>
                    </div>
                )}

                {/* Empty */}
                {!loading &&
                    !error &&
                    purchaseOrders.length === 0 && (
                        <div
                            className="column-container"
                            style={{
                                width: "100%",
                                alignItems: "center",
                                justifyContent: "center",
                                padding: "var(--space-3)",
                            }}
                        >
                            <p className="body-title">
                                No purchase orders available for receiving.
                            </p>

                            <p
                                className="body-title"
                                style={{
                                    color: "var(--grey)",
                                }}
                            >
                                All available receipts may already be
                                completed.
                            </p>
                        </div>
                    )}

                {/* Purchase Orders */}
                {!loading &&
                    !error &&
                    purchaseOrders.length > 0 && (
                        <div
                            className="row-container"
                            style={{
                                width: "100%",
                                height: "100%",
                                flexWrap: "wrap",
                                alignItems: "stretch",
                            }}
                        >
                            {purchaseOrders.map((purchaseOrder) => {
                                const items =
                                    poItems[
                                    purchaseOrder.purchaseOrderId
                                    ] ?? [];

                                const firstFourItems = items.slice(0, 4);

                                const isSelected =
                                    selectedPO ===
                                    purchaseOrder.purchaseOrderId;

                                return (
                                    <div
                                        key={
                                            purchaseOrder.purchaseOrderId
                                        }
                                        className="card column-container"
                                        onClick={() =>
                                            handleSelectPO(
                                                purchaseOrder.purchaseOrderId
                                            )
                                        }
                                        style={{
                                            minHeight: "0",
                                            flex: "1 1 300px",
                                            cursor: "pointer",

                                            border: isSelected
                                                ? "2px solid var(--dark-green)"
                                                : "var(--border-default)",

                                            backgroundColor: isSelected
                                                ? "var(--beige)"
                                                : undefined,

                                            transition:
                                                "border 0.15s ease, background-color 0.15s ease",

                                            gap: "var(--space-3)",
                                        }}
                                    >
                                        {/* PO Information */}
                                        <div
                                            className="column-container"
                                            style={{
                                                gap: "var(--space-1)",
                                            }}
                                        >
                                            <p
                                                className="body-title"
                                                style={{
                                                    color: "var(--dark-green)",
                                                }}
                                            >
                                                {purchaseOrder.poNumber}
                                            </p>

                                            <p className="body-title">
                                                {
                                                    purchaseOrder.supplierName
                                                }
                                            </p>
                                        </div>

                                        {/* Items */}
                                        <div
                                            className="column-container"
                                            style={{
                                                gap: "var(--space-1)",
                                            }}
                                        >
                                            <p>
                                                {
                                                    purchaseOrder.itemsCount
                                                }{" "}
                                                Items Expected
                                            </p>

                                            <div
                                                className="column-container"
                                                style={{
                                                    gap: "var(--space-1)",
                                                }}
                                            >
                                                {firstFourItems.length >
                                                    0 ? (
                                                    firstFourItems.map(
                                                        (item) => (
                                                            <p
                                                                key={
                                                                    item.purchaseOrderItemId
                                                                }
                                                            >
                                                                {item.sku} ·{" "}
                                                                {
                                                                    item.productName
                                                                }
                                                            </p>
                                                        )
                                                    )
                                                ) : (
                                                    <p
                                                        style={{
                                                            color: "var(--grey)",
                                                        }}
                                                    >
                                                        No items found
                                                    </p>
                                                )}
                                            </div>

                                            {items.length > 4 && (
                                                <p
                                                    style={{
                                                        color: "var(--grey)",
                                                    }}
                                                >
                                                    +
                                                    {items.length - 4}{" "}
                                                    more
                                                </p>
                                            )}
                                        </div>

                                        {/* Quantity */}
                                        <div
                                            className="row-container"
                                            style={{
                                                justifyContent:
                                                    "space-between",
                                                alignItems: "center",
                                                marginTop: "auto",
                                            }}
                                        >
                                            <p>
                                                Expected:{" "}
                                                {
                                                    purchaseOrder.totalOrderedQuantity
                                                }
                                            </p>

                                            <p>
                                                Received:{" "}
                                                {
                                                    purchaseOrder.totalReceivedQuantity
                                                }
                                            </p>
                                        </div>

                                        {/* Selected State */}
                                        {isSelected && (
                                            <p
                                                className="body-title"
                                                style={{
                                                    color: "var(--dark-green)",
                                                }}
                                            >
                                                Selected
                                            </p>
                                        )}
                                    </div>
                                );
                            })}

                        </div>
                    )}
                <Button
                    style={{
                        marginLeft: "auto",
                    }}
                    variant="secondary"
                    onClick={() => {
                        if (!selectedPO) return;

                        router.push(`/receiving/po-lines?poId=${selectedPO}`);
                    }}
                    disabled={!selectedPO}
                >
                    Start Receiving
                </Button>
            </div>
        </div>
    );
}