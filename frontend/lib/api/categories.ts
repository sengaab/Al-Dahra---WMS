import { apiFetch } from "./api";
import {
    Category,
    CreateCategoryRequest,
    UpdateCategoryRequest,
} from "./types";

export async function getCategories(): Promise<Category[]> {
    return apiFetch<Category[]>(
        "/api/Categories/Getall"
    );
}

export async function getCategory(
    id: number
): Promise<Category> {
    return apiFetch<Category>(
        `/api/Categories/GetbyId${id}`
    );
}

export async function getCategoriesByDepartment(
    departmentId: number
): Promise<Category[]> {
    return apiFetch<Category[]>(
        `/api/Categories/Department/${departmentId}`
    );
}

export async function createCategory(
    data: CreateCategoryRequest
): Promise<Category> {
    return apiFetch<Category>(
        "/api/Categories/create",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateCategory(
    id: number,
    data: UpdateCategoryRequest
): Promise<Category> {
    return apiFetch<Category>(
        `/api/Categories/Update/${id}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteCategory(
    id: number
): Promise<void> {
    await apiFetch<void>(
        `/api/Categories/${id}`,
        {
            method: "DELETE",
        }
    );
}