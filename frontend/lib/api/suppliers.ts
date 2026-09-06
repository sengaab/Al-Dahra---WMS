import { apiFetch } from "@/lib/api";
import type {
    CreateSupplierDto,
    CreateSupplierProductDto,
    DeleteSupplierProductResponseDto,
    DeleteSupplierResponseDto,
    SupplierDto,
    SupplierPerformanceDto,
    SupplierProductDto,
    UpdateSupplierDto,
    UpdateSupplierProductDto,
} from "@/types/supplier";

export async function getSuppliers(): Promise<SupplierDto[]> {
    return apiFetch<SupplierDto[]>(
        "/api/Suppliers"
    );
}

export async function getSupplierById(
    supplierId: number
): Promise<SupplierDto> {
    return apiFetch<SupplierDto>(
        `/api/Suppliers/${supplierId}`
    );
}

export async function createSupplier(
    data: CreateSupplierDto
): Promise<SupplierDto> {
    return apiFetch<SupplierDto>(
        "/api/Suppliers",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateSupplier(
    supplierId: number,
    data: UpdateSupplierDto
): Promise<SupplierDto> {
    return apiFetch<SupplierDto>(
        `/api/Suppliers/${supplierId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteSupplier(
    supplierId: number
): Promise<DeleteSupplierResponseDto> {
    return apiFetch<DeleteSupplierResponseDto>(
        `/api/Suppliers/${supplierId}`,
        {
            method: "DELETE",
        }
    );
}

export async function getSupplierProducts(
    supplierId: number
): Promise<SupplierProductDto[]> {
    return apiFetch<SupplierProductDto[]>(
        `/api/Suppliers/${supplierId}/products`
    );
}

export async function addSupplierProduct(
    supplierId: number,
    data: CreateSupplierProductDto
): Promise<SupplierProductDto> {
    return apiFetch<SupplierProductDto>(
        `/api/Suppliers/${supplierId}/products`,
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateSupplierProduct(
    supplierId: number,
    productId: number,
    data: UpdateSupplierProductDto
): Promise<SupplierProductDto> {
    return apiFetch<SupplierProductDto>(
        `/api/Suppliers/${supplierId}/products/${productId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteSupplierProduct(
    supplierId: number,
    productId: number
): Promise<DeleteSupplierProductResponseDto> {
    return apiFetch<DeleteSupplierProductResponseDto>(
        `/api/Suppliers/${supplierId}/products/${productId}`,
        {
            method: "DELETE",
        }
    );
}

export async function getSupplierPurchaseOrders(
    supplierId: number
) {
    return apiFetch(
        `/api/Suppliers/${supplierId}/purchase-orders`
    );
}

export async function getSupplierReceipts(
    supplierId: number
) {
    return apiFetch(
        `/api/Suppliers/${supplierId}/receipts`
    );
}

export async function getSupplierPerformance(
    supplierId: number
): Promise<SupplierPerformanceDto> {
    return apiFetch<SupplierPerformanceDto>(
        `/api/Suppliers/${supplierId}/performance`
    );
}