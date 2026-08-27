"use client";

import { useCallback, useEffect, useState } from "react";
import {
    getStock,
    getStockSummary,
} from "@/lib/api/stock";

import type {
    StockDto,
    StockSummaryDto,
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