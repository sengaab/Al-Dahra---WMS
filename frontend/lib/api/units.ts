import { apiFetch } from "./api";

export function getUnits() {
    return apiFetch("/api/Units/Getall");
}

export function getUnitById(id: string | number) {
    return apiFetch(`/api/Units/GetbyId/${id}`);
}

export function createUnit(data: unknown) {
    return apiFetch("/api/Units/Create", {
        method: "POST",
        body: JSON.stringify(data),
    });
}

export function updateUnit(id: string | number, data: unknown) {
    return apiFetch(`/api/Units/Update/${id}`, {
        method: "PUT",
        body: JSON.stringify(data),
    });
}

export function deleteUnit(id: string | number) {
    return apiFetch(`/api/Units/${id}`, {
        method: "DELETE",
    });
}