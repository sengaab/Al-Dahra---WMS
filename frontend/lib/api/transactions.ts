import { apiFetch } from "./api";

export function createTransaction(data: unknown) {
    return apiFetch("/api/Transactions/Create", {
        method: "POST",
        body: JSON.stringify(data),
    });
}

export function getTransactions() {
    return apiFetch("/api/Transactions/Getall");
}

export function getTransactionById(id: string | number) {
    return apiFetch(`/api/Transactions/Getbyid/${id}`);
}

export function getTransactionsByProductId(
    productId: string | number
) {
    return apiFetch(
        `/api/Transactions/Product/${productId}GetTransactionById`
    );
}

export function getTransactionsByBinId(binId: string | number) {
    return apiFetch(`/api/Transactions/${binId}GetbyBin`);
}

export function getTransactionsByUserId(userId: string | number) {
    return apiFetch(
        `/api/Transactions/GetTransactionByUser/${userId}`
    );
}

export function getTransactionsByType(type: string) {
    return apiFetch(
        `/api/Transactions/GetByType/${encodeURIComponent(type)}`
    );
}

export function filterTransactions(
    params?: Record<string, string | number>
) {
    const query = params
        ? `?${new URLSearchParams(
              Object.entries(params).map(([key, value]) => [
                  key,
                  String(value),
              ])
          ).toString()}`
        : "";

    return apiFetch(`/api/Transactions/Filter${query}`);
}