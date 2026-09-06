import { apiFetch } from "@/lib/api";
import type {
    CreatePartitionDto,
    PartitionDto,
    PartitionSummaryDto,
    UpdatePartitionDto,
} from "@/types/partition";

export interface GetPartitionsParams {
    warehouseId?: number;
    search?: string;
    status?: string;
    page?: number;
    pageSize?: number;
}

export async function getPartitions(
    params?: GetPartitionsParams
): Promise<PartitionDto[]> {
    const query = new URLSearchParams();

    if (params?.warehouseId !== undefined) {
        query.set(
            "warehouseId",
            params.warehouseId.toString()
        );
    }

    if (params?.search) {
        query.set("search", params.search);
    }

    if (params?.status) {
        query.set("status", params.status);
    }

    if (params?.page !== undefined) {
        query.set("page", params.page.toString());
    }

    if (params?.pageSize !== undefined) {
        query.set("pageSize", params.pageSize.toString());
    }

    const queryString = query.toString();

    return apiFetch<PartitionDto[]>(
        `/api/Partitions${queryString ? `?${queryString}` : ""}`
    );
}

export async function getPartitionById(
    partitionId: number
): Promise<PartitionDto> {
    return apiFetch<PartitionDto>(
        `/api/Partitions/${partitionId}`
    );
}

export async function getPartitionsByWarehouseId(
    warehouseId: number
): Promise<PartitionDto[]> {
    return apiFetch<PartitionDto[]>(
        `/api/Partitions/warehouse/${warehouseId}`
    );
}

export async function getPartitionSummary(
    warehouseId?: number
): Promise<PartitionSummaryDto[]> {
    const query = new URLSearchParams();

    if (warehouseId !== undefined) {
        query.set("warehouseId", warehouseId.toString());
    }

    const queryString = query.toString();

    return apiFetch<PartitionSummaryDto[]>(
        `/api/Partitions/summary${
            queryString ? `?${queryString}` : ""
        }`
    );
}

export async function createPartition(
    data: CreatePartitionDto
): Promise<PartitionDto> {
    return apiFetch<PartitionDto>(
        "/api/Partitions",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updatePartition(
    partitionId: number,
    data: UpdatePartitionDto
): Promise<PartitionDto> {
    return apiFetch<PartitionDto>(
        `/api/Partitions/${partitionId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deletePartition(
    partitionId: number
): Promise<{ message: string }> {
    return apiFetch<{ message: string }>(
        `/api/Partitions/${partitionId}`,
        {
            method: "DELETE",
        }
    );
}