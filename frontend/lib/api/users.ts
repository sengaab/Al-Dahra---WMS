import { apiFetch } from "./api";

export function getUsers() {
    return apiFetch("/api/Users/Getall");
}

export function getUserById(id: string | number) {
    return apiFetch(`/api/Users/GetbyId/${id}`);
}

export function getUserByEmail(email: string) {
    return apiFetch(
        `/api/Users/Getbyemail/${encodeURIComponent(email)}`
    );
}

export function updateUser(id: string | number, data: unknown) {
    return apiFetch(`/api/Users/UpdateUser/${id}`, {
        method: "PUT",
        body: JSON.stringify(data),
    });
}

export function changeUserRole(data: unknown) {
    return apiFetch("/api/Users/ChangeRole", {
        method: "PUT",
        body: JSON.stringify(data),
    });
}

export function deleteUser(id: string | number) {
    return apiFetch(`/api/Users/${id}`, {
        method: "DELETE",
    });
}