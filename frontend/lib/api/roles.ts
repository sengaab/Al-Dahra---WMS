import { apiFetch } from "./api";

export function getRoles() {
    return apiFetch("/api/Roles/Getall");
}

export function getRoleById(id: string | number) {
    return apiFetch(`/api/Roles/GetbyId/${id}`);
}

export function createRole(data: unknown) {
    return apiFetch("/api/Roles/Create", {
        method: "POST",
        body: JSON.stringify(data),
    });
}

export function updateRole(id: string | number, data: unknown) {
    return apiFetch(`/api/Roles/Update${id}`, {
        method: "PUT",
        body: JSON.stringify(data),
    });
}

export function deleteRole(id: string | number) {
    return apiFetch(`/api/Roles/${id}`, {
        method: "DELETE",
    });
}