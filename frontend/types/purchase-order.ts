export interface PurchaseOrderDto {
    purchaseOrderId: number;
    poNumber: string;

    supplierId: number;
    supplierName: string;

    siteId: number;
    siteName: string;

    orderDate: string;
    expectedDate: string | null;

    status: string;

    totalValue: number;

    createdBy: string;
    creatorName: string | null;

    approvedBy: string | null;
    approverName: string | null;

    approvedAt: string | null;

    createdAt: string;
    updatedAt: string;

    itemsCount: number;
    totalOrderedQuantity: number;
    totalReceivedQuantity: number;
    totalRemainingQuantity: number;
}

export interface CreatePurchaseOrderDto {
    poNumber: string;
    supplierId: number;
    siteId: number;
    orderDate?: string | null;
    expectedDate?: string | null;
}

export interface UpdatePurchaseOrderDto {
    poNumber?: string | null;
    supplierId?: number | null;
    siteId?: number | null;
    orderDate?: string | null;
    expectedDate?: string | null;
}

export interface CreatePurchaseOrderItemDto {
    productId: number;
    orderedQuantity: number;
    unitPrice: number;
}

export interface UpdatePurchaseOrderItemDto {
    productId?: number | null;
    orderedQuantity?: number | null;
    unitPrice?: number | null;
}

export interface PurchaseOrderItemDto {
    purchaseOrderItemId: number;
    purchaseOrderId: number;

    productId: number;
    productName: string;
    sku: string;

    orderedQuantity: number;
    receivedQuantity: number;
    remainingQuantity: number;

    unitPrice: number;
    totalPrice: number;
}

export interface PurchaseOrderReceiptDto {
    receiptId: number;
    receiptNumber: string;

    purchaseOrderId: number;

    warehouseId: number;
    warehouseName: string;

    receivedBy: string;
    receiverName: string | null;

    receivedAt: string;

    notes: string | null;

    status: string;

    itemsCount: number;
}

export interface PurchaseOrderHistoryDto {
    eventType: string;
    description: string;
    status: string | null;
    date: string;

    userId: string | null;
    userName: string | null;

    receiptId: number | null;
    receiptItemId: number | null;
    inspectionId: number | null;
}

export interface PurchaseOrderActionResponseDto {
    message: string;
    status: string;
}

export interface DeletePurchaseOrderResponseDto {
    message: string;
}

export interface DeletePurchaseOrderItemResponseDto {
    message: string;
}