import { apiFetch } from "./api";

export function getStock() {
    return apiFetch("/api/Stock");
}

export function createStock(data: unknown) {
    return apiFetch("/api/Stock", {
        method: "POST",
        body: JSON.stringify(data),
    });
}

export function getStockById(id: string | number) {
    return apiFetch(`/api/Stock/${id}`);
}

export function updateStock(id: string | number, data: unknown) {
    return apiFetch(`/api/Stock/${id}`, {
        method: "PUT",
        body: JSON.stringify(data),
    });
}

export function deleteStock(id: string | number) {
    return apiFetch(`/api/Stock/${id}`, {
        method: "DELETE",
    });
}

export function updateStockStatus(
    id: string | number,
    data: unknown
) {
    return apiFetch(`/api/Stock/${id}/status`, {
        method: "PATCH",
        body: JSON.stringify(data),
    });
}