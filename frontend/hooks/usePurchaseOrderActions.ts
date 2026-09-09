"use client";

import { useState } from "react";

import {
    submitPurchaseOrder,
    approvePurchaseOrder,
    orderPurchaseOrder,
} from "@/lib/api/purchase-orders";

import {
    createReceipt,
    addReceiptItem,
} from "@/lib/api/receipts";

import { getWarehouses } from "@/lib/api/warehouses";

import type {
    PurchaseOrderDto,
    PurchaseOrderItemDto,
} from "@/types/purchase-order";

export function usePurchaseOrderActions(
    poId: number,
    purchaseOrder: PurchaseOrderDto | null,
    items: PurchaseOrderItemDto[],
    refreshPurchaseOrder: () => Promise<void>
) {
    const [actionLoading, setActionLoading] = useState(false);
    const [actionError, setActionError] = useState("");

    const handleSubmit = async () => {
        try {
            setActionLoading(true);
            setActionError("");

            await submitPurchaseOrder(poId);

            await refreshPurchaseOrder();
        } catch (err: any) {
            console.error("Submit PO error:", err);

            setActionError(
                err?.message ||
                    "Failed to submit purchase order."
            );
        } finally {
            setActionLoading(false);
        }
    };

    const handleApprove = async () => {
        try {
            setActionLoading(true);
            setActionError("");

            await approvePurchaseOrder(poId);

            await refreshPurchaseOrder();
        } catch (err: any) {
            console.error("Approve PO error:", err);

            setActionError(
                err?.message ||
                    "Failed to approve purchase order."
            );
        } finally {
            setActionLoading(false);
        }
    };

    const handleOrder = async () => {
        try {
            setActionLoading(true);
            setActionError("");

            if (!purchaseOrder) {
                throw new Error(
                    "Purchase order information is not available."
                );
            }

            // 1. Get warehouses belonging to this site
            const warehouses = await getWarehouses({
                siteId: purchaseOrder.siteId,
                page: 1,
                pageSize: 1,
            });

            if (!warehouses || warehouses.length === 0) {
                throw new Error(
                    `No warehouse found for site ${purchaseOrder.siteName}.`
                );
            }

            // 2. Take the first warehouse
            const warehouse = warehouses[0];

            // 3. Change PO status to Ordered
            await orderPurchaseOrder(poId);

            // 4. Create receipt
            const receipt = await createReceipt({
                purchaseOrderId: poId,
                warehouseId: warehouse.warehouseId,
                notes: `Receipt created for purchase order ${purchaseOrder.poNumber}`,
            });

            // 5. Add PO items to the receipt
            const remainingItems = items.filter(
                (item) => item.remainingQuantity > 0
            );

            for (const item of remainingItems) {
                await addReceiptItem(
                    receipt.receiptId,
                    {
                        purchaseOrderItemId:
                            item.purchaseOrderItemId,

                        productId: item.productId,

                        receivedQuantity:
                            item.remainingQuantity,

                        acceptedQuantity:
                            item.remainingQuantity,

                        quarantineQuantity: 0,

                        rejectedQuantity: 0,

                        batchNumber: null,

                        expiryDate: null,
                    }
                );
            }

            // 6. Refresh PO
            await refreshPurchaseOrder();
        } catch (err: any) {
            console.error(
                "Order purchase order error:",
                err
            );

            setActionError(
                err?.message ||
                    "Failed to order purchase order."
            );
        } finally {
            setActionLoading(false);
        }
    };

    return {
        actionLoading,
        actionError,
        handleSubmit,
        handleApprove,
        handleOrder,
    };
}