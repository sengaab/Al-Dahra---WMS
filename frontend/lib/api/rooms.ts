import { apiFetch } from "./api";
import {
    Room,
    CreateRoomRequest,
    UpdateRoomRequest,
} from "./types";

export async function getRooms(): Promise<Room[]> {
    return apiFetch<Room[]>("/api/Rooms/Getall");
}

export async function getRoom(
    id: number
): Promise<Room> {
    return apiFetch<Room>(
        `/api/Rooms/Getbyid${id}`
    );
}

export async function getRoomsByWarehouse(
    warehouseId: number
): Promise<Room[]> {
    return apiFetch<Room[]>(
        `/api/Rooms/GETROOMSBYWAREHOUSE/${warehouseId}`
    );
}

export async function createRoom(
    data: CreateRoomRequest
): Promise<Room> {
    return apiFetch<Room>(
        "/api/Rooms/Create",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateRoom(
    id: number,
    data: UpdateRoomRequest
): Promise<Room> {
    return apiFetch<Room>(
        `/api/Rooms/Update/${id}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteRoom(
    id: number
): Promise<void> {
    await apiFetch<void>(
        `/api/Rooms/${id}`,
        {
            method: "DELETE",
        }
    );
}