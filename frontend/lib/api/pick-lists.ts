import { apiFetch } from "@/lib/api";
import type {
    AssignPickListDto,
    AssignPickListResponseDto,
    CreatePickListDto,
    PickItemActionResponseDto,
    PickListActionResponseDto,
    PickListResponseDto,
    UpdatePickListDto,
} from "@/types/pick-list";

// =====================================================
// GET ALL
// GET /api/pick-lists
// =====================================================

export async function getPickLists(): Promise<
    PickListResponseDto[]
> {
    return apiFetch<PickListResponseDto[]>(
        "/api/pick-lists"
    );
}

// =====================================================
// GET BY ID
// GET /api/pick-lists/{id}
// =====================================================

export async function getPickListById(
    pickListId: number
): Promise<PickListResponseDto> {
    return apiFetch<PickListResponseDto>(
        `/api/pick-lists/${pickListId}`
    );
}

// =====================================================
// CREATE
// POST /api/pick-lists
// =====================================================

export async function createPickList(
    data: CreatePickListDto
): Promise<PickListResponseDto> {
    return apiFetch<PickListResponseDto>(
        "/api/pick-lists",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

// =====================================================
// UPDATE
// PUT /api/pick-lists/{id}
// =====================================================

export async function updatePickList(
    pickListId: number,
    data: UpdatePickListDto
): Promise<PickListResponseDto> {
    return apiFetch<PickListResponseDto>(
        `/api/pick-lists/${pickListId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

// =====================================================
// ASSIGN
// POST /api/pick-lists/{id}/assign
// =====================================================

export async function assignPickList(
    pickListId: number,
    data: AssignPickListDto
): Promise<AssignPickListResponseDto> {
    return apiFetch<AssignPickListResponseDto>(
        `/api/pick-lists/${pickListId}/assign`,
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

// =====================================================
// START
// POST /api/pick-lists/{id}/start
// =====================================================

export async function startPickList(
    pickListId: number
): Promise<PickListActionResponseDto> {
    return apiFetch<PickListActionResponseDto>(
        `/api/pick-lists/${pickListId}/start`,
        {
            method: "POST",
        }
    );
}

// =====================================================
// COMPLETE
// POST /api/pick-lists/{id}/complete
// =====================================================

export async function completePickList(
    pickListId: number
): Promise<PickListActionResponseDto> {
    return apiFetch<PickListActionResponseDto>(
        `/api/pick-lists/${pickListId}/complete`,
        {
            method: "POST",
        }
    );
}

// =====================================================
// CANCEL
// POST /api/pick-lists/{id}/cancel
// =====================================================

export async function cancelPickList(
    pickListId: number
): Promise<PickListActionResponseDto> {
    return apiFetch<PickListActionResponseDto>(
        `/api/pick-lists/${pickListId}/cancel`,
        {
            method: "POST",
        }
    );
}

// =====================================================
// PICK ITEM
// POST /api/pick-lists/{id}/items/{itemId}/pick
// =====================================================

export async function pickItem(
    pickListId: number,
    itemId: number
): Promise<PickItemActionResponseDto> {
    return apiFetch<PickItemActionResponseDto>(
        `/api/pick-lists/${pickListId}/items/${itemId}/pick`,
        {
            method: "POST",
        }
    );
}