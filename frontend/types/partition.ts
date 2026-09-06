export interface PartitionDto {
    partitionId: number;
    warehouseId: number;
    warehouseName: string;
    code: string;
    name: string;
    description: string | null;
    isActive: boolean;
    createdAt: string;
    updatedAt: string;
    binsCount: number;
}

export interface CreatePartitionDto {
    warehouseId: number;
    code: string;
    name: string;
    description?: string | null;
}

export interface UpdatePartitionDto {
    warehouseId?: number | null;
    code?: string | null;
    name?: string | null;
    description?: string | null;
    isActive?: boolean | null;
}

export interface PartitionSummaryDto {
    partitionId: number;
    code: string;
    name: string;
    warehouseId: number;
    warehouseName: string;
    binsCount: number;
    isActive: boolean;
}