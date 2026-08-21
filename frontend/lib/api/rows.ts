import { apiFetch } from "./api";
import {
    Row,
    CreateRowRequest,
    UpdateRowRequest,
} from "./types";

export async function getRows(): Promise<Row[]> {
    return apiFetch<Row[]>("/api/Rows/Getall");
}

export async function getRow(
    id: number
): Promise<Row> {
    return apiFetch<Row>(
        `/api/Rows/Getbyid${id}`
    );
}

export async function getRowsByRoom(
    roomId: number
): Promise<Row[]> {
    return apiFetch<Row[]>(
        `/api/Rows/GetRowbyroomid/${roomId}`
    );
}

export async function createRow(
    data: CreateRowRequest
): Promise<Row> {
    return apiFetch<Row>(
        "/api/Rows/create",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateRow(
    id: number,
    data: UpdateRowRequest
): Promise<Row> {
    return apiFetch<Row>(
        `/api/Rows/Update/${id}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteRow(
    id: number
): Promise<void> {
    await apiFetch<void>(
        `/api/Rows/${id}`,
        {
            method: "DELETE",
        }
    );
}