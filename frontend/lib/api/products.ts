import { apiFetch } from "./api";
import {
    Product,
    CreateProductRequest,
    UpdateProductRequest,
    UpdateProductStatusRequest,
} from "./types";

export async function getProducts(): Promise<Product[]> {
    return apiFetch<Product[]>("/api/Products/Getall");
}

export async function getProduct(
    id: number
): Promise<Product> {
    return apiFetch<Product>(
        `/api/Products/GetProductBy/${id}`
    );
}

export async function getProductBySKU(
    sku: string
): Promise<Product> {
    return apiFetch<Product>(
        `/api/Products/GetbySKU/${encodeURIComponent(sku)}`
    );
}

export async function getProductByBarcode(
    barcode: string
): Promise<Product> {
    return apiFetch<Product>(
        `/api/Products/GetbyBarcode/${encodeURIComponent(barcode)}`
    );
}

export async function getProductByQR(
    qrValue: string
): Promise<Product> {
    return apiFetch<Product>(
        `/api/Products/GetbyQR/${encodeURIComponent(qrValue)}`
    );
}

export async function searchProducts(
    query: string
): Promise<Product[]> {
    return apiFetch<Product[]>(
        `/api/Products/Searchproducts?search=${encodeURIComponent(query)}`
    );
}

export async function createProduct(
    data: CreateProductRequest
): Promise<Product> {
    return apiFetch<Product>(
        "/api/Products/create",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateProduct(
    id: number,
    data: UpdateProductRequest
): Promise<Product> {
    return apiFetch<Product>(
        `/api/Products/Update${id}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteProduct(
    id: number
): Promise<void> {
    await apiFetch<void>(
        `/api/Products/${id}`,
        {
            method: "DELETE",
        }
    );
}

export async function updateProductStatus(
    id: number,
    data: UpdateProductStatusRequest
): Promise<Product> {
    return apiFetch<Product>(
        `/api/Products/UpdateStatus/${id}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}