"use client";

import { useCallback, useEffect, useState } from "react";
import { getDashboard } from "@/lib/api/dashboard";
import type { DashboardDto } from "@/types";

interface UseDashboardOptions {
    siteId?: number;
    departmentId?: number;
}

export function useDashboard({
    siteId,
    departmentId,
}: UseDashboardOptions = {}) {
    const [data, setData] = useState<DashboardDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const fetchDashboard = useCallback(async () => {
        try {
            setLoading(true);
            setError(null);

            const result = await getDashboard({
                siteId,
                departmentId,
            });

            setData(result);
        } catch (err) {
            setError(
                err instanceof Error
                    ? err.message
                    : "Failed to load dashboard"
            );
        } finally {
            setLoading(false);
        }
    }, [siteId, departmentId]);

    useEffect(() => {
        fetchDashboard();
    }, [fetchDashboard]);

    return {
        data,
        loading,
        error,
        refetch: fetchDashboard,
    };
}