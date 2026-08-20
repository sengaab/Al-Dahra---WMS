import { apiFetch } from "./api";

export function getCategories() {
    return apiFetch("/api/Categories/Getall");
}

export function getCategoryById(id: string | number) {
    return apiFetch(`/api/Categories/GetbyId${id}`);
}

export function getCategoriesByDepartment(departmentId: string | number) {
    return apiFetch(`/api/Categories/Department/${departmentId}`);
}

export function createCategory(data: unknown) {
    return apiFetch("/api/Categories/create", {
        method: "POST",
        body: JSON.stringify(data),
    });
}

export function updateCategory(id: string | number, data: unknown) {
    return apiFetch(`/api/Categories/Update/${id}`, {
        method: "PUT",
        body: JSON.stringify(data),
    });
}

export function deleteCategory(id: string | number) {
    return apiFetch(`/api/Categories/${id}`, {
        method: "DELETE",
    });
}