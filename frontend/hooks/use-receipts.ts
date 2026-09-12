"use client";

import { useCallback, useEffect, useState } from "react";

import { getReceipts } from "@/lib/api/receipts";
import { getPurchaseOrders } from "@/lib/api/purchase-orders";

import type { ReceiptDto } from "@/types/receipt";
import type { PurchaseOrderDto } from "@/types/purchase-order";
import type { ReceiptListItem } from "@/types/receipt";

interface UseReceiptsReturn {
    receipts: ReceiptListItem[];
    loading: boolean;
    error: string | null;
    refetch: () => Promise<void>;
}

export function useReceipts(): UseReceiptsReturn {
    const [receipts, setReceipts] = useState<ReceiptListItem[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const fetchReceipts = useCallback(async () => {
        try {
            setLoading(true);
            setError(null);

            const [receiptData, purchaseOrderData] =
                await Promise.all([
                    getReceipts(),
                    getPurchaseOrders(),
                ]);

            const purchaseOrdersMap = new Map<
                number,
                PurchaseOrderDto
            >();

            purchaseOrderData.forEach((purchaseOrder) => {
                purchaseOrdersMap.set(
                    purchaseOrder.purchaseOrderId,
                    purchaseOrder
                );
            });

            const mappedReceipts: ReceiptListItem[] =
                receiptData.map((receipt) => {
                    const purchaseOrder =
                        purchaseOrdersMap.get(receipt.purchaseOrderId);

                    const receivedQty = receipt.items.reduce(
                        (total, item) =>
                            total + Number(item.receivedQuantity || 0),
                        0
                    );

                    return {
                        receiptId: receipt.receiptId,
                        receiptNumber: receipt.receiptNumber,

                        poId: receipt.purchaseOrderId,

                        poNumber: purchaseOrder?.poNumber ?? "-",
                        supplierName:
                            purchaseOrder?.supplierName ?? "-",
                        expectedDate:
                            purchaseOrder?.expectedDate ?? null,

                        itemsCount: receipt.items.length,

                        expectedQty:
                            purchaseOrder?.totalOrderedQuantity ?? 0,

                        receivedQty,

                        receiptStatus: receipt.receiptStatus,
                    };
                });

            setReceipts(mappedReceipts);
        } catch (err) {
            setError(
                err instanceof Error
                    ? err.message
                    : "Failed to load receipts."
            );
        } finally {
            setLoading(false);
        }
    }, []);

    useEffect(() => {
        fetchReceipts();
    }, [fetchReceipts]);

    return {
        receipts,
        loading,
        error,
        refetch: fetchReceipts,
    };
}