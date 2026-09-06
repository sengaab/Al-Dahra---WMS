export interface CreateUserDto {
    employeeCode?: string | null;
    name: string;
    email: string;
    roleId: number;
    departmentId?: number | null;
    isActive: boolean;
}

export interface UpdateUserDto {
    employeeCode?: string | null;
    name: string;
    email: string;
    roleId: number;
    departmentId?: number | null;
    isActive: boolean;
}

export interface UserResponseDto {
    userId: string;

    employeeCode: string | null;

    name: string;
    email: string;

    roleId: number;
    roleName: string | null;

    departmentId: number | null;
    departmentName: string | null;

    isActive: boolean;

    createdAt: string;
    updatedAt: string;
}

export interface UserActivityDto {
    auditLogId: number;

    entityType: string;
    entityId: number;

    action: string;

    oldValue: string | null;
    newValue: string | null;

    createdAt: string;
}

export interface UserPermissionsDto {
    userId: string;

    roleId: number;
    roleName: string | null;

    permissions: string[];
}

export interface DeleteUserResponseDto {
    message: string;
    userId: string;
}