export interface LocationDto {
    locationId: number;

    warehouseId: number | null;
    warehouseName: string | null;

    partitionId: number | null;
    partitionName: string | null;
    partitionCode: string | null;

    binId: number;
    binName: string | null;
    binCode: string | null;

    code: string;
    name: string;
    type: string;

    isActive: boolean;

    createdAt: string;
    updatedAt: string;

    childrenCount: number;
}

export interface CreateLocationDto {
    binId: number;

    code: string;
    name: string;
    type: string;
}

export interface UpdateLocationDto {
    binId?: number | null;

    code?: string | null;
    name?: string | null;
    type?: string | null;

    isActive?: boolean | null;
}

export interface LocationTreeDto {
    locationId: number;

    binId: number;
    partitionId: number;
    warehouseId: number;

    code: string;
    name: string;
    type: string;

    isActive: boolean;

    children: LocationTreeDto[];
}

export interface WarehouseTreeDto {
    warehouseId: number;

    code: string;
    name: string;

    isActive: boolean;

    locations: LocationTreeDto[];
}

export interface LocationOccupancyDto {
    locationId: number;

    locationName: string;
    locationType: string;

    totalStockItems: number;

    totalQuantity: number;
    totalReservedQuantity: number;
    totalAvailableQuantity: number;

    totalValue: number;

    isOccupied: boolean;
}

export interface LocationStructureDto {
    locationId: number;

    warehouseId: number;
    warehouseName: string | null;

    partitionId: number;
    partitionName: string | null;
    partitionCode: string | null;

    binId: number;
    binName: string | null;
    binCode: string | null;

    code: string;
    name: string;
    type: string;

    isActive: boolean;

    stockCount: number;
}