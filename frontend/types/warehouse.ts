export interface WarehouseDto {
    warehouseId: number;
    siteId: number;
    siteName: string;
    code: string;
    name: string;
    description: string | null;
    isActive: boolean;
    createdAt: string;
    updatedAt: string;
    partitionsCount: number;
    binsCount: number;
    locationsCount: number;
}

export interface CreateWarehouseDto {
    siteId: number;
    code: string;
    name: string;
    description?: string | null;
}

export interface UpdateWarehouseDto {
    siteId?: number | null;
    code?: string | null;
    name?: string | null;
    description?: string | null;
    isActive?: boolean | null;
}

export interface WarehouseStatsDto {
    warehouseId: number;
    warehouseName: string;

    // Structure
    totalPartitions: number;
    totalBins: number;
    totalLocations: number;
    activeLocations: number;
    inactiveLocations: number;

    // Stock
    totalStockItems: number;
    totalQuantity: number;
    totalReservedQuantity: number;
    totalAvailableQuantity: number;
    totalValue: number;
}

export interface WarehouseOccupancyDto {
    warehouseId: number;
    warehouseName: string;
    totalLocations: number;
    occupiedLocations: number;
    emptyLocations: number;
    occupancyPercentage: number;
}

export interface DeleteWarehouseResponseDto {
    message: string;
}