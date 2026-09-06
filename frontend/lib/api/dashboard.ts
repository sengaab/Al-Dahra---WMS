import { apiFetch } from "@/lib/api";
import type { DashboardDto } from "@/types/dashboard";

export interface GetDashboardParams {
    siteId?: number;
    departmentId?: number;
    warehouseId?: number;
    fromDate?: string;
    toDate?: string;
}

/**
 * GET /api/Dashboard
 *
 * Get dashboard data with optional filters.
 */
export async function getDashboard(
    params?: GetDashboardParams
): Promise<DashboardDto> {
    const query = new URLSearchParams();

    if (params?.siteId !== undefined) {
        query.set("siteId", params.siteId.toString());
    }

    if (params?.departmentId !== undefined) {
        query.set(
            "departmentId",
            params.departmentId.toString()
        );
    }

    if (params?.warehouseId !== undefined) {
        query.set(
            "warehouseId",
            params.warehouseId.toString()
        );
    }

    if (params?.fromDate) {
        query.set("fromDate", params.fromDate);
    }

    if (params?.toDate) {
        query.set("toDate", params.toDate);
    }

    const queryString = query.toString();

    return apiFetch<DashboardDto>(
        `/api/Dashboard${queryString ? `?${queryString}` : ""}`
    );
}