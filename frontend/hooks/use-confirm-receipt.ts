"use client";

import { useCallback, useEffect, useState } from "react";

import {
    getReceiptById,
    getReceiptItems,
    partialReceivedReceipt,
    completeReceipt,
} from "@/lib/api/receipts";

import {
    getPurchaseOrderById,
    getPurchaseOrderItems,
    partialReceivedPurchaseOrder,
    receivedPurchaseOrder,
} from "@/lib/api/purchase-orders";

import { getProductById } from "@/lib/api/products";

import { createStock } from "@/lib/api/stock";

import type { ReceiptDto, ReceiptItemDto } from "@/types/receipt";
import type {
    PurchaseOrderDto,
    PurchaseOrderItemDto,
} from "@/types/purchase-order";
import type { ProductDto } from "@/types/product";

export interface ConfirmReceiptLine {
    receiptItemId: number;
    purchaseOrderItemId: number;

    productId: number;

    sku: string;
    product: string;

    expected: number;
    received: number;
    damaged: number;
    net: number;

    batch: string | null;
    expiry: string | null;

    unitPrice: number;
    lineTotal: number;

    inspection: string;

    minimumStock: number;
}

export type ConfirmReceiptStatus =
    | "Pending"
    | "Partial"
    | "Completed";

interface UseConfirmReceiptResult {
    receiptId: number | null;

    receipt: ReceiptDto | null;
    po: PurchaseOrderDto | null;

    receiptItems: ReceiptItemDto[];
    poItems: PurchaseOrderItemDto[];

    lines: ConfirmReceiptLine[];

    receiptStatus: ConfirmReceiptStatus | null;

    loading: boolean;
    posting: boolean;

    error: string;
    success: string;

    postReceiptToInspection: () => Promise<void>;

    reload: () => Promise<void>;
}

export function useConfirmReceipt(
    receiptId: number | null
): UseConfirmReceiptResult {
    const [receipt, setReceipt] =
        useState<ReceiptDto | null>(null);

    const [po, setPo] =
        useState<PurchaseOrderDto | null>(null);

    const [receiptItems, setReceiptItems] =
        useState<ReceiptItemDto[]>([]);

    const [poItems, setPoItems] =
        useState<PurchaseOrderItemDto[]>([]);

    const [lines, setLines] =
        useState<ConfirmReceiptLine[]>([]);

    const [receiptStatus, setReceiptStatus] =
        useState<ConfirmReceiptStatus | null>(null);

    const [loading, setLoading] =
        useState(true);

    const [posting, setPosting] =
        useState(false);

    const [error, setError] =
        useState("");

    const [success, setSuccess] =
        useState("");

    const getProductSafe = useCallback(
        async (
            productId: number
        ): Promise<ProductDto | null> => {
            try {
                return await getProductById(productId);
            } catch {
                return null;
            }
        },
        []
    );

    const loadConfirmReceipt =
        useCallback(async () => {
            if (!receiptId) {
                setLoading(false);
                setError(
                    "Receipt ID is missing."
                );
                return;
            }

            try {
                setLoading(true);
                setError("");
                setSuccess("");

                /*
                 * 1. Load receipt
                 */
                const receiptData =
                    await getReceiptById(
                        receiptId
                    );

                setReceipt(receiptData);

                /*
                 * 2. Load PO, PO items and
                 *    receipt items
                 */
                const [
                    purchaseOrderData,
                    purchaseOrderItemsData,
                    receiptItemsData,
                ] = await Promise.all([
                    getPurchaseOrderById(
                        receiptData.purchaseOrderId
                    ),
                    getPurchaseOrderItems(
                        receiptData.purchaseOrderId
                    ),
                    getReceiptItems(
                        receiptId
                    ),
                ]);

                setPo(purchaseOrderData);
                setPoItems(
                    purchaseOrderItemsData
                );
                setReceiptItems(
                    receiptItemsData
                );

                /*
                 * 3. Build confirmation lines
                 */
                const confirmationLines =
                    await Promise.all(
                        receiptItemsData.map(
                            async (
                                receiptItem
                            ) => {
                                const poItem =
                                    purchaseOrderItemsData.find(
                                        (item) =>
                                            item.purchaseOrderItemId ===
                                            receiptItem.purchaseOrderItemId
                                    );

                                if (!poItem) {
                                    throw new Error(
                                        `Purchase order item ${receiptItem.purchaseOrderItemId} was not found.`
                                    );
                                }

                                const product =
                                    await getProductSafe(
                                        poItem.productId
                                    );

                                const expected =
                                    Number(
                                        poItem.orderedQuantity ??
                                            0
                                    );

                                const received =
                                    Number(
                                        receiptItem.receivedQuantity ??
                                            0
                                    );

                                const damaged =
                                    Number(
                                        receiptItem.rejectedQuantity ??
                                            0
                                    );

                                const net =
                                    Math.max(
                                        0,
                                        received -
                                            damaged
                                    );

                                const unitPrice =
                                    Number(
                                        poItem.unitPrice ??
                                            product?.unitPrice ??
                                            0
                                    );

                                const lineTotal =
                                    net *
                                    unitPrice;

                                return {
                                    receiptItemId:
                                        receiptItem.receiptItemId,

                                    purchaseOrderItemId:
                                        receiptItem.purchaseOrderItemId,

                                    productId:
                                        poItem.productId,

                                    sku:
                                        product?.sku ??
                                        poItem.sku ??
                                        "",

                                    product:
                                        product?.name ??
                                        poItem.productName ??
                                        "",

                                    expected,

                                    received,

                                    damaged,

                                    net,

                                    batch:
                                        receiptItem.batchNumber ??
                                        null,

                                    expiry:
                                        receiptItem.expiryDate
                                            ? String(
                                                  receiptItem.expiryDate
                                              ).split(
                                                  "T"
                                              )[0]
                                            : null,

                                    unitPrice,

                                    lineTotal,

                                    inspection:
                                        "Pending",

                                    minimumStock:
                                        Number(
                                            product?.minimumStock ??
                                                0
                                        ),
                                };
                            }
                        )
                    );

                setLines(
                    confirmationLines
                );

                /*
                 * 4. Determine overall receipt
                 *    status.
                 */
                const totalExpected =
                    confirmationLines.reduce(
                        (sum, line) =>
                            sum +
                            line.expected,
                        0
                    );

                const totalReceived =
                    confirmationLines.reduce(
                        (sum, line) =>
                            sum +
                            line.received,
                        0
                    );

                let calculatedStatus:
                    ConfirmReceiptStatus;

                if (
                    totalReceived === 0
                ) {
                    calculatedStatus =
                        "Pending";
                } else if (
                    totalReceived <
                    totalExpected
                ) {
                    calculatedStatus =
                        "Partial";
                } else {
                    calculatedStatus =
                        "Completed";
                }

                setReceiptStatus(
                    calculatedStatus
                );
            } catch (err) {
                console.error(
                    "Failed to load confirm receipt:",
                    err
                );

                setError(
                    err instanceof Error
                        ? err.message
                        : "Failed to load receipt."
                );

                setReceipt(null);
                setPo(null);
                setReceiptItems([]);
                setPoItems([]);
                setLines([]);
                setReceiptStatus(null);
            } finally {
                setLoading(false);
            }
        }, [
            receiptId,
            getProductSafe,
        ]);

    useEffect(() => {
        loadConfirmReceipt();
    }, [loadConfirmReceipt]);

    const postReceiptToInspection =
        useCallback(async () => {
            if (!receiptId) {
                setError(
                    "Receipt ID is missing."
                );
                return;
            }

            if (!receipt) {
                setError(
                    "Receipt could not be loaded."
                );
                return;
            }

            if (!po) {
                setError(
                    "Purchase order could not be loaded."
                );
                return;
            }

            if (!lines.length) {
                setError(
                    "There are no receipt items to post."
                );
                return;
            }

            try {
                setPosting(true);
                setError("");
                setSuccess("");

                /*
                 * Recalculate the status immediately
                 * before posting.
                 */
                const totalExpected =
                    lines.reduce(
                        (sum, line) =>
                            sum +
                            line.expected,
                        0
                    );

                const totalReceived =
                    lines.reduce(
                        (sum, line) =>
                            sum +
                            line.received,
                        0
                    );

                if (
                    totalReceived === 0
                ) {
                    throw new Error(
                        "No quantity has been received."
                    );
                }

                const isPartial =
                    totalReceived <
                    totalExpected;

                /*
                 * =================================
                 * 1. UPDATE PURCHASE ORDER STATUS
                 * =================================
                 */
                if (isPartial) {
                    await partialReceivedPurchaseOrder(
                        po.purchaseOrderId
                    );
                } else {
                    await receivedPurchaseOrder(
                        po.purchaseOrderId
                    );
                }

                /*
                 * =================================
                 * 2. UPDATE RECEIPT STATUS
                 * =================================
                 */
                if (isPartial) {
                    await partialReceivedReceipt(
                        receipt.receiptId
                    );
                } else {
                    await completeReceipt(
                        receipt.receiptId
                    );
                }

                /*
                 * =================================
                 * 3. CREATE QUARANTINED STOCK
                 * =================================
                 *
                 * Stock is created without a
                 * location.
                 *
                 * The backend automatically
                 * creates it as Quarantined.
                 */
                for (const line of lines) {
                    const netQuantity =
                        Math.max(
                            0,
                            line.received -
                                line.damaged
                        );

                    /*
                     * Don't create a stock row
                     * when nothing was accepted.
                     */
                    if (
                        netQuantity <= 0
                    ) {
                        continue;
                    }

                    await createStock({
                        productId:
                            line.productId,

                        warehouseId:
                            receipt.warehouseId,

                        locationId: null,

                        quantity:
                            netQuantity,

                        reservedQuantity: 0,

                        unitPrice:
                            line.unitPrice,

                        minimumStock:
                            line.minimumStock,

                        batchNumber:
                            line.batch,

                        expiryDate:
                            line.expiry,
                    });
                }

                /*
                 * Update local status so the UI
                 * immediately reflects the result.
                 */
                const finalStatus:
                    ConfirmReceiptStatus =
                    isPartial
                        ? "Partial"
                        : "Completed";

                setReceiptStatus(
                    finalStatus
                );

                setSuccess(
                    `Receipt posted successfully as ${finalStatus}. Stock was created as Quarantined.`
                );

                /*
                 * Reload everything from the
                 * backend.
                 */
                await loadConfirmReceipt();
            } catch (err) {
                console.error(
                    "Failed to post receipt:",
                    err
                );

                setError(
                    err instanceof Error
                        ? err.message
                        : "Failed to post receipt."
                );
            } finally {
                setPosting(false);
            }
        }, [
            receiptId,
            receipt,
            po,
            lines,
            loadConfirmReceipt,
        ]);

    return {
        receiptId,

        receipt,
        po,

        receiptItems,
        poItems,

        lines,

        receiptStatus,

        loading,
        posting,

        error,
        success,

        postReceiptToInspection,

        reload: loadConfirmReceipt,
    };
}