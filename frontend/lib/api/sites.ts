import { apiFetch } from "@/lib/api";
import type {
    SiteCreateDto,
    SiteDto,
    SiteInventoryDto,
    SiteStatsDto,
    SiteUpdateDto,
    SiteWarehouseDto,
} from "@/types/site";

export async function getSites(): Promise<SiteDto[]> {
    return apiFetch<SiteDto[]>("/api/sites");
}

export async function getSiteById(
    siteId: number
): Promise<SiteDto> {
    return apiFetch<SiteDto>(
        `/api/sites/${siteId}`
    );
}

export async function createSite(
    data: SiteCreateDto
): Promise<SiteDto> {
    return apiFetch<SiteDto>(
        "/api/sites",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateSite(
    siteId: number,
    data: SiteUpdateDto
): Promise<SiteDto> {
    return apiFetch<SiteDto>(
        `/api/sites/${siteId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteSite(
    siteId: number
): Promise<void> {
    return apiFetch<void>(
        `/api/sites/${siteId}`,
        {
            method: "DELETE",
        }
    );
}

export async function getSiteWarehouses(
    siteId: number
): Promise<SiteWarehouseDto[]> {
    return apiFetch<SiteWarehouseDto[]>(
        `/api/sites/${siteId}/warehouses`
    );
}

export async function getSiteInventory(
    siteId: number
): Promise<SiteInventoryDto[]> {
    return apiFetch<SiteInventoryDto[]>(
        `/api/sites/${siteId}/inventory`
    );
}

export async function getSiteStats(
    siteId: number
): Promise<SiteStatsDto> {
    return apiFetch<SiteStatsDto>(
        `/api/sites/${siteId}/stats`
    );
}