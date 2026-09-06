export interface CategoryDto {
    categoryId: number;

    name: string;
    description: string | null;

    isActive: boolean;

    createdAt: string;
    updatedAt: string;
}

export interface CreateCategoryDto {
    name: string;
    description?: string | null;
}

export interface UpdateCategoryDto {
    name: string;
    description?: string | null;
    isActive: boolean;
}