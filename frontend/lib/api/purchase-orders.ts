import { apiFetch } from "@/lib/api";
import type {
    CreatePurchaseOrderDto,
    CreatePurchaseOrderItemDto,
    DeletePurchaseOrderItemResponseDto,
    DeletePurchaseOrderResponseDto,
    PurchaseOrderDto,
    PurchaseOrderHistoryDto,
    PurchaseOrderItemDto,
    PurchaseOrderReceiptDto,
    PurchaseOrderActionResponseDto,
    UpdatePurchaseOrderDto,
    UpdatePurchaseOrderItemDto,
} from "@/types/purchase-order";

// =====================================================
// GET ALL
// GET /api/purchase-orders
// =====================================================

export interface GetPurchaseOrdersParams {
    search?: string;
    status?: string;
    supplierId?: number;
    siteId?: number;
    page?: number;
    pageSize?: number;
}

export async function getPurchaseOrders(
    params?: GetPurchaseOrdersParams
): Promise<PurchaseOrderDto[]> {
    const query = new URLSearchParams();

    if (params?.search) {
        query.set("search", params.search);
    }

    if (params?.status) {
        query.set("status", params.status);
    }

    if (params?.supplierId !== undefined) {
        query.set(
            "supplierId",
            params.supplierId.toString()
        );
    }

    if (params?.siteId !== undefined) {
        query.set(
            "siteId",
            params.siteId.toString()
        );
    }

    if (params?.page !== undefined) {
        query.set("page", params.page.toString());
    }

    if (params?.pageSize !== undefined) {
        query.set(
            "pageSize",
            params.pageSize.toString()
        );
    }

    const queryString = query.toString();

    return apiFetch<PurchaseOrderDto[]>(
        `/api/purchase-orders${
            queryString ? `?${queryString}` : ""
        }`
    );
}

// =====================================================
// GET BY ID
// GET /api/purchase-orders/{id}
// =====================================================

export async function getPurchaseOrderById(
    purchaseOrderId: number
): Promise<PurchaseOrderDto> {
    return apiFetch<PurchaseOrderDto>(
        `/api/purchase-orders/${purchaseOrderId}`
    );
}

// =====================================================
// CREATE
// POST /api/purchase-orders
// =====================================================

export async function createPurchaseOrder(
    data: CreatePurchaseOrderDto
): Promise<PurchaseOrderDto> {
    return apiFetch<PurchaseOrderDto>(
        "/api/purchase-orders",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

// =====================================================
// UPDATE
// PUT /api/purchase-orders/{id}
// =====================================================

export async function updatePurchaseOrder(
    purchaseOrderId: number,
    data: UpdatePurchaseOrderDto
): Promise<PurchaseOrderDto> {
    return apiFetch<PurchaseOrderDto>(
        `/api/purchase-orders/${purchaseOrderId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

// =====================================================
// DELETE
// DELETE /api/purchase-orders/{id}
// =====================================================

export async function deletePurchaseOrder(
    purchaseOrderId: number
): Promise<DeletePurchaseOrderResponseDto> {
    return apiFetch<DeletePurchaseOrderResponseDto>(
        `/api/purchase-orders/${purchaseOrderId}`,
        {
            method: "DELETE",
        }
    );
}

// =====================================================
// SUBMIT
// POST /api/purchase-orders/{id}/submit
// =====================================================

export async function submitPurchaseOrder(
    purchaseOrderId: number
): Promise<PurchaseOrderActionResponseDto> {
    return apiFetch<PurchaseOrderActionResponseDto>(
        `/api/purchase-orders/${purchaseOrderId}/submit`,
        {
            method: "POST",
        }
    );
}

// =====================================================
// APPROVE
// POST /api/purchase-orders/{id}/approve
// =====================================================

export async function approvePurchaseOrder(
    purchaseOrderId: number
): Promise<PurchaseOrderActionResponseDto> {
    return apiFetch<PurchaseOrderActionResponseDto>(
        `/api/purchase-orders/${purchaseOrderId}/approve`,
        {
            method: "POST",
        }
    );
}

// =====================================================
// REJECT
// POST /api/purchase-orders/{id}/reject
// =====================================================

export async function rejectPurchaseOrder(
    purchaseOrderId: number
): Promise<PurchaseOrderActionResponseDto> {
    return apiFetch<PurchaseOrderActionResponseDto>(
        `/api/purchase-orders/${purchaseOrderId}/reject`,
        {
            method: "POST",
        }
    );
}

// =====================================================
// CANCEL
// POST /api/purchase-orders/{id}/cancel
// =====================================================

export async function cancelPurchaseOrder(
    purchaseOrderId: number
): Promise<PurchaseOrderActionResponseDto> {
    return apiFetch<PurchaseOrderActionResponseDto>(
        `/api/purchase-orders/${purchaseOrderId}/cancel`,
        {
            method: "POST",
        }
    );
}

// =====================================================
// GET ITEMS
// GET /api/purchase-orders/{id}/items
// =====================================================

export async function getPurchaseOrderItems(
    purchaseOrderId: number
): Promise<PurchaseOrderItemDto[]> {
    return apiFetch<PurchaseOrderItemDto[]>(
        `/api/purchase-orders/${purchaseOrderId}/items`
    );
}

// =====================================================
// ADD ITEM
// POST /api/purchase-orders/{id}/items
// =====================================================

export async function addPurchaseOrderItem(
    purchaseOrderId: number,
    data: CreatePurchaseOrderItemDto
): Promise<PurchaseOrderItemDto> {
    return apiFetch<PurchaseOrderItemDto>(
        `/api/purchase-orders/${purchaseOrderId}/items`,
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

// =====================================================
// UPDATE ITEM
// PUT /api/purchase-orders/{id}/items/{itemId}
// =====================================================

export async function updatePurchaseOrderItem(
    purchaseOrderId: number,
    itemId: number,
    data: UpdatePurchaseOrderItemDto
): Promise<PurchaseOrderItemDto> {
    return apiFetch<PurchaseOrderItemDto>(
        `/api/purchase-orders/${purchaseOrderId}/items/${itemId}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

// =====================================================
// DELETE ITEM
// DELETE /api/purchase-orders/{id}/items/{itemId}
// =====================================================

export async function deletePurchaseOrderItem(
    purchaseOrderId: number,
    itemId: number
): Promise<DeletePurchaseOrderItemResponseDto> {
    return apiFetch<DeletePurchaseOrderItemResponseDto>(
        `/api/purchase-orders/${purchaseOrderId}/items/${itemId}`,
        {
            method: "DELETE",
        }
    );
}

// =====================================================
// GET RECEIPTS
// GET /api/purchase-orders/{id}/receipts
// =====================================================

export async function getPurchaseOrderReceipts(
    purchaseOrderId: number
): Promise<PurchaseOrderReceiptDto[]> {
    return apiFetch<PurchaseOrderReceiptDto[]>(
        `/api/purchase-orders/${purchaseOrderId}/receipts`
    );
}

// =====================================================
// GET HISTORY
// GET /api/purchase-orders/{id}/history
// =====================================================

export async function getPurchaseOrderHistory(
    purchaseOrderId: number
): Promise<PurchaseOrderHistoryDto[]> {
    return apiFetch<PurchaseOrderHistoryDto[]>(
        `/api/purchase-orders/${purchaseOrderId}/history`
    );
}

export async function orderPurchaseOrder(
    purchaseOrderId: number
) {
    return apiFetch<{
        message: string;
        status: string;
    }>(
        `/api/purchase-orders/${purchaseOrderId}/order`,
        {
            method: "POST",
        }
    );
}