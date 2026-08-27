import { apiFetch } from "@/lib/api";
import type { DashboardDto } from "@/types";

export interface DashboardFilters {
    siteId?: number;
    departmentId?: number;
}

export async function getDashboard(
    filters?: DashboardFilters
): Promise<DashboardDto> {
    const params = new URLSearchParams();

    if (filters?.siteId !== undefined) {
        params.append("siteId", filters.siteId.toString());
    }

    if (filters?.departmentId !== undefined) {
        params.append(
            "departmentId",
            filters.departmentId.toString()
        );
    }

    const query = params.toString();

    return apiFetch<DashboardDto>(
        `/api/dashboard${query ? `?${query}` : ""}`
    );
}