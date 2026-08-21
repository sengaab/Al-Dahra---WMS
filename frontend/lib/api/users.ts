import { apiFetch } from "./api";
import {
    User,
    UpdateUserRequest,
    ChangeUserRoleRequest,
} from "./types";

export async function getUsers(): Promise<User[]> {
    return apiFetch<User[]>(
        "/api/Users/Getall"
    );
}

export async function getUser(
    id: string
): Promise<User> {
    return apiFetch<User>(
        `/api/Users/GetbyId/${id}`
    );
}

export async function getUserByEmail(
    email: string
): Promise<User> {
    return apiFetch<User>(
        `/api/Users/Getbyemail/${encodeURIComponent(email)}`
    );
}

export async function updateUser(
    id: string,
    data: UpdateUserRequest
): Promise<User> {
    return apiFetch<User>(
        `/api/Users/UpdateUser/${id}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function changeUserRole(
    data: ChangeUserRoleRequest
): Promise<User> {
    return apiFetch<User>(
        "/api/Users/ChangeRole",
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteUser(
    id: string
): Promise<void> {
    await apiFetch<void>(
        `/api/Users/${id}`,
        {
            method: "DELETE",
        }
    );
}