import { apiFetch } from "@/lib/api";
import type {
    CreateProductDto,
    DeleteProductResponseDto,
    ProductDto,
    ProductSupplierDto,
    UpdateProductDto,
} from "@/types/product";

// =====================================================
// GET ALL PRODUCTS
// GET /api/products
// =====================================================

export async function getProducts(): Promise<ProductDto[]> {
    return apiFetch<ProductDto[]>("/api/Products");
}

// =====================================================
// GET PRODUCT BY ID
// GET /api/products/{id}
// =====================================================

export async function getProductById(
    productId: number
): Promise<ProductDto> {
    return apiFetch<ProductDto>(
        `/api/Products/${productId}`
    );
}

// =====================================================
// SEARCH PRODUCTS
// GET /api/products/search?q=
// =====================================================

export async function searchProducts(
    query: string
): Promise<ProductDto[]> {
    const params = new URLSearchParams();

    params.set("q", query);

    return apiFetch<ProductDto[]>(
        `/api/Products/search?${params.toString()}`
    );
}

// =====================================================
// GET PRODUCT BY BARCODE
// GET /api/products/barcode/{barcode}
// =====================================================

export async function getProductByBarcode(
    barcode: string
): Promise<ProductDto> {
    return apiFetch<ProductDto>(
        `/api/Products/barcode/${encodeURIComponent(barcode)}`
    );
}

// =====================================================
// GET PRODUCT BY SKU
// GET /api/products/sku/{sku}
// =====================================================

export async function getProductBySku(
    sku: string
): Promise<ProductDto> {
    return apiFetch<ProductDto>(
        `/api/Products/sku/${encodeURIComponent(sku)}`
    );
}

// =====================================================
// CREATE PRODUCT
// POST /api/products
// =====================================================

export async function createProduct(
    data: CreateProductDto
): Promise<ProductDto> {
    return apiFetch<ProductDto>(
        "/api/Products",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

// =====================================================
// UPDATE PRODUCT
// PUT /api/products/{id}
// =====================================================

export async function updateProduct(
    productId: number,
    data: UpdateProductDto
): Promise<ProductDto> {
    return apiFetch<ProductDto>(
        `/api/Products/${productId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

// =====================================================
// DELETE PRODUCT
// DELETE /api/products/{id}
// =====================================================

export async function deleteProduct(
    productId: number
): Promise<DeleteProductResponseDto> {
    return apiFetch<DeleteProductResponseDto>(
        `/api/Products/${productId}`,
        {
            method: "DELETE",
        }
    );
}

// =====================================================
// PRODUCT INVENTORY
// GET /api/products/{id}/inventory
// =====================================================

export async function getProductInventory(
    productId: number
) {
    return apiFetch(
        `/api/Products/${productId}/inventory`
    );
}

// =====================================================
// PRODUCT STOCK
// GET /api/products/{id}/stock
// =====================================================

export async function getProductStock(
    productId: number
) {
    return apiFetch(
        `/api/Products/${productId}/stock`
    );
}

// =====================================================
// PRODUCT LOCATIONS
// GET /api/products/{id}/locations
// =====================================================

export async function getProductLocations(
    productId: number
) {
    return apiFetch(
        `/api/Products/${productId}/locations`
    );
}

// =====================================================
// PRODUCT TRANSACTIONS
// GET /api/products/{id}/transactions
// =====================================================

export async function getProductTransactions(
    productId: number
) {
    return apiFetch(
        `/api/Products/${productId}/transactions`
    );
}

// =====================================================
// PRODUCT SUPPLIERS
// GET /api/products/{id}/suppliers
// =====================================================

export async function getProductSuppliers(
    productId: number
): Promise<ProductSupplierDto[]> {
    return apiFetch<ProductSupplierDto[]>(
        `/api/Products/${productId}/suppliers`
    );
}

// =====================================================
// PURCHASE HISTORY
// GET /api/products/{id}/purchase-history
// =====================================================

export async function getProductPurchaseHistory(
    productId: number
) {
    return apiFetch(
        `/api/Products/${productId}/purchase-history`
    );
}

// =====================================================
// STOCK SUMMARY
// GET /api/products/{id}/stock-summary
// =====================================================

export async function getProductStockSummary(
    productId: number
) {
    return apiFetch(
        `/api/Products/${productId}/stock-summary`
    );
}

// =====================================================
// STOCK BY WAREHOUSE
// GET /api/products/{id}/stock-by-warehouse
// =====================================================

export async function getProductStockByWarehouse(
    productId: number
) {
    return apiFetch(
        `/api/Products/${productId}/stock-by-warehouse`
    );
}

// =====================================================
// STOCK BY LOCATION
// GET /api/products/{id}/stock-by-location
// =====================================================

export async function getProductStockByLocation(
    productId: number
) {
    return apiFetch(
        `/api/Products/${productId}/stock-by-location`
    );
}