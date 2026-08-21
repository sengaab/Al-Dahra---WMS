import { apiFetch } from "./api";
import {
    Bin,
    CreateBinRequest,
    UpdateBinRequest,
} from "./types";

export async function getBins(): Promise<Bin[]> {
    return apiFetch<Bin[]>("/api/Bins/Getall");
}

export async function getBin(
    id: number
): Promise<Bin> {
    return apiFetch<Bin>(
        `/api/Bins/GetbyId/${id}`
    );
}

export async function getBinsByShelf(
    shelfId: number
): Promise<Bin[]> {
    return apiFetch<Bin[]>(
        `/api/Bins/GetBinbyshelfid/${shelfId}`
    );
}

export async function createBin(
    data: CreateBinRequest
): Promise<Bin> {
    return apiFetch<Bin>(
        "/api/Bins/Create",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateBin(
    id: number,
    data: UpdateBinRequest
): Promise<Bin> {
    return apiFetch<Bin>(
        `/api/Bins/Updatebyid${id}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteBin(
    id: number
): Promise<void> {
    await apiFetch<void>(
        `/api/Bins/${id}`,
        {
            method: "DELETE",
        }
    );
}