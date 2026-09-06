export interface CreateStockCountDto {
    warehouseId: number;
    locationId?: number | null;
    createdBy: string;
    countDate: string;
    items: CreateStockCountItemDto[];
}

export interface CreateStockCountItemDto {
    stockId: number;
    productId: number;
}

export interface UpdateStockCountDto {
    warehouseId: number;
    locationId?: number | null;
    countDate: string;
    items: CreateStockCountItemDto[];
}

export interface StockCountResponseDto {
    stockCountId: number;
    countNumber: string;

    warehouseId: number;
    locationId: number | null;

    createdBy: string;
    approvedBy: string | null;

    stockCountStatus: string;

    countDate: string;

    items: StockCountItemResponseDto[];
}

export interface StockCountItemResponseDto {
    stockCountItemId: number;

    stockId: number;
    productId: number;

    expectedQuantity: number;
    countedQuantity: number;
    variance: number;

    reason: string | null;
}

export interface CountStockCountItemDto {
    countedQuantity: number;
    reason?: string | null;
}

export interface ApproveStockCountDto {
    approvedBy: string;
}