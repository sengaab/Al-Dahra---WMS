import { apiFetch } from "./api";
import {
    Role,
    CreateRoleRequest,
    UpdateRoleRequest,
} from "./types";

export async function getRoles(): Promise<Role[]> {
    return apiFetch<Role[]>("/api/Roles/Getall");
}

export async function getRole(
    id: number
): Promise<Role> {
    return apiFetch<Role>(
        `/api/Roles/GetbyId/${id}`
    );
}

export async function createRole(
    data: CreateRoleRequest
): Promise<Role> {
    return apiFetch<Role>(
        "/api/Roles/Create",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateRole(
    id: number,
    data: UpdateRoleRequest
): Promise<Role> {
    return apiFetch<Role>(
        `/api/Roles/Update${id}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteRole(
    id: number
): Promise<void> {
    await apiFetch<void>(
        `/api/Roles/${id}`,
        {
            method: "DELETE",
        }
    );
}