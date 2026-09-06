export interface RoleResponseDto {
    roleId: number;
    name: string;
    isActive: boolean;
    createdAt: string;
    updatedAt: string;
    usersCount: number;
}

export interface CreateRoleDto {
    name: string;
    isActive: boolean;
}

export interface UpdateRoleDto {
    name: string;
    isActive: boolean;
}

export interface RolePermissionsDto {
    roleId: number;
    roleName: string;
    permissions: string[];
}

export interface UpdateRolePermissionsDto {
    permissions: string[];
}

export interface DeleteRoleResponseDto {
    message: string;
    roleId: number;
}

export interface UpdateRolePermissionsResponseDto {
    message: string;
    roleId: number;
    permissions: string[];
}