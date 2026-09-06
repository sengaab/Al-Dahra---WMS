import { apiFetch } from "@/lib/api";
import type {
    CreateDepartmentDto,
    DepartmentRequestDto,
    DepartmentResponseDto,
    DepartmentUserDto,
    UpdateDepartmentDto,
} from "@/types/department";

/**
 * GET /api/departments
 *
 * Get all departments.
 */
export async function getDepartments(): Promise<
    DepartmentResponseDto[]
> {
    return apiFetch<DepartmentResponseDto[]>(
        "/api/departments"
    );
}

/**
 * GET /api/departments/{id}
 *
 * Get a department by ID.
 */
export async function getDepartmentById(
    departmentId: number
): Promise<DepartmentResponseDto> {
    return apiFetch<DepartmentResponseDto>(
        `/api/departments/${departmentId}`
    );
}

/**
 * POST /api/departments
 *
 * Create a department.
 */
export async function createDepartment(
    data: CreateDepartmentDto
): Promise<DepartmentResponseDto> {
    return apiFetch<DepartmentResponseDto>(
        "/api/departments",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

/**
 * PUT /api/departments/{id}
 *
 * Update a department.
 */
export async function updateDepartment(
    departmentId: number,
    data: UpdateDepartmentDto
): Promise<DepartmentResponseDto> {
    return apiFetch<DepartmentResponseDto>(
        `/api/departments/${departmentId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

/**
 * DELETE /api/departments/{id}
 */
export async function deleteDepartment(
    departmentId: number
): Promise<{
    message: string;
    departmentId: number;
}> {
    return apiFetch<{
        message: string;
        departmentId: number;
    }>(`/api/departments/${departmentId}`, {
        method: "DELETE",
    });
}

/**
 * GET /api/departments/{id}/users
 *
 * Get all users belonging to a department.
 */
export async function getDepartmentUsers(
    departmentId: number
): Promise<DepartmentUserDto[]> {
    return apiFetch<DepartmentUserDto[]>(
        `/api/departments/${departmentId}/users`
    );
}

/**
 * GET /api/departments/{id}/requests
 *
 * Get all requests belonging to a department.
 */
export async function getDepartmentRequests(
    departmentId: number
): Promise<DepartmentRequestDto[]> {
    return apiFetch<DepartmentRequestDto[]>(
        `/api/departments/${departmentId}/requests`
    );
}