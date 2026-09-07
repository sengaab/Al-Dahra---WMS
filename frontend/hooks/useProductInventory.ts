"use client";

import { useCallback, useEffect, useState } from "react";
import { getProductById } from "@/lib/api/products";
import { getStockByProductId } from "@/lib/api/stock";
import type { StockDto } from "@/types";

export interface ProductInventoryStock {
    stockId: string;

    location: {
        warehouse: string | null;
        partition: string | null;
        bin: string | null;
    };

    available: number;
    reserved: number;

    supplier: string | null;
    batch: string | null;

    quantity: number;
    price: number;
    expiry: string | null;
    status: string;
}

export interface ProductInventoryDetails {
    productName: string;
    sku: string;
    barcode: string | null;
    category: string | null;
    unit: string | null;

    totalQuantity: number;
    totalAvailable: number;
    totalReserved: number;
    stockValue: number;
    numberOfLocations: number;
    minimumStock: number;
    status: string;

    stock: ProductInventoryStock[];
}

interface UseProductInventoryResult {
    data: ProductInventoryDetails | null;
    loading: boolean;
    error: string | null;
    refetch: () => Promise<void>;
}

export function useProductInventory(
    productId: number | null
): UseProductInventoryResult {
    const [data, setData] =
        useState<ProductInventoryDetails | null>(null);

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const fetchData = useCallback(async () => {
        if (productId === null) {
            setData(null);
            return;
        }

        try {
            setLoading(true);
            setError(null);

            const [product, stockRows] = await Promise.all([
                getProductById(productId),
                getStockByProductId(productId),
            ]);

            // -------------------------------------------------
            // Totals
            // -------------------------------------------------

            const totalQuantity = stockRows.reduce(
                (total, item) => total + item.quantity,
                0
            );

            const totalAvailable = stockRows.reduce(
                (total, item) => total + item.availableQuantity,
                0
            );

            const totalReserved = stockRows.reduce(
                (total, item) => total + item.reservedQuantity,
                0
            );

            const stockValue = stockRows.reduce(
                (total, item) =>
                    total + item.quantity * item.unitPrice,
                0
            );

            // -------------------------------------------------
            // Distinct locations
            // -------------------------------------------------

            const locationIds = new Set(
                stockRows
                    .map((item) => item.locationId)
                    .filter(
                        (locationId): locationId is number =>
                            locationId !== null
                    )
            );

            const numberOfLocations = locationIds.size;

            // -------------------------------------------------
            // Product information
            // -------------------------------------------------

            const minimumStock = product.minimumStock;

            // -------------------------------------------------
            // Overall status
            // -------------------------------------------------

            let status = "Available";

            if (
                stockRows.some(
                    (item) => item.stockStatus === "Quarantined"
                )
            ) {
                status = "Quarantined";
            } else if (
                stockRows.some(
                    (item) => item.stockStatus === "Damaged"
                )
            ) {
                status = "Damaged";
            } else if (
                stockRows.some(
                    (item) => item.stockStatus === "Expired"
                )
            ) {
                status = "Expired";
            } else if (totalQuantity === 0) {
                status = "Out of Stock";
            } else if (totalQuantity < minimumStock) {
                status = "Low Stock";
            }

            // -------------------------------------------------
            // Stock by location
            // -------------------------------------------------

            const stockDetails: ProductInventoryStock[] =
                stockRows.map((item) => ({
                    stockId: item.stockCode,

                    location: {
                        warehouse: item.warehouseName ?? null,

                        // Current backend provides partition information.
                        // Map it to "room" for the existing UI shape.
                        partition: item.partitionName ?? null,

                        bin: item.binName ?? null,
                    },

                    available: item.availableQuantity,
                    reserved: item.reservedQuantity,

                    supplier: item.supplierName ?? null,
                    batch: item.batchNumber ?? null,

                    quantity: item.quantity,
                    price: item.unitPrice,
                    expiry: item.expiryDate ?? null,

                    status:
                        item.quantity === 0
                            ? "Out of Stock"
                            : item.stockStatus,
                }));

            // -------------------------------------------------
            // Final data
            // -------------------------------------------------

            setData({
                productName: product.name,
                sku: product.sku,
                barcode: product.barcode,
                category: product.categoryName,
                unit: product.unitName,

                totalQuantity,
                totalAvailable,
                totalReserved,
                stockValue,
                numberOfLocations,
                minimumStock,
                status,

                stock: stockDetails,
            });
        } catch (err) {
            console.error(
                "Failed to load product inventory:",
                err
            );

            setError(
                err instanceof Error
                    ? err.message
                    : "Failed to load product inventory"
            );

            setData(null);
        } finally {
            setLoading(false);
        }
    }, [productId]);

    useEffect(() => {
        fetchData();
    }, [fetchData]);

    return {
        data,
        loading,
        error,
        refetch: fetchData,
    };
}