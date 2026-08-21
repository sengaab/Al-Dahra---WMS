import { apiFetch } from "./api";
import {
    Stock,
    CreateStockRequest,
    UpdateStockRequest,
    UpdateStockStatusRequest,
} from "./types";

export async function getStock(): Promise<Stock[]> {
    return apiFetch<Stock[]>("/api/Stock");
}

export async function getStockById(
    id: number
): Promise<Stock> {
    return apiFetch<Stock>(
        `/api/Stock/${id}`
    );
}

export async function createStock(
    data: CreateStockRequest
): Promise<Stock> {
    return apiFetch<Stock>(
        "/api/Stock",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateStock(
    id: number,
    data: UpdateStockRequest
): Promise<Stock> {
    return apiFetch<Stock>(
        `/api/Stock/${id}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteStock(
    id: number
): Promise<void> {
    await apiFetch<void>(
        `/api/Stock/${id}`,
        {
            method: "DELETE",
        }
    );
}

export async function updateStockStatus(
    id: number,
    data: UpdateStockStatusRequest
): Promise<Stock> {
    return apiFetch<Stock>(
        `/api/Stock/${id}/status`,
        {
            method: "PATCH",
            body: JSON.stringify(data),
        }
    );
}