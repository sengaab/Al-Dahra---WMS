import { apiFetch } from "@/lib/api";
import type {
    CreateUserDto,
    DeleteUserResponseDto,
    UpdateUserDto,
    UserActivityDto,
    UserPermissionsDto,
    UserResponseDto,
} from "@/types/user";

export async function getUsers(): Promise<UserResponseDto[]> {
    return apiFetch<UserResponseDto[]>(
        "/api/users"
    );
}

export async function getUserById(
    userId: string
): Promise<UserResponseDto> {
    return apiFetch<UserResponseDto>(
        `/api/users/${userId}`
    );
}

export async function createUser(
    data: CreateUserDto
): Promise<UserResponseDto> {
    return apiFetch<UserResponseDto>(
        "/api/users",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateUser(
    userId: string,
    data: UpdateUserDto
): Promise<UserResponseDto> {
    return apiFetch<UserResponseDto>(
        `/api/users/${userId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteUser(
    userId: string
): Promise<DeleteUserResponseDto> {
    return apiFetch<DeleteUserResponseDto>(
        `/api/users/${userId}`,
        {
            method: "DELETE",
        }
    );
}

export async function getUserActivity(
    userId: string
): Promise<UserActivityDto[]> {
    return apiFetch<UserActivityDto[]>(
        `/api/users/${userId}/activity`
    );
}

export async function getUserPermissions(
    userId: string
): Promise<UserPermissionsDto> {
    return apiFetch<UserPermissionsDto>(
        `/api/users/${userId}/permissions`
    );
}