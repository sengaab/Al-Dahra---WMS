import { apiFetch } from "@/lib/api";
import type {
    CreateLocationDto,
    LocationDto,
    LocationOccupancyDto,
    LocationStructureDto,
    LocationTreeDto,
    UpdateLocationDto,
    WarehouseTreeDto,
} from "@/types/location";

export interface GetLocationsParams {
    warehouseId?: number;
    partitionId?: number;
    binId?: number;

    search?: string;
    type?: string;
    status?: string;

    page?: number;
    pageSize?: number;
}

/**
 * GET /api/locations
 *
 * Get locations with optional filters and pagination.
 */
export async function getLocations(
    params?: GetLocationsParams
): Promise<LocationDto[]> {
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

    if (params?.binId !== undefined) {
        query.set(
            "binId",
            params.binId.toString()
        );
    }

    if (params?.search) {
        query.set("search", params.search);
    }

    if (params?.type) {
        query.set("type", params.type);
    }

    if (params?.status) {
        query.set("status", params.status);
    }

    if (params?.page !== undefined) {
        query.set(
            "page",
            params.page.toString()
        );
    }

    if (params?.pageSize !== undefined) {
        query.set(
            "pageSize",
            params.pageSize.toString()
        );
    }

    const queryString = query.toString();

    return apiFetch<LocationDto[]>(
        `/api/locations${
            queryString ? `?${queryString}` : ""
        }`
    );
}

/**
 * GET /api/locations/{id}
 *
 * Get a location by ID.
 */
export async function getLocationById(
    locationId: number
): Promise<LocationDto> {
    return apiFetch<LocationDto>(
        `/api/locations/${locationId}`
    );
}

/**
 * GET /api/locations/{id}/structure
 *
 * Get the complete warehouse/partition/bin/location
 * structure for a location.
 */
export async function getLocationStructure(
    locationId: number
): Promise<LocationStructureDto> {
    return apiFetch<LocationStructureDto>(
        `/api/locations/${locationId}/structure`
    );
}

/**
 * POST /api/locations
 *
 * Create a new location.
 */
export async function createLocation(
    data: CreateLocationDto
): Promise<LocationDto> {
    return apiFetch<LocationDto>(
        "/api/locations",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

/**
 * PUT /api/locations/{id}
 *
 * Update a location.
 */
export async function updateLocation(
    locationId: number,
    data: UpdateLocationDto
): Promise<LocationDto> {
    return apiFetch<LocationDto>(
        `/api/locations/${locationId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

/**
 * DELETE /api/locations/{id}
 */
export async function deleteLocation(
    locationId: number
): Promise<{ message: string }> {
    return apiFetch<{ message: string }>(
        `/api/locations/${locationId}`,
        {
            method: "DELETE",
        }
    );
}

/**
 * GET /api/locations/{id}/inventory
 *
 * Get inventory/stock belonging to a location.
 *
 * The exact DTO returned by the backend is not included
 * in the supplied controller/DTO files, so the response
 * type is intentionally left generic for now.
 */
export async function getLocationInventory(
    locationId: number
) {
    return apiFetch(
        `/api/locations/${locationId}/inventory`
    );
}

/**
 * GET /api/locations/{id}/occupancy
 *
 * Get occupancy information for a location.
 */
export async function getLocationOccupancy(
    locationId: number
): Promise<LocationOccupancyDto> {
    return apiFetch<LocationOccupancyDto>(
        `/api/locations/${locationId}/occupancy`
    );
}

/**
 * GET /api/locations/tree
 *
 * Get the complete location tree.
 */
export async function getLocationTree(): Promise<
    LocationTreeDto[] | WarehouseTreeDto[]
> {
    return apiFetch<
        LocationTreeDto[] | WarehouseTreeDto[]
    >("/api/locations/tree");
}

/**
 * GET /api/locations/bin/{binId}
 *
 * Get all locations belonging to a bin.
 */
export async function getLocationsByBinId(
    binId: number
): Promise<LocationDto[]> {
    return apiFetch<LocationDto[]>(
        `/api/locations/bin/${binId}`
    );
}