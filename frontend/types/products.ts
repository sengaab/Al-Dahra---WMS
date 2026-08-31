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