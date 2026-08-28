export interface StockDto {
    stockId: number;
    productId: number;
    productName: string;
    categoryName: string;
    sku: string;
    warehouseId: number;
    warehouseName: string;
    locationId: number | null;
    locationName: string | null;
    stockCode: string;
    batchNumber: string | null;
    expiryDate: string | null;
    quantity: number;
    reservedQuantity: number;
    availableQuantity: number;
    unitPrice: number;
    minimumStock: number;
    stockStatus: string;
    createdAt: string;
    updatedAt: string;
}

export interface CreateStockDto {
    productId: number;
    warehouseId: number;
    locationId?: number | null;
    batchNumber?: string | null;
    expiryDate?: string | null;
    quantity: number;
    reservedQuantity: number;
    unitPrice: number;
    minimumStock: number;
}

export interface UpdateStockDto {
    locationId?: number | null;
    batchNumber?: string | null;
    expiryDate?: string | null;
    quantity: number;
    reservedQuantity: number;
    unitPrice: number;
    minimumStock: number;
    stockStatus?: string | null;
}

export interface StockSummaryDto {
    totalStockItems: number;
    totalQuantity: number;
    totalReservedQuantity: number;
    totalAvailableQuantity: number;
    totalValue: number;
    availableItems: number;
    quarantinedItems: number;
    damagedItems: number;
    expiredItems: number;
    blockedItems: number;
    lowStockItems: number;
    outOfStockItems: number;
}

export interface StockByProductDto {
    stockId: number;
    productId: number;
    productName: string;
    categoryName: string;
    sku: string;
    warehouseId: number;
    warehouseName: string;
    locationId: number;
    locationName: string;
    stockCode: string;
    batchNumber: string | null;
    expiryDate: string | null;
    quantity: number;
    reservedQuantity: number;
    availableQuantity: number;
    unitPrice: number;
    minimumStock: number;
    stockStatus: string;
    createdAt: string;
    updatedAt: string;
}