import { apiFetch } from "@/lib/api";
import type { SupplierDto } from "@/types";

export async function getSuppliers(): Promise<SupplierDto[]> {
    return apiFetch("/api/Suppliers", {
        method: "GET",
    });
}