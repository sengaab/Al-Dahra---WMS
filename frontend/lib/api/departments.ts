import { apiFetch } from "./api";
import {
    Department,
    CreateDepartmentRequest,
    UpdateDepartmentRequest,
} from "./types";

export async function getDepartments(): Promise<Department[]> {
    return apiFetch<Department[]>(
        "/api/Department"
    );
}

export async function getDepartment(
    id: number
): Promise<Department> {
    return apiFetch<Department>(
        `/api/Department/${id}`
    );
}

export async function getDepartmentByName(
    name: string
): Promise<Department> {
    return apiFetch<Department>(
        `/api/Department/GetbyName/${encodeURIComponent(name)}`
    );
}

export async function createDepartment(
    data: CreateDepartmentRequest
): Promise<Department> {
    return apiFetch<Department>(
        "/api/Department",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateDepartment(
    id: number,
    data: UpdateDepartmentRequest
): Promise<Department> {
    return apiFetch<Department>(
        `/api/Department/${id}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteDepartment(
    id: number
): Promise<void> {
    await apiFetch<void>(
        `/api/Department/${id}`,
        {
            method: "DELETE",
        }
    );
}