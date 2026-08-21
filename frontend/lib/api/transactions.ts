import { apiFetch } from "./api";
import {
    Transaction,
    CreateTransactionRequest,
} from "./types";

export async function getTransactions(): Promise<Transaction[]> {
    return apiFetch<Transaction[]>(
        "/api/Transactions/Getall"
    );
}

export async function getTransaction(
    id: number
): Promise<Transaction> {
    return apiFetch<Transaction>(
        `/api/Transactions/Getbyid/${id}`
    );
}

export async function createTransaction(
    data: CreateTransactionRequest
): Promise<Transaction> {
    return apiFetch<Transaction>(
        "/api/Transactions/Create",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function getTransactionsByProduct(
    productId: number
): Promise<Transaction[]> {
    return apiFetch<Transaction[]>(
        `/api/Transactions/Product/${productId}GetTransactionById`
    );
}

export async function getTransactionsByBin(
    binId: number
): Promise<Transaction[]> {
    return apiFetch<Transaction[]>(
        `/api/Transactions/${binId}GetbyBin`
    );
}

export async function getTransactionsByUser(
    userId: string
): Promise<Transaction[]> {
    return apiFetch<Transaction[]>(
        `/api/Transactions/GetTransactionByUser/${userId}`
    );
}

export async function getTransactionsByType(
    type: string
): Promise<Transaction[]> {
    return apiFetch<Transaction[]>(
        `/api/Transactions/GetByType/${encodeURIComponent(type)}`
    );
}

export async function filterTransactions(
    filters: Record<
        string,
        string | number | undefined
    >
): Promise<Transaction[]> {
    const params = new URLSearchParams();

    Object.entries(filters).forEach(
        ([key, value]) => {
            if (
                value !== undefined &&
                value !== ""
            ) {
                params.append(key, String(value));
            }
        }
    );

    const query = params.toString();

    return apiFetch<Transaction[]>(
        `/api/Transactions/Filter${query ? `?${query}` : ""}`
    );
}