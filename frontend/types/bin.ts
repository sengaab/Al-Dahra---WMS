export interface BinDto {
    binId: number;

    warehouseId: number;
    warehouseName: string;

    partitionId: number;
    partitionName: string;
    partitionCode: string;

    code: string;
    name: string;

    description: string | null;

    isActive: boolean;

    locationsCount: number;
    stockCount: number;
}

export interface CreateBinDto {
    warehouseId: number;
    partitionId: number;
    code: string;
    name: string;
    description?: string | null;
}

export interface UpdateBinDto {
    warehouseId?: number | null;
    partitionId?: number | null;
    code?: string | null;
    name?: string | null;
    description?: string | null;
    isActive?: boolean | null;
}