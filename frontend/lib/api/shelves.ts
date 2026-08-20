import { apiFetch } from "./api";

export function getShelves() {
    return apiFetch("/api/Shelves/Getall");
}

export function getShelfById(id: string | number) {
    return apiFetch(`/api/Shelves/Getbyid${id}`);
}

export function getShelvesByRowId(rowId: string | number) {
    return apiFetch(`/api/Shelves/Getshelvesbyrow/${rowId}`);
}

export function createShelf(data: unknown) {
    return apiFetch("/api/Shelves", {
        method: "POST",
        body: JSON.stringify(data),
    });
}

export function updateShelf(id: string | number, data: unknown) {
    return apiFetch(`/api/Shelves/Update${id}`, {
        method: "PUT",
        body: JSON.stringify(data),
    });
}

export function deleteShelf(id: string | number) {
    return apiFetch(`/api/Shelves/${id}`, {
        method: "DELETE",
    });
}