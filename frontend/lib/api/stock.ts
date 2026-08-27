import { apiFetch } from "@/lib/api";
import type {
    StockDto,
    CreateStockDto,
    UpdateStockDto,
    StockSummaryDto,
} from "@/types";

export async function getStock(): Promise<StockDto[]> {
    return apiFetch<StockDto[]>("/api/stock");
}

export async function getStockById(
    stockId: number
): Promise<StockDto> {
    return apiFetch<StockDto>(
        `/api/stock/${stockId}`
    );
}

export async function createStock(
    data: CreateStockDto
): Promise<StockDto> {
    return apiFetch<StockDto>("/api/stock", {
        method: "POST",
        body: JSON.stringify(data),
    });
}

export async function updateStock(
    stockId: number,
    data: UpdateStockDto
): Promise<StockDto> {
    return apiFetch<StockDto>(
        `/api/stock/${stockId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteStock(
    stockId: number
): Promise<void> {
    return apiFetch<void>(
        `/api/stock/${stockId}`,
        {
            method: "DELETE",
        }
    );
}

export async function getStockSummary(): Promise<StockSummaryDto> {
    return apiFetch<StockSummaryDto>(
        "/api/stock/summary"
    );
}