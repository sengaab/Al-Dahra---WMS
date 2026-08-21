import { apiFetch } from "./api";
import {
    Warehouse,
    CreateWarehouseRequest,
    UpdateWarehouseRequest,
} from "./types";

export async function getWarehouses(): Promise<Warehouse[]> {
    return apiFetch<Warehouse[]>(
        "/api/Warehouses/GetallWarehouses"
    );
}

export async function getWarehouse(
    id: number
): Promise<Warehouse> {
    return apiFetch<Warehouse>(
        `/api/Warehouses/${id}/GetWarehousebyid`
    );
}

export async function getWarehouseByCode(
    code: string
): Promise<Warehouse> {
    return apiFetch<Warehouse>(
        `/api/Warehouses/GetWarehouseCode/${encodeURIComponent(code)}`
    );
}

export async function searchWarehouses(
    query: string
): Promise<Warehouse[]> {
    return apiFetch<Warehouse[]>(
        `/api/Warehouses/Search?search=${encodeURIComponent(query)}`
    );
}

export async function createWarehouse(
    data: CreateWarehouseRequest
): Promise<Warehouse> {
    return apiFetch<Warehouse>(
        "/api/Warehouses/Create",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateWarehouse(
    id: number,
    data: UpdateWarehouseRequest
): Promise<Warehouse> {
    return apiFetch<Warehouse>(
        `/api/Warehouses/UpdateWarehouseby/${id}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteWarehouse(
    id: number
): Promise<void> {
    await apiFetch<void>(
        `/api/Warehouses/DeleteWarehouseby/${id}`,
        {
            method: "DELETE",
        }
    );
}

export async function activateWarehouse(
    id: number
): Promise<Warehouse> {
    return apiFetch<Warehouse>(
        `/api/Warehouses/ActivateWarehouseby/${id}`,
        {
            method: "PATCH",
        }
    );
}

export async function deactivateWarehouse(
    id: number
): Promise<Warehouse> {
    return apiFetch<Warehouse>(
        `/api/Warehouses/${id}/deactivate`,
        {
            method: "PATCH",
        }
    );
}