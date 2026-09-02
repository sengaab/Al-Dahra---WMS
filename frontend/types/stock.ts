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
    roomId: number;
    roomName: string | null;
    roomCode: string | null;
    rackId: number;
    rackName: string | null;
    rackCode: string | null;
    shelfId: number;
    shelfName: string | null;
    shelfCode: string | null;
    binId: number;
    binName: null;
    binCode: string | null;
    supplierId: number;
    supplierName: string | null;
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
    roomId: number;
    roomName: string | null;
    roomCode: string | null;
    rackId: number;
    rackName: string | null;
    rackCode: string | null;
    shelfId: number;
    shelfName: string | null;
    shelfCode: string | null;
    binId: number;
    binName: null;
    binCode: string | null;
    supplierId: number;
    supplierName: string | null;
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