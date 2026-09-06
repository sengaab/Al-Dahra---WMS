import { apiFetch } from "@/lib/api";
import type {
    CreateWarehouseDto,
    DeleteWarehouseResponseDto,
    UpdateWarehouseDto,
    WarehouseDto,
    WarehouseOccupancyDto,
    WarehouseStatsDto,
} from "@/types/warehouse";

export interface GetWarehousesParams {
    siteId?: number;
    search?: string;
    status?: string;
    page?: number;
    pageSize?: number;
}

export async function getWarehouses(
    params?: GetWarehousesParams
): Promise<WarehouseDto[]> {
    const query = new URLSearchParams();

    if (params?.siteId !== undefined) {
        query.set("siteId", params.siteId.toString());
    }

    if (params?.search) {
        query.set("search", params.search);
    }

    if (params?.status) {
        query.set("status", params.status);
    }

    if (params?.page !== undefined) {
        query.set("page", params.page.toString());
    }

    if (params?.pageSize !== undefined) {
        query.set("pageSize", params.pageSize.toString());
    }

    const queryString = query.toString();

    return apiFetch<WarehouseDto[]>(
        `/api/Warehouses${queryString ? `?${queryString}` : ""}`
    );
}

export async function getWarehouseById(
    warehouseId: number
): Promise<WarehouseDto> {
    return apiFetch<WarehouseDto>(
        `/api/Warehouses/${warehouseId}`
    );
}

export async function createWarehouse(
    data: CreateWarehouseDto
): Promise<WarehouseDto> {
    return apiFetch<WarehouseDto>("/api/Warehouses", {
        method: "POST",
        body: JSON.stringify(data),
    });
}

export async function updateWarehouse(
    warehouseId: number,
    data: UpdateWarehouseDto
): Promise<WarehouseDto> {
    return apiFetch<WarehouseDto>(
        `/api/Warehouses/${warehouseId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteWarehouse(
    warehouseId: number
): Promise<DeleteWarehouseResponseDto> {
    return apiFetch<DeleteWarehouseResponseDto>(
        `/api/Warehouses/${warehouseId}`,
        {
            method: "DELETE",
        }
    );
}

export async function getWarehouseInventory(
    warehouseId: number
) {
    return apiFetch(
        `/api/Warehouses/${warehouseId}/inventory`
    );
}

export async function getWarehouseStats(
    warehouseId: number
): Promise<WarehouseStatsDto> {
    return apiFetch<WarehouseStatsDto>(
        `/api/Warehouses/${warehouseId}/stats`
    );
}

export async function getWarehouseOccupancy(
    warehouseId: number
): Promise<WarehouseOccupancyDto> {
    return apiFetch<WarehouseOccupancyDto>(
        `/api/Warehouses/${warehouseId}/occupancy`
    );
}