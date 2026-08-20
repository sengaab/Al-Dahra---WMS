import { apiFetch } from "./api";

export function getRooms() {
    return apiFetch("/api/Rooms/Getall");
}

export function getRoomById(id: string | number) {
    return apiFetch(`/api/Rooms/Getbyid${id}`);
}

export function getRoomsByWarehouseId(warehouseId: string | number) {
    return apiFetch(
        `/api/Rooms/GETROOMSBYWAREHOUSE/${warehouseId}`
    );
}

export function createRoom(data: unknown) {
    return apiFetch("/api/Rooms/Create", {
        method: "POST",
        body: JSON.stringify(data),
    });
}

export function updateRoom(id: string | number, data: unknown) {
    return apiFetch(`/api/Rooms/Update${id}`, {
        method: "PUT",
        body: JSON.stringify(data),
    });
}

export function deleteRoom(id: string | number) {
    return apiFetch(`/api/Rooms/${id}`, {
        method: "DELETE",
    });
}