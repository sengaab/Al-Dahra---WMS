import { apiFetch } from "./api";
import {
    Unit,
    CreateUnitRequest,
    UpdateUnitRequest,
} from "./types";

export async function getUnits(): Promise<Unit[]> {
    return apiFetch<Unit[]>(
        "/api/Units/Getall"
    );
}

export async function getUnit(
    id: number
): Promise<Unit> {
    return apiFetch<Unit>(
        `/api/Units/GetbyId/${id}`
    );
}

export async function createUnit(
    data: CreateUnitRequest
): Promise<Unit> {
    return apiFetch<Unit>(
        "/api/Units/Create",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateUnit(
    id: number,
    data: UpdateUnitRequest
): Promise<Unit> {
    return apiFetch<Unit>(
        `/api/Units/Update/${id}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteUnit(
    id: number
): Promise<void> {
    await apiFetch<void>(
        `/api/Units/${id}`,
        {
            method: "DELETE",
        }
    );
}