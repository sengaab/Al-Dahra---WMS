import { apiFetch } from "./api";

export function createProduct(data: unknown) {
    return apiFetch("/api/Products/create", {
        method: "POST",
        body: JSON.stringify(data),
    });
}

export function getProducts() {
    return apiFetch("/api/Products/Getall");
}

export function getProductById(id: string | number) {
    return apiFetch(`/api/Products/GetProductBy/${id}`);
}

export function getProductBySKU(sku: string) {
    return apiFetch(`/api/Products/GetbySKU/${encodeURIComponent(sku)}`);
}

export function getProductByBarcode(barcode: string) {
    return apiFetch(
        `/api/Products/GetbyBarcode/${encodeURIComponent(barcode)}`
    );
}

export function getProductByQR(qrValue: string) {
    return apiFetch(
        `/api/Products/GetbyQR/${encodeURIComponent(qrValue)}`
    );
}

export function searchProducts(params?: Record<string, string | number>) {
    const query = params
        ? `?${new URLSearchParams(
              Object.entries(params).map(([key, value]) => [
                  key,
                  String(value),
              ])
          ).toString()}`
        : "";

    return apiFetch(`/api/Products/Searchproducts${query}`);
}

export function updateProduct(id: string | number, data: unknown) {
    return apiFetch(`/api/Products/Update${id}`, {
        method: "PUT",
        body: JSON.stringify(data),
    });
}

export function deleteProduct(id: string | number) {
    return apiFetch(`/api/Products/${id}`, {
        method: "DELETE",
    });
}

export function getProductQRCodeImage(id: string | number) {
    return apiFetch(`/api/Products/${id}/GetQRCodeImage`);
}

export function getProductBarcodeImage(id: string | number) {
    return apiFetch(`/api/Products/${id}/barcodeImage`);
}

export function updateProductStatus(
    id: string | number,
    data: unknown
) {
    return apiFetch(`/api/Products/UpdateStatus/${id}`, {
        method: "PUT",
        body: JSON.stringify(data),
    });
}