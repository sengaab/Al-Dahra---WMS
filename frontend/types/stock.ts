export interface StockDto {
    stockId: number;

    // Product
    productId: number;
    productName: string;
    categoryName: string;
    SKU: string;
    barcode: string | null;

    // Warehouse
    warehouseId: number;
    warehouseName: string;

    // Partition
    partitionId: number | null;
    partitionName: string | null;
    partitionCode: string | null;

    // Bin
    binId: number | null;
    binName: string | null;
    binCode: string | null;

    // Location
    locationId: number | null;
    locationName: string | null;
    locationCode: string | null;

    // Supplier
    supplierId: number | null;
    supplierName: string | null;

    // Stock details
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
    supplierId?: number | null;

    batchNumber?: string | null;
    expiryDate?: string | null;

    quantity: number;
    reservedQuantity: number;

    unitPrice: number;
    minimumStock: number;
}

export interface UpdateStockDto {
    locationId?: number | null;
    supplierId?: number | null;

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

export interface StockTotalQuantityResponseDto {
    totalQuantity: number;
}

export interface StockTotalValueResponseDto {
    totalValue: number;
}

export interface DeleteStockResponseDto {
    message: string;
}