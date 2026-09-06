import { apiFetch } from "@/lib/api";
import type {
    CreateStockDto,
    DeleteStockResponseDto,
    StockDto,
    StockSummaryDto,
    StockTotalQuantityResponseDto,
    StockTotalValueResponseDto,
    UpdateStockDto,
} from "@/types/stock";

export async function getStock(): Promise<StockDto[]> {
    return apiFetch<StockDto[]>(
        "/api/Stock"
    );
}

export async function getStockById(
    stockId: number
): Promise<StockDto> {
    return apiFetch<StockDto>(
        `/api/Stock/${stockId}`
    );
}

export async function getStockByProductId(
    productId: number
): Promise<StockDto[]> {
    return apiFetch<StockDto[]>(
        `/api/Stock/product/${productId}`
    );
}

export async function getStockByLocationId(
    locationId: number
): Promise<StockDto[]> {
    return apiFetch<StockDto[]>(
        `/api/Stock/location/${locationId}`
    );
}

export async function getStockByWarehouseId(
    warehouseId: number
): Promise<StockDto[]> {
    return apiFetch<StockDto[]>(
        `/api/Stock/warehouse/${warehouseId}`
    );
}

export async function getAvailableStock(): Promise<StockDto[]> {
    return apiFetch<StockDto[]>(
        "/api/Stock/available"
    );
}

export async function getLowStock(): Promise<StockDto[]> {
    return apiFetch<StockDto[]>(
        "/api/Stock/low"
    );
}

export async function getOutOfStock(): Promise<StockDto[]> {
    return apiFetch<StockDto[]>(
        "/api/Stock/out-of-stock"
    );
}

export async function getStockSummary(): Promise<StockSummaryDto> {
    return apiFetch<StockSummaryDto>(
        "/api/Stock/summary"
    );
}

export async function getTotalStockQuantity(): Promise<number> {
    const response =
        await apiFetch<StockTotalQuantityResponseDto>(
            "/api/Stock/total-quantity"
        );

    return response.totalQuantity;
}

export async function getTotalStockValue(): Promise<number> {
    const response =
        await apiFetch<StockTotalValueResponseDto>(
            "/api/Stock/total-value"
        );

    return response.totalValue;
}

export async function createStock(
    data: CreateStockDto
): Promise<StockDto> {
    return apiFetch<StockDto>(
        "/api/Stock",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateStock(
    stockId: number,
    data: UpdateStockDto
): Promise<StockDto> {
    return apiFetch<StockDto>(
        `/api/Stock/${stockId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteStock(
    stockId: number
): Promise<DeleteStockResponseDto> {
    return apiFetch<DeleteStockResponseDto>(
        `/api/Stock/${stockId}`,
        {
            method: "DELETE",
        }
    );
}