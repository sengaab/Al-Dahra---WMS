export interface CreatePickListDto {
    requestId: number;
    warehouseId: number;
    assignedTo?: string | null;
}

export interface UpdatePickListDto {
    assignedTo?: string | null;
    pickListStatus: string;
}

export interface AssignPickListDto {
    assignedTo: string;
}

export interface PickListResponseDto {
    pickListId: number;
    pickNumber: string;
    requestId: number;
    requestNumber: string | null;
    warehouseId: number;
    warehouseName: string | null;
    assignedTo: string | null;
    assigneeName: string | null;
    pickListStatus: string;
    createdAt: string;
    startedAt: string | null;
    completedAt: string | null;
    items: PickItemResponseDto[];
}

export interface PickItemResponseDto {
    pickItemId: number;
    productId: number;
    productName: string | null;
    stockId: number;
    locationId: number;
    requestedQuantity: number;
    pickedQuantity: number;
    pickItemStatus: string;
}

export interface PickListActionResponseDto {
    message: string;
    pickListId: number;
    status: string;
}

export interface AssignPickListResponseDto {
    message: string;
    pickListId: number;
    assignedTo: string | null;
}

export interface PickItemActionResponseDto {
    message: string;
    pickListId: number;
    itemId: number;
    pickedQuantity: number;
    status: string;
}