import { apiFetch } from "./api";

export async function getWarehouses() {
    return apiFetch("/api/Warehouses/GetallWarehouses");
}

export async function getWarehouseById(id: number) {
    return apiFetch(`/api/Warehouses/${id}/GetWarehousebyid`);
}

export async function getWarehouseByCode(code: string) {
    return apiFetch(`/api/Warehouses/GetWarehouseCode/${code}`);
}

export async function createWarehouse(data: unknown) {
    return apiFetch("/api/Warehouses/Create", {
        method: "POST",
        body: JSON.stringify(data),
    });
}

export async function updateWarehouse(id: number, data: unknown) {
    return apiFetch(`/api/Warehouses/UpdateWarehouseby${id}`, {
        method: "PUT",
        body: JSON.stringify(data),
    });
}

export async function deleteWarehouse(id: number) {
    return apiFetch(`/api/Warehouses/DeleteWarehouseby${id}`, {
        method: "DELETE",
    });
}

export async function activateWarehouse(id: number) {
    return apiFetch(`/api/Warehouses/ActivateWarehouseby${id}`, {
        method: "PATCH",
    });
}

export async function deactivateWarehouse(id: number) {
    return apiFetch(`/api/Warehouses/${id}/deactivate`, {
        method: "PATCH",
    });
}