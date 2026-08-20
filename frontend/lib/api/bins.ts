import { apiFetch } from "./api";

export function getBins() {
    return apiFetch("/api/Bins/Getall");
}

export function getBinById(id: string | number) {
    return apiFetch(`/api/Bins/GetbyId/${id}`);
}

export function getBinsByShelfId(shelfId: string | number) {
    return apiFetch(`/api/Bins/GetBinbyshelfid/${shelfId}`);
}

export function createBin(data: unknown) {
    return apiFetch("/api/Bins/Create", {
        method: "POST",
        body: JSON.stringify(data),
    });
}

export function updateBin(id: string | number, data: unknown) {
    return apiFetch(`/api/Bins/Updatebyid${id}`, {
        method: "PUT",
        body: JSON.stringify(data),
    });
}

export function deleteBin(id: string | number) {
    return apiFetch(`/api/Bins/${id}`, {
        method: "DELETE",
    });
}