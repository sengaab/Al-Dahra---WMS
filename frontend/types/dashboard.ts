export interface DashboardDto {
    stats: DashboardStatsDto;
    warehouseOverview: WarehouseOverviewDto[];
    stockStatus: StockStatusDto[];
    lowStock: LowStockDto[];
    valueByCategory: ValueByCategoryDto[];
    recentTransactions: RecentTransactionDto[];
    pendingReceipts: PendingItemDto[];
    pendingRequests: PendingItemDto[];
    pendingPickLists: PendingItemDto[];
    pendingTransfers: PendingItemDto[];
}

export interface DashboardStatsDto {
    totalProducts: number;
    totalStockItems: number;
    totalQuantity: number;
    totalStockValue: number;
    lowStockItems: number;
    outOfStockItems: number;
    activeWarehouses: number;
}

export interface WarehouseOverviewDto {
    warehouseId: number;
    warehouseName: string;
    stockItems: number;
    quantity: number;
    totalValue: number;
}

export interface StockStatusDto {
    status: string;
    count: number;
    quantity: number;
}

export interface LowStockDto {
    stockId: number;
    productId: number;
    productName: string;
    sku: string;
    warehouseId: number;
    warehouseName: string;
    quantity: number;
    availableQuantity: number;
    minimumStock: number;
}

export interface ValueByCategoryDto {
    categoryId: number | null;
    categoryName: string;
    totalValue: number;
}

export interface RecentTransactionDto {
    transactionId: number;
    productId: number;
    productName: string;
    transactionType: string;
    quantity: number;
    referenceType: string | null;
    createdAt: string;
}

export interface PendingItemDto {
    id: number;
    number: string;
    status: string;
    createdAt: string;
}