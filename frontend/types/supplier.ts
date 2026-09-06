export interface SupplierDto {
    supplierId: number;

    code: string;
    name: string;

    contactName: string | null;
    email: string | null;
    phone: string | null;
    address: string | null;

    isActive: boolean;
    supplierStatus: string;

    createdAt: string;
    updatedAt: string;

    productsCount: number;
}

export interface CreateSupplierDto {
    code: string;
    name: string;

    contactName?: string | null;
    email?: string | null;
    phone?: string | null;
    address?: string | null;
}

export interface UpdateSupplierDto {
    code?: string | null;
    name?: string | null;

    contactName?: string | null;
    email?: string | null;
    phone?: string | null;
    address?: string | null;

    supplierStatus?: string | null;
    isActive?: boolean | null;
}

export interface SupplierProductDto {
    supplierProductId: number;

    supplierId: number;
    productId: number;

    productName: string;
    SKU: string;

    supplierSKU: string | null;

    unitPrice: number;
    leadTimeDays: number | null;

    isPreferred: boolean;
}

export interface CreateSupplierProductDto {
    productId: number;

    supplierSKU?: string | null;

    unitPrice: number;
    leadTimeDays?: number | null;

    isPreferred: boolean;
}

export interface UpdateSupplierProductDto {
    supplierSKU?: string | null;

    unitPrice: number;
    leadTimeDays?: number | null;

    isPreferred: boolean;
}

export interface DeleteSupplierResponseDto {
    message: string;
}

export interface DeleteSupplierProductResponseDto {
    message: string;
}

export interface SupplierPerformanceDto {
    supplierId: number;
    message: string;
}