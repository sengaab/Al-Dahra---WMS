import { apiFetch } from "@/lib/api";
import type {
    CreateStockAdjustmentDto,
    StockAdjustmentActionDto,
    StockAdjustmentActionResponseDto,
    StockAdjustmentResponseDto,
} from "@/types/stock-adjustment";

export async function getStockAdjustments(): Promise<
    StockAdjustmentResponseDto[]
> {
    return apiFetch<StockAdjustmentResponseDto[]>(
        "/api/stock-adjustments"
    );
}

export async function getStockAdjustmentById(
    adjustmentId: number
): Promise<StockAdjustmentResponseDto> {
    return apiFetch<StockAdjustmentResponseDto>(
        `/api/stock-adjustments/${adjustmentId}`
    );
}

export async function createStockAdjustment(
    data: CreateStockAdjustmentDto
): Promise<StockAdjustmentResponseDto> {
    return apiFetch<StockAdjustmentResponseDto>(
        "/api/stock-adjustments",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function submitStockAdjustment(
    adjustmentId: number
): Promise<StockAdjustmentActionResponseDto> {
    return apiFetch<StockAdjustmentActionResponseDto>(
        `/api/stock-adjustments/${adjustmentId}/submit`,
        {
            method: "POST",
        }
    );
}

export async function approveStockAdjustment(
    adjustmentId: number,
    data: StockAdjustmentActionDto
): Promise<StockAdjustmentActionResponseDto> {
    return apiFetch<StockAdjustmentActionResponseDto>(
        `/api/stock-adjustments/${adjustmentId}/approve`,
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function rejectStockAdjustment(
    adjustmentId: number,
    data: StockAdjustmentActionDto
): Promise<StockAdjustmentActionResponseDto> {
    return apiFetch<StockAdjustmentActionResponseDto>(
        `/api/stock-adjustments/${adjustmentId}/reject`,
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function applyStockAdjustment(
    adjustmentId: number
): Promise<StockAdjustmentActionResponseDto> {
    return apiFetch<StockAdjustmentActionResponseDto>(
        `/api/stock-adjustments/${adjustmentId}/apply`,
        {
            method: "POST",
        }
    );
}