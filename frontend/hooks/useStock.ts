"use client";

import { useCallback, useEffect, useState } from "react";
import {
    getStock,
    getStockSummary,
    getStockByProduct,
} from "@/lib/api/stock";

import type {
    StockDto,
    StockSummaryDto,
    StockByProductDto,
} from "@/types";

export function useStock() {
    const [stock, setStock] = useState<StockDto[]>([]);
    const [summary, setSummary] =
        useState<StockSummaryDto | null>(null);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const fetchStock = useCallback(async () => {
        try {
            setLoading(true);
            setError(null);

            const [stockData, summaryData] =
                await Promise.all([
                    getStock(),
                    getStockSummary(),
                ]);

            setStock(stockData);
            setSummary(summaryData);
        } catch (err) {
            setError(
                err instanceof Error
                    ? err.message
                    : "Failed to load stock"
            );
        } finally {
            setLoading(false);
        }
    }, []);

    useEffect(() => {
        fetchStock();
    }, [fetchStock]);

    return {
        stock,
        summary,
        loading,
        error,
        refetch: fetchStock,
    };
}

export function useStockByProductId(productId: number | null) {
    const [stock, setStock] = useState<StockByProductDto | null>(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const fetchStock = useCallback(async () => {
        if (productId === null) {
            setStock(null);
            return;
        }

        try {
            setLoading(true);
            setError(null);

            const data = await getStockByProduct(productId);
            setStock(data);
        } catch (err) {
            console.error("Failed to fetch stock by product:", err);
            setError(
                err instanceof Error
                    ? err.message
                    : "Failed to fetch stock"
            );
            setStock(null);
        } finally {
            setLoading(false);
        }
    }, [productId]);

    useEffect(() => {
        fetchStock();
    }, [fetchStock]);

    return {
        stock,
        loading,
        error,
        refetch: fetchStock,
    };
}