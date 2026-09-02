import { apiFetch } from "@/lib/api";
import type { CreateProductDto } from "@/types";
import { ProductDto } from "@/types";

export async function createProduct(
    data: CreateProductDto
) {
    return apiFetch("/api/Products", {
        method: "POST",
        body: JSON.stringify(data),
    });
}

export async function getProductById(
    productId: number
): Promise<ProductDto> {
    return apiFetch<ProductDto>(
        `/api/products/${productId}`
    );
}