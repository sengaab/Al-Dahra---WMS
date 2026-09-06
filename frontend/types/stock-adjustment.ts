export type StockAdjustmentStatus =
    | "Pending"
    | "Approved"
    | "Rejected"
    | "Applied";

export interface CreateStockAdjustmentDto {
    stockId: number;
    productId: number;
    adjustmentQuantity: number;
    reason: string;
    createdBy: string;
}

export interface StockAdjustmentResponseDto {
    adjustmentId: number;
    adjustmentNumber: string;

    stockId: number;
    productId: number;

    previousQuantity: number;
    adjustmentQuantity: number;
    newQuantity: number;

    reason: string;

    createdBy: string;
    approvedBy: string | null;

    createdAt: string;

    stockAdjustmentStatus: StockAdjustmentStatus;
}

export interface StockAdjustmentActionDto {
    userId: string;
}

export interface StockAdjustmentActionResponseDto {
    message: string;
    adjustment: StockAdjustmentResponseDto;
}