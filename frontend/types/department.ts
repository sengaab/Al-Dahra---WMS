export interface CreateDepartmentDto {
    name: string;
    code?: string | null;
    isActive: boolean;
}

export interface UpdateDepartmentDto {
    name: string;
    code?: string | null;
    isActive: boolean;
}

export interface DepartmentResponseDto {
    departmentId: number;
    name: string;
    code: string | null;
    isActive: boolean;

    createdAt: string;
    updatedAt: string;

    usersCount: number;
    requestsCount: number;
}

export interface DepartmentUserDto {
    userId: string;
    employeeCode: string | null;

    name: string;
    email: string;

    roleId: number;
    roleName: string | null;

    isActive: boolean;
}

export interface DepartmentRequestDto {
    requestId: number;
    requestNumber: string | null;

    requestedBy: string;
    requesterName: string | null;

    departmentId: number;
    status: string | null;

    createdAt: string;
}