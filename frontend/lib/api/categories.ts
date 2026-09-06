import { apiFetch } from "@/lib/api";
import type {
    CategoryDto,
    CreateCategoryDto,
    UpdateCategoryDto,
} from "@/types/category";

/**
 * Get all categories
 * GET /api/categories
 */
export async function getCategories(): Promise<CategoryDto[]> {
    return apiFetch<CategoryDto[]>("/api/Categories");
}

/**
 * Get a category by ID
 * GET /api/categories/{id}
 */
export async function getCategoryById(
    categoryId: number
): Promise<CategoryDto> {
    return apiFetch<CategoryDto>(
        `/api/Categories/${categoryId}`
    );
}

/**
 * Get all products belonging to a category
 * GET /api/categories/{id}/products
 */
export async function getProductsByCategoryId(
    categoryId: number
) {
    return apiFetch(
        `/api/Categories/${categoryId}/products`
    );
}

/**
 * Create a category
 * POST /api/categories
 */
export async function createCategory(
    data: CreateCategoryDto
): Promise<CategoryDto> {
    return apiFetch<CategoryDto>("/api/Categories", {
        method: "POST",
        body: JSON.stringify(data),
    });
}

/**
 * Update a category
 * PUT /api/categories/{id}
 */
export async function updateCategory(
    categoryId: number,
    data: UpdateCategoryDto
): Promise<CategoryDto> {
    return apiFetch<CategoryDto>(
        `/api/Categories/${categoryId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

/**
 * Delete a category
 * DELETE /api/categories/{id}
 */
export async function deleteCategory(
    categoryId: number
): Promise<{ message: string }> {
    return apiFetch<{ message: string }>(
        `/api/Categories/${categoryId}`,
        {
            method: "DELETE",
        }
    );
}