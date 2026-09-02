export interface CreateProductDto {
    sku: string;
    barcode: string;
    qrValue: string;
    name: string;
    categoryId: number;
    unitId: number;
    unitPrice: number;
    minimumStock: number;
    description: string;
}

export interface ProductDto {
    sku: string;
    barcode: string;
    qrValue: string;
    name: string;
    categoryName: string;
    categoryId: number;
    unitId: number;
    unitName: string;
    unitPrice: number;
    minimumStock: number;
    description: string;
}