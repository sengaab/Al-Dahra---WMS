"use client";

import { useCallback, useEffect, useMemo, useState } from "react";

import { createClient } from "@/lib/supabase/client";

import {
    getPurchaseOrderById,
    getPurchaseOrderItems,
    getPurchaseOrderReceipts,
    updatePurchaseOrderItem,
} from "@/lib/api/purchase-orders";

import { getCategories } from "@/lib/api/categories";
import { getUnits } from "@/lib/api/units";

import type { CategoryDto } from "@/types/category";
import type { UnitDto } from "@/types/unit";

import {
    getReceiptById,
    getReceiptItems,
    updateReceipt,
    updateReceiptItem,
} from "@/lib/api/receipts";

import {
    createProduct,
    getProductByBarcode,
    getProductBySku,
} from "@/lib/api/products";

import { getWarehouses } from "@/lib/api/warehouses";

import type {
    PurchaseOrderDto,
    PurchaseOrderItemDto,
    PurchaseOrderReceiptDto,
} from "@/types/purchase-order";

import type {
    ReceiptDto,
    ReceiptItemDto,
} from "@/types/receipt";

import type {
    ProductDto,
    CreateProductDto,
} from "@/types/product";

import type { WarehouseDto } from "@/types/warehouse";

export const RECEIVING_STEPS = [
    {
        title: "Scan / Identify",
        subtitle: "Scan or enter the product barcode",
    },
    {
        title: "Verify Product",
        subtitle: "Confirm that the scanned product matches the PO line",
    },
    {
        title: "Quantity",
        subtitle: "Enter the quantity received",
    },
    {
        title: "Batch / Lot",
        subtitle: "Enter the batch or lot number",
    },
    {
        title: "Expiry Date",
        subtitle: "Enter the product expiry date",
    },
    {
        title: "Unit Price",
        subtitle: "Confirm the unit price",
    },
    {
        title: "Damaged Quantity",
        subtitle: "Enter the quantity of damaged items",
    },
] as const;

export interface ReceivingLine {
    purchaseOrderItemId: number;
    receiptItemId: number;

    productId: number;
    productName: string;
    sku: string;
    barcode: string | null;

    categoryId: number | null;
    categoryName: string | null;

    unitId: number | null;
    unitName: string | null;

    minimumStock: number;
    description: string | null;

    orderedQuantity: number;
    previouslyReceivedQuantity: number;
    remainingQuantity: number;

    receivedQuantity: number;
    damagedQuantity: number;

    batchNumber: string;
    expiryDate: string;
    unitPrice: number;

    isExistingProduct: boolean;
    isProductVerified: boolean;

    receivingStatus: "Pending" | "Partial" | "Complete";
    isSaved: boolean;
}

interface UseReceivingProps {
    poId: number;
}

interface UpdatePurchaseOrderItemReceivingDto {
    productId?: number;
    orderedQuantity?: number;
    unitPrice?: number;
    receivedQuantity?: number;
}

function isNotFoundError(error: unknown) {
    if (!error) return false;

    const message =
        error instanceof Error
            ? error.message.toLowerCase()
            : String(error).toLowerCase();

    return (
        message.includes("404") ||
        message.includes("not found")
    );
}

export default function useReceiving({
    poId,
}: UseReceivingProps) {
    const supabase = createClient();

    const [po, setPo] = useState<PurchaseOrderDto | null>(null);
    const [poItems, setPoItems] = useState<
        PurchaseOrderItemDto[]
    >([]);

    const [receipt, setReceipt] =
        useState<ReceiptDto | null>(null);

    const [receiptItems, setReceiptItems] = useState<
        ReceiptItemDto[]
    >([]);

    const [warehouses, setWarehouses] = useState<
        WarehouseDto[]
    >([]);

    const [categories, setCategories] = useState<
        CategoryDto[]
    >([]);

    const [units, setUnits] = useState<
        UnitDto[]
    >([]);

    const [selectedWarehouseId, setSelectedWarehouseId] =
        useState<number | null>(null);

    const [receivedBy, setReceivedBy] =
        useState<string>("");

    const [lines, setLines] = useState<
        ReceivingLine[]
    >([]);

    const [selectedLineId, setSelectedLineId] =
        useState<number | null>(null);

    const [currentStep, setCurrentStep] =
        useState(0);

    const [loading, setLoading] =
        useState(true);

    const [saving, setSaving] =
        useState(false);

    const [error, setError] =
        useState("");

    const [scanError, setScanError] =
        useState("");

    /*
     * =====================================================
     * LOAD RECEIVING DATA
     * =====================================================
     */

    const loadReceiving = useCallback(async () => {
        try {
            setLoading(true);
            setError("");

            /*
             * Current authenticated user
             */
            const {
                data: { user },
                error: userError,
            } = await supabase.auth.getUser();

            if (userError) {
                throw userError;
            }

            if (!user) {
                throw new Error(
                    "No authenticated user found."
                );
            }

            setReceivedBy(user.id);

            /*
             * Load PO
             */
            const purchaseOrder =
                await getPurchaseOrderById(poId);

            setPo(purchaseOrder);

            /*
             * Load PO items
             */
            const purchaseOrderItems =
                await getPurchaseOrderItems(poId);

            setPoItems(purchaseOrderItems);

            /*
             * Load existing receipts.
             *
             * This page does NOT create a receipt.
             */
            const purchaseOrderReceipts =
                await getPurchaseOrderReceipts(poId);

            if (
                !purchaseOrderReceipts ||
                purchaseOrderReceipts.length === 0
            ) {
                throw new Error(
                    "No receipt exists for this purchase order."
                );
            }

            /*
             * Use the latest receipt.
             *
             * Receipt items have already been created
             * before entering this step.
             */
            const existingReceipt =
                purchaseOrderReceipts[
                purchaseOrderReceipts.length - 1
                ];

            const fullReceipt =
                await getReceiptById(
                    existingReceipt.receiptId
                );

            setReceipt(fullReceipt);

            /*
             * Load existing receipt items
             */
            const existingReceiptItems =
                await getReceiptItems(
                    existingReceipt.receiptId
                );

            setReceiptItems(existingReceiptItems);

            /*
 * Load dropdown data
 *
 * Warehouses are limited to the PO site.
 * Categories and units are loaded globally.
 */
            const [
                siteWarehouses,
                productCategories,
                productUnits,
            ] = await Promise.all([
                getWarehouses({
                    siteId: purchaseOrder.siteId,
                    status: "Active",
                }),

                getCategories(),

                getUnits(),
            ]);

            setWarehouses(siteWarehouses);
            setCategories(productCategories);
            setUnits(productUnits);

            /*
             * Keep the receipt warehouse if already assigned.
             * Otherwise let the user choose one.
             */
            if (fullReceipt.warehouseId) {
                setSelectedWarehouseId(
                    fullReceipt.warehouseId
                );
            }

            /*
             * Build receiving lines.
             */
            const receivingLines: ReceivingLine[] =
                purchaseOrderItems.map((item) => {
                    const receiptItem =
                        existingReceiptItems.find(
                            (ri) =>
                                ri.purchaseOrderItemId ===
                                item.purchaseOrderItemId
                        );

                    if (!receiptItem) {
                        throw new Error(
                            `Receipt item is missing for PO item ${item.purchaseOrderItemId}.`
                        );
                    }

                    const orderedQuantity =
                        Number(item.orderedQuantity ?? 0);

                    const previouslyReceivedQuantity =
                        Number(
                            receiptItem.receivedQuantity ?? 0
                        );

                    const remainingQuantity =
                        Math.max(
                            0,
                            orderedQuantity -
                            previouslyReceivedQuantity
                        );

                    return {
                        purchaseOrderItemId:
                            item.purchaseOrderItemId,

                        receiptItemId:
                            receiptItem.receiptItemId,

                        productId:
                            item.productId,

                        productName:
                            item.productName ?? "",

                        sku:
                            item.sku ?? "",

                        barcode: null,

                        categoryId: null,

                        categoryName: null,

                        unitId: null,

                        unitName: null,

                        minimumStock: 0,

                        description: null,

                        orderedQuantity,

                        previouslyReceivedQuantity,

                        remainingQuantity,

                        /*
                         * This should be the quantity being entered
                         * for the current receiving operation.
                         */
                        receivedQuantity: 0,

                        damagedQuantity: 0,

                        batchNumber:
                            receiptItem.batchNumber ?? "",

                        expiryDate:
                            receiptItem.expiryDate
                                ? String(
                                    receiptItem.expiryDate
                                ).split("T")[0]
                                : "",

                        unitPrice:
                            Number(item.unitPrice ?? 0),

                        isExistingProduct: true,

                        isProductVerified: false,

                        receivingStatus:
                            remainingQuantity <= 0
                                ? "Complete"
                                : "Pending",

                        isSaved:
                            previouslyReceivedQuantity > 0,
                    };
                });

            const enrichedLines =
                await Promise.all(
                    receivingLines.map(
                        async (line) => {
                            const product =
                                await getProductByIdSafe(
                                    line.productId
                                );

                            if (!product) {
                                return line;
                            }

                            return {
                                ...line,

                                productName:
                                    product.name ??
                                    line.productName,

                                sku:
                                    product.sku ??
                                    line.sku,

                                barcode:
                                    product.barcode ??
                                    null,

                                categoryId:
                                    product.categoryId ??
                                    null,

                                categoryName:
                                    product.categoryName ??
                                    null,

                                unitId:
                                    product.unitId ??
                                    null,

                                unitName:
                                    product.unitName ??
                                    null,

                                minimumStock:
                                    Number(
                                        product.minimumStock ??
                                        0
                                    ),

                                description:
                                    product.description ??
                                    null,

                                unitPrice:
                                    Number(
                                        line.unitPrice ??
                                        product.unitPrice ??
                                        0
                                    ),
                            };
                        }
                    )
                );

            setLines(enrichedLines);

            /*
             * Select first line automatically.
             */
            if (enrichedLines.length > 0) {
                setSelectedLineId(
                    enrichedLines[0]
                        .purchaseOrderItemId
                );
            }
        } catch (err) {
            console.error(err);

            setError(
                err instanceof Error
                    ? err.message
                    : "Failed to load receiving data."
            );
        } finally {
            setLoading(false);
        }
    }, [poId]);

    /*
     * Small local wrapper so product enrichment
     * does not expose another public hook function.
     */
    const getProductByIdSafe = async (
        productId: number
    ): Promise<ProductDto | null> => {
        try {
            const { getProductById } =
                await import("@/lib/api/products");

            return await getProductById(productId);
        } catch {
            return null;
        }
    };

    useEffect(() => {
        loadReceiving();
    }, [loadReceiving]);

    /*
     * =====================================================
     * SELECTED LINE
     * =====================================================
     */

    const selectedLine = useMemo(
        () =>
            lines.find(
                (line) =>
                    line.purchaseOrderItemId ===
                    selectedLineId
            ) ?? null,
        [lines, selectedLineId]
    );

    const selectedLineIndex = useMemo(
        () =>
            lines.findIndex(
                (line) =>
                    line.purchaseOrderItemId ===
                    selectedLineId
            ),
        [lines, selectedLineId]
    );

    /*
     * =====================================================
     * UPDATE LINE
     * =====================================================
     */

    const updateLine = useCallback(
        (
            updates: Partial<ReceivingLine>
        ) => {
            if (selectedLineId === null) {
                return;
            }

            setLines((current) =>
                current.map((line) =>
                    line.purchaseOrderItemId ===
                        selectedLineId
                        ? {
                            ...line,
                            ...updates,
                        }
                        : line
                )
            );
        },
        [selectedLineId]
    );

    /*
     * =====================================================
     * SCAN PRODUCT
     * =====================================================
     */

    const scanProduct = useCallback(
        async (value: string) => {
            if (!value.trim()) {
                return;
            }

            setScanError("");

            try {
                let product: ProductDto;

                /*
                 * Try barcode first.
                 */
                try {
                    product =
                        await getProductByBarcode(
                            value.trim()
                        );
                } catch {
                    /*
                     * If barcode fails, try SKU.
                     */
                    product =
                        await getProductBySku(
                            value.trim()
                        );
                }

                if (!selectedLine) {
                    throw new Error(
                        "Select a PO line first."
                    );
                }

                /*
                 * Verify scanned product against
                 * the selected PO line.
                 */
                if (
                    product.productId !==
                    selectedLine.productId
                ) {
                    setScanError(
                        `Scanned product does not match ${selectedLine.productName}.`
                    );

                    return;
                }

                updateLine({
                    productId:
                        product.productId,

                    productName:
                        product.name,

                    sku:
                        product.sku,

                    barcode:
                        product.barcode,

                    categoryId:
                        product.categoryId,

                    categoryName:
                        product.categoryName,

                    unitId:
                        product.unitId,

                    unitName:
                        product.unitName,

                    minimumStock:
                        product.minimumStock,

                    description:
                        product.description,

                    isExistingProduct:
                        true,

                    isProductVerified:
                        true,
                });

                setCurrentStep(1);
            } catch (err) {
                console.error(err);

                setScanError(
                    "Product was not found. You can add it as a new product."
                );

                updateLine({
                    isExistingProduct:
                        false,

                    isProductVerified:
                        false,
                });
            }
        },
        [
            selectedLine,
            updateLine,
        ]
    );

    /*
     * =====================================================
     * CREATE NEW PRODUCT
     * =====================================================
     */

    const createNewReceivingProduct =
        useCallback(
            async (
                data: CreateProductDto
            ) => {
                try {
                    setSaving(true);
                    setError("");

                    const product =
                        await createProduct(
                            data
                        );

                    updateLine({
                        productId:
                            product.productId,

                        productName:
                            product.name,

                        sku:
                            product.sku ?? "",

                        barcode:
                            product.barcode,

                        categoryId:
                            product.categoryId,

                        categoryName:
                            product.categoryName,

                        unitId:
                            product.unitId,

                        unitName:
                            product.unitName,

                        minimumStock:
                            product.minimumStock,

                        description:
                            product.description,

                        unitPrice:
                            product.unitPrice,

                        isExistingProduct:
                            false,

                        isProductVerified:
                            true,
                    });

                    return product;
                } catch (err) {
                    console.error(err);

                    setError(
                        err instanceof Error
                            ? err.message
                            : "Failed to create product."
                    );

                    return null;
                } finally {
                    setSaving(false);
                }
            },
            [updateLine]
        );

    /*
     * =====================================================
     * VERIFY PRODUCT
     * =====================================================
     */

    const verifyProduct = useCallback(() => {
        if (!selectedLine) {
            return false;
        }

        if (
            !selectedLine.productName.trim()
        ) {
            setError(
                "Product name is required."
            );

            return false;
        }

        if (!selectedLine.sku.trim()) {
            setError(
                "SKU is required."
            );

            return false;
        }

        if (!selectedLine.categoryId) {
            setError(
                "Category is required."
            );

            return false;
        }

        if (!selectedLine.unitId) {
            setError(
                "Unit of measure is required."
            );

            return false;
        }

        if (
            !selectedWarehouseId
        ) {
            setError(
                "Warehouse is required."
            );

            return false;
        }

        setError("");

        updateLine({
            isProductVerified:
                true,
        });

        setCurrentStep(2);

        return true;
    }, [
        selectedLine,
        selectedWarehouseId,
        updateLine,
    ]);

    /*
     * =====================================================
     * RECEIVING STATUS
     * =====================================================
     */

    const getReceivingStatus = useCallback(
        (
            line: ReceivingLine
        ): "Pending" | "Partial" | "Complete" => {
            if (
                line.receivedQuantity <= 0
            ) {
                return "Pending";
            }

            if (
                line.receivedQuantity >=
                line.remainingQuantity
            ) {
                return "Complete";
            }

            return "Partial";
        },
        []
    );

    /*
     * =====================================================
     * SAVE CURRENT LINE
     * =====================================================
     */

    const saveCurrentLine = useCallback(
        async () => {
            if (!selectedLine) {
                return false;
            }

            if (!receipt) {
                setError(
                    "Receipt was not found."
                );

                return false;
            }

            if (!selectedWarehouseId) {
                setError(
                    "Please select a warehouse."
                );

                setCurrentStep(1);

                return false;
            }

            if (
                !selectedLine.isProductVerified
            ) {
                setError(
                    "Please verify the product first."
                );

                setCurrentStep(1);

                return false;
            }

            if (
                selectedLine.receivedQuantity <= 0
            ) {
                setError(
                    "Received quantity must be greater than zero."
                );

                setCurrentStep(2);

                return false;
            }

            if (
                selectedLine.receivedQuantity >
                selectedLine.remainingQuantity
            ) {
                setError(
                    "Received quantity cannot exceed the remaining PO quantity."
                );

                setCurrentStep(2);

                return false;
            }

            if (
                selectedLine.damagedQuantity >
                selectedLine.receivedQuantity
            ) {
                setError(
                    "Damaged quantity cannot exceed received quantity."
                );

                setCurrentStep(6);

                return false;
            }

            const acceptedQuantity =
                selectedLine.receivedQuantity -
                selectedLine.damagedQuantity;

            const receivingStatus =
                getReceivingStatus(
                    selectedLine
                );

            try {
                setSaving(true);
                setError("");

                /*
                 * =================================================
                 * 1. Update existing receipt
                 * =================================================
                 */
                await updateReceipt(
                    receipt.receiptId,
                    {
                        warehouseId:
                            selectedWarehouseId,

                        receivedBy:
                            receivedBy,

                        receivedAt:
                            new Date().toISOString(),

                        notes:
                            receipt.notes,
                    }
                );

                /*
                 * =================================================
                 * 2. Update EXISTING receipt item
                 * =================================================
                 */
                await updateReceiptItem(
                    receipt.receiptId,
                    selectedLine.receiptItemId,
                    {
                        receivedQuantity:
                            selectedLine.receivedQuantity,

                        acceptedQuantity:
                            acceptedQuantity,

                        quarantineQuantity:
                            0,

                        rejectedQuantity:
                            selectedLine.damagedQuantity,

                        batchNumber:
                            selectedLine.batchNumber ||
                            null,

                        expiryDate:
                            selectedLine.expiryDate ||
                            null,
                    }
                );

                /*
                 * =================================================
                 * 3. Update PO item
                 *
                 * We intentionally send receivedQuantity here
                 * as requested.
                 * =================================================
                 */
                await updatePurchaseOrderItem(
                    poId,
                    selectedLine.purchaseOrderItemId,
                    {
                        productId:
                            selectedLine.productId,

                        unitPrice:
                            selectedLine.unitPrice,

                        receivedQuantity:
                            selectedLine
                                .receivedQuantity,
                    } as UpdatePurchaseOrderItemReceivingDto
                );

                /*
                 * =================================================
                 * Update local state
                 * =================================================
                 */
                setLines((current) =>
                    current.map((line) =>
                        line.purchaseOrderItemId ===
                            selectedLine.purchaseOrderItemId
                            ? {
                                ...line,

                                receivingStatus,

                                isSaved:
                                    true,

                                previouslyReceivedQuantity:
                                    line.receivedQuantity,

                                remainingQuantity:
                                    Math.max(
                                        0,
                                        line.remainingQuantity -
                                        line.receivedQuantity
                                    ),
                            }
                            : line
                    )
                );

                /*
                 * Refresh receipt/PO data.
                 */
                const [
                    refreshedPo,
                    refreshedReceipt,
                    refreshedReceiptItems,
                ] = await Promise.all([
                    getPurchaseOrderById(
                        poId
                    ),

                    getReceiptById(
                        receipt.receiptId
                    ),

                    getReceiptItems(
                        receipt.receiptId
                    ),
                ]);

                setPo(refreshedPo);
                setReceipt(refreshedReceipt);
                setReceiptItems(
                    refreshedReceiptItems
                );

                return true;
            } catch (err) {
                console.error(err);

                setError(
                    err instanceof Error
                        ? err.message
                        : "Failed to save receiving line."
                );

                return false;
            } finally {
                setSaving(false);
            }
        },
        [
            selectedLine,
            receipt,
            selectedWarehouseId,
            receivedBy,
            poId,
            getReceivingStatus,
        ]
    );

    /*
     * =====================================================
     * NEXT STEP
     * =====================================================
     */

    const nextStep = useCallback(async () => {
        if (!selectedLine) {
            return;
        }

        setError("");

        /*
         * Last step = save this PO line.
         */
        if (
            currentStep ===
            RECEIVING_STEPS.length - 1
        ) {
            const saved =
                await saveCurrentLine();

            if (!saved) {
                return;
            }

            /*
             * Move to next PO line.
             */
            if (
                selectedLineIndex <
                lines.length - 1
            ) {
                const nextLine =
                    lines[
                    selectedLineIndex + 1
                    ];

                setSelectedLineId(
                    nextLine.purchaseOrderItemId
                );

                setCurrentStep(0);

                return;
            }

            /*
             * All lines completed.
             */
            return;
        }

        /*
         * Verify step requires validation.
         */
        if (currentStep === 1) {
            const verified =
                verifyProduct();

            if (!verified) {
                return;
            }

            return;
        }

        /*
         * Quantity validation before leaving
         * the quantity step.
         */
        if (currentStep === 2) {
            if (
                selectedLine.receivedQuantity <= 0
            ) {
                setError(
                    "Received quantity must be greater than zero."
                );

                return;
            }

            if (
                selectedLine.receivedQuantity >
                selectedLine.remainingQuantity
            ) {
                setError(
                    "Received quantity cannot exceed the remaining quantity."
                );

                return;
            }
        }

        /*
         * Damaged quantity validation.
         */
        if (currentStep === 6) {
            if (
                selectedLine.damagedQuantity >
                selectedLine.receivedQuantity
            ) {
                setError(
                    "Damaged quantity cannot exceed received quantity."
                );

                return;
            }
        }

        setCurrentStep(
            (current) =>
                Math.min(
                    current + 1,
                    RECEIVING_STEPS.length - 1
                )
        );
    }, [
        selectedLine,
        currentStep,
        saveCurrentLine,
        selectedLineIndex,
        lines,
        verifyProduct,
    ]);

    /*
     * =====================================================
     * PREVIOUS STEP
     * =====================================================
     */

    const previousStep = useCallback(() => {
        setError("");

        if (currentStep > 0) {
            setCurrentStep(
                (current) =>
                    current - 1
            );

            return;
        }

        /*
         * At the first step, go to previous PO line.
         */
        if (selectedLineIndex > 0) {
            const previousLine =
                lines[
                selectedLineIndex - 1
                ];

            setSelectedLineId(
                previousLine.purchaseOrderItemId
            );

            setCurrentStep(
                RECEIVING_STEPS.length - 1
            );
        }
    }, [
        currentStep,
        selectedLineIndex,
        lines,
    ]);

    /*
     * =====================================================
     * SELECT LINE
     * =====================================================
     */

    const selectLine = useCallback(
        (purchaseOrderItemId: number) => {
            setSelectedLineId(
                purchaseOrderItemId
            );

            setCurrentStep(0);
            setError("");
            setScanError("");
        },
        []
    );

    /*
     * =====================================================
     * REVIEW
     * =====================================================
     */

    const scannedLines = useMemo(
        () =>
            lines.filter(
                (line) =>
                    line.isSaved
            ).length,
        [lines]
    );

    const completedLines = useMemo(
        () =>
            lines.filter(
                (line) =>
                    line.receivingStatus ===
                    "Complete"
            ).length,
        [lines]
    );

    const partialLines = useMemo(
        () =>
            lines.filter(
                (line) =>
                    line.receivingStatus ===
                    "Partial"
            ).length,
        [lines]
    );

    const allLinesSaved =
        lines.length > 0 &&
        lines.every(
            (line) => line.isSaved
        );

    /*
     * =====================================================
     * RETURN
     * =====================================================
     */

    return {
    po,
    poItems,
    receipt,
    receiptItems,

    warehouses,
    categories,
    units,

    selectedWarehouseId,
    setSelectedWarehouseId,

    receivedBy,

    lines,
    selectedLine,
    selectedLineId,

    currentStep,
    setCurrentStep,

    loading,
    saving,

    error,
    scanError,

    scannedLines,
    completedLines,
    partialLines,
    allLinesSaved,

    selectLine,
    updateLine,

    scanProduct,
    createNewReceivingProduct,

    verifyProduct,

    saveCurrentLine,

    nextStep,
    previousStep,

    reload: loadReceiving,
};
}