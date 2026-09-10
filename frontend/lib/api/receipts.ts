import { apiFetch } from "@/lib/api";
import type {
    AddReceiptItemResponseDto,
    CreateReceiptDto,
    CreateReceiptItemDto,
    ReceiptActionResponseDto,
    ReceiptDto,
    ReceiptItemActionResponseDto,
    ReceiptItemDto,
    UpdateReceiptDto,
    UpdateReceiptItemDto,
    UpdateReceiptResponseDto,
} from "@/types/receipt";

export async function getReceipts(): Promise<ReceiptDto[]> {
    return apiFetch<ReceiptDto[]>("/api/Receipts");
}

export async function getReceiptById(
    receiptId: number
): Promise<ReceiptDto> {
    return apiFetch<ReceiptDto>(
        `/api/Receipts/${receiptId}`
    );
}

export async function createReceipt(
    data: CreateReceiptDto
): Promise<ReceiptDto> {
    return apiFetch<ReceiptDto>(
        "/api/Receipts",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateReceipt(
    receiptId: number,
    data: UpdateReceiptDto
): Promise<UpdateReceiptResponseDto> {
    return apiFetch<UpdateReceiptResponseDto>(
        `/api/Receipts/${receiptId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function getReceiptItems(
    receiptId: number
): Promise<ReceiptItemDto[]> {
    return apiFetch<ReceiptItemDto[]>(
        `/api/Receipts/${receiptId}/items`
    );
}

export async function addReceiptItem(
    receiptId: number,
    data: CreateReceiptItemDto
): Promise<AddReceiptItemResponseDto> {
    return apiFetch<AddReceiptItemResponseDto>(
        `/api/Receipts/${receiptId}/items`,
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateReceiptItem(
    receiptId: number,
    itemId: number,
    data: UpdateReceiptItemDto
): Promise<ReceiptItemActionResponseDto> {
    return apiFetch<ReceiptItemActionResponseDto>(
        `/api/Receipts/${receiptId}/items/${itemId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteReceiptItem(
    receiptId: number,
    itemId: number
): Promise<ReceiptItemActionResponseDto> {
    return apiFetch<ReceiptItemActionResponseDto>(
        `/api/Receipts/${receiptId}/items/${itemId}`,
        {
            method: "DELETE",
        }
    );
}

export async function startReceipt(
    receiptId: number
): Promise<ReceiptActionResponseDto> {
    return apiFetch<ReceiptActionResponseDto>(
        `/api/Receipts/${receiptId}/start`,
        {
            method: "POST",
        }
    );
}

export async function completeReceipt(
    receiptId: number
): Promise<ReceiptActionResponseDto> {
    return apiFetch<ReceiptActionResponseDto>(
        `/api/Receipts/${receiptId}/complete`,
        {
            method: "POST",
        }
    );
}

export async function cancelReceipt(
    receiptId: number
): Promise<ReceiptActionResponseDto> {
    return apiFetch<ReceiptActionResponseDto>(
        `/api/Receipts/${receiptId}/cancel`,
        {
            method: "POST",
        }
    );
}

export async function partialReceivedReceipt(
    receiptId: number
): Promise<ReceiptActionResponseDto> {
    return apiFetch<ReceiptActionResponseDto>(
        `/api/Receipts/${receiptId}/partially-received`,
        {
            method: "POST",
        }
    );
}