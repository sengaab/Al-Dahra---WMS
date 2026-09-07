export interface ProductDto {
    productId: number;
    sku: string;
    barcode: string | null;
    qrValue: string | null;
    name: string;
    categoryId: number | null;
    categoryName: string | null;
    unitId: number | null;
    unitName: string | null;
    unitPrice: number;
    minimumStock: number;
    description: string | null;
    productStatus: string;
    isActive: boolean;
    createdAt: string;
    updatedAt: string;
    suppliers: ProductSupplierDto[];
}

export interface ProductSupplierDto {
    supplierId: number;
    supplierName: string;
}

export interface CreateProductDto {
    sku?: string | null;
    barcode: string | null;
    qrValue: string | null;
    name: string;
    categoryId?: number | null;
    unitId?: number | null;
    unitPrice: number;
    minimumStock: number;
    description?: string | null;
}

export interface UpdateProductDto {
    barcode?: string | null;
    QRValue?: string | null;
    name?: string | null;
    categoryId?: number | null;
    unitId?: number | null;
    unitPrice?: number | null;
    minimumStock?: number | null;
    description?: string | null;
    productStatus?: string | null;
    isActive?: boolean | null;
}

export interface DeleteProductResponseDto {
    message: string;
}