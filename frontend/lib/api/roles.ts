import { apiFetch } from "@/lib/api";
import type {
    CreateRoleDto,
    DeleteRoleResponseDto,
    RolePermissionsDto,
    RoleResponseDto,
    UpdateRoleDto,
    UpdateRolePermissionsDto,
    UpdateRolePermissionsResponseDto,
} from "@/types/role";

export async function getRoles(): Promise<RoleResponseDto[]> {
    return apiFetch<RoleResponseDto[]>(
        "/api/roles"
    );
}

export async function getRoleById(
    roleId: number
): Promise<RoleResponseDto> {
    return apiFetch<RoleResponseDto>(
        `/api/roles/${roleId}`
    );
}

export async function createRole(
    data: CreateRoleDto
): Promise<RoleResponseDto> {
    return apiFetch<RoleResponseDto>(
        "/api/roles",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateRole(
    roleId: number,
    data: UpdateRoleDto
): Promise<RoleResponseDto> {
    return apiFetch<RoleResponseDto>(
        `/api/roles/${roleId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteRole(
    roleId: number
): Promise<DeleteRoleResponseDto> {
    return apiFetch<DeleteRoleResponseDto>(
        `/api/roles/${roleId}`,
        {
            method: "DELETE",
        }
    );
}

export async function getRolePermissions(
    roleId: number
): Promise<RolePermissionsDto> {
    return apiFetch<RolePermissionsDto>(
        `/api/roles/${roleId}/permissions`
    );
}

export async function updateRolePermissions(
    roleId: number,
    data: UpdateRolePermissionsDto
): Promise<UpdateRolePermissionsResponseDto> {
    return apiFetch<UpdateRolePermissionsResponseDto>(
        `/api/roles/${roleId}/permissions`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}