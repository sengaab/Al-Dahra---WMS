import { apiFetch } from "@/lib/api";
import type { CategoryDto } from "@/types";

export async function getCategories(): Promise<CategoryDto[]> {
    return apiFetch("/api/Categories", {
        method: "GET",
    });
}