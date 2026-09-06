export interface SiteDto {
    siteId: number;
    code: string;
    name: string;
    description: string | null;
    isActive: boolean;
    createdAt: string;
    updatedAt: string;
    warehouseCount: number;
}

export interface SiteCreateDto {
    code: string;
    name: string;
    description?: string | null;
    isActive: boolean;
}

export interface SiteUpdateDto {
    code: string;
    name: string;
    description?: string | null;
    isActive: boolean;
}

export interface SiteWarehouseDto {
    warehouseId: number;
    code: string;
    name: string;
    description: string | null;
    isActive: boolean;
}

export interface SiteInventoryDto {
    productId: number;
    SKU: string;
    productName: string;
    quantity: number;
    warehouseId: number;
    warehouseName: string;
    binId: number;
    binName: string;
}

export interface SiteStatsDto {
    siteId: number;
    siteCode: string;
    siteName: string;
    warehouseCount: number;
    productCount: number;
    binCount: number;
    totalQuantity: number;
}

export interface DeleteSiteResponseDto {
    message?: string;
}