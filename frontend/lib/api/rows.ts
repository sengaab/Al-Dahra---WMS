import { apiFetch } from "./api";

export function getRows() {
    return apiFetch("/api/Rows/Getall");
}

export function getRowById(id: string | number) {
    return apiFetch(`/api/Rows/Getbyid${id}`);
}

export function getRowsByRoomId(roomId: string | number) {
    return apiFetch(`/api/Rows/GetRowbyroomid/${roomId}`);
}

export function createRow(data: unknown) {
    return apiFetch("/api/Rows/create", {
        method: "POST",
        body: JSON.stringify(data),
    });
}

export function updateRow(id: string | number, data: unknown) {
    return apiFetch(`/api/Rows/Update${id}`, {
        method: "PUT",
        body: JSON.stringify(data),
    });
}

export function deleteRow(id: string | number) {
    return apiFetch(`/api/Rows/${id}`, {
        method: "DELETE",
    });
}