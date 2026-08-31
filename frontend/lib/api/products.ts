import { apiFetch } from "@/lib/api";
import type { CreateProductDto } from "@/types";

export async function createProduct(
    data: CreateProductDto
) {
    return apiFetch("/api/Products", {
        method: "POST",
        body: JSON.stringify(data),
    });
}