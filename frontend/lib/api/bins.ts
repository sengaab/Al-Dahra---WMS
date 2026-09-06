import { apiFetch } from "@/lib/api";
import type {
    BinDto,
    CreateBinDto,
    UpdateBinDto,
} from "@/types/bin";

export interface GetBinsParams {
    warehouseId?: number;
    partitionId?: number;
    search?: string;
    status?: string;
    page?: number;
    pageSize?: number;
}

/**
 * GET /api/Bins
 *
 * Get bins with optional filtering and pagination.
 */
export async function getBins(
    params?: GetBinsParams
): Promise<BinDto[]> {
    const query = new URLSearchParams();

    if (params?.warehouseId !== undefined) {
        query.set(
            "warehouseId",
            params.warehouseId.toString()
        );
    }

    if (params?.partitionId !== undefined) {
        query.set(
            "partitionId",
            params.partitionId.toString()
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
        query.set(
            "pageSize",
            params.pageSize.toString()
        );
    }

    const queryString = query.toString();

    return apiFetch<BinDto[]>(
        `/api/Bins${queryString ? `?${queryString}` : ""}`
    );
}

/**
 * GET /api/Bins/{id}
 *
 * Get a single bin by ID.
 */
export async function getBinById(
    binId: number
): Promise<BinDto> {
    return apiFetch<BinDto>(
        `/api/Bins/${binId}`
    );
}

/**
 * GET /api/Bins/warehouse/{warehouseId}
 *
 * Get all bins belonging to a warehouse.
 */
export async function getBinsByWarehouseId(
    warehouseId: number
): Promise<BinDto[]> {
    return apiFetch<BinDto[]>(
        `/api/Bins/warehouse/${warehouseId}`
    );
}

/**
 * GET /api/Bins/partition/{partitionId}
 *
 * Get all bins belonging to a partition.
 */
export async function getBinsByPartitionId(
    partitionId: number
): Promise<BinDto[]> {
    return apiFetch<BinDto[]>(
        `/api/Bins/partition/${partitionId}`
    );
}

/**
 * GET /api/Bins/location/{locationId}
 *
 * Get bins associated with a location.
 */
export async function getBinsByLocationId(
    locationId: number
): Promise<BinDto[]> {
    return apiFetch<BinDto[]>(
        `/api/Bins/location/${locationId}`
    );
}

/**
 * POST /api/Bins
 *
 * Create a new bin.
 */
export async function createBin(
    data: CreateBinDto
): Promise<BinDto> {
    return apiFetch<BinDto>("/api/Bins", {
        method: "POST",
        body: JSON.stringify(data),
    });
}

/**
 * PUT /api/Bins/{id}
 *
 * Update an existing bin.
 */
export async function updateBin(
    binId: number,
    data: UpdateBinDto
): Promise<BinDto> {
    return apiFetch<BinDto>(
        `/api/Bins/${binId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

/**
 * DELETE /api/Bins/{id}
 */
export async function deleteBin(
    binId: number
): Promise<{ message: string }> {
    return apiFetch<{ message: string }>(
        `/api/Bins/${binId}`,
        {
            method: "DELETE",
        }
    );
}