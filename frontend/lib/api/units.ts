import { apiFetch } from "@/lib/api";
import type {
    CreateUnitDto,
    DeleteUnitResponseDto,
    UnitDto,
    UpdateUnitDto,
} from "@/types/unit";

export async function getUnits(): Promise<UnitDto[]> {
    return apiFetch<UnitDto[]>(
        "/api/Units"
    );
}

export async function getUnitById(
    unitId: number
): Promise<UnitDto> {
    return apiFetch<UnitDto>(
        `/api/Units/${unitId}`
    );
}

export async function createUnit(
    data: CreateUnitDto
): Promise<UnitDto> {
    return apiFetch<UnitDto>(
        "/api/Units",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateUnit(
    unitId: number,
    data: UpdateUnitDto
): Promise<UnitDto> {
    return apiFetch<UnitDto>(
        `/api/Units/${unitId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteUnit(
    unitId: number
): Promise<DeleteUnitResponseDto> {
    return apiFetch<DeleteUnitResponseDto>(
        `/api/Units/${unitId}`,
        {
            method: "DELETE",
        }
    );
}