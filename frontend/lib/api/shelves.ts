import { apiFetch } from "./api";
import {
    Shelf,
    CreateShelfRequest,
    UpdateShelfRequest,
} from "./types";

export async function getShelves(): Promise<Shelf[]> {
    return apiFetch<Shelf[]>(
        "/api/Shelves/Getall"
    );
}

export async function getShelf(
    id: number
): Promise<Shelf> {
    return apiFetch<Shelf>(
        `/api/Shelves/Getbyid${id}`
    );
}

export async function getShelvesByRow(
    rowId: number
): Promise<Shelf[]> {
    return apiFetch<Shelf[]>(
        `/api/Shelves/Getshelvesbyrow/${rowId}`
    );
}

export async function createShelf(
    data: CreateShelfRequest
): Promise<Shelf> {
    return apiFetch<Shelf>(
        "/api/Shelves",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateShelf(
    id: number,
    data: UpdateShelfRequest
): Promise<Shelf> {
    return apiFetch<Shelf>(
        `/api/Shelves/Update/${id}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteShelf(
    id: number
): Promise<void> {
    await apiFetch<void>(
        `/api/Shelves/${id}`,
        {
            method: "DELETE",
        }
    );
}