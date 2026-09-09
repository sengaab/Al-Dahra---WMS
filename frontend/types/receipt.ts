export interface ReceiptDto {
    receiptId: number;
    receiptNumber: string;

    purchaseOrderId: number;
    warehouseId: number;

    receivedBy: string;
    receivedAt: string;

    notes: string | null;

    receiptStatus: string;

    items: ReceiptItemDto[];
}

export interface CreateReceiptDto {
    purchaseOrderId: number;
    warehouseId: number;

    receivedBy?: string;
    receivedAt?: string;

    notes?: string | null;
}

export interface UpdateReceiptDto {
    warehouseId: number;
    receivedBy: string;
    receivedAt: string;
    notes?: string | null;
}

export interface ReceiptItemDto {
    receiptItemId: number;
    receiptId: number;
    purchaseOrderItemId: number;
    productId: number;

    receivedQuantity: number;
    acceptedQuantity: number;
    quarantineQuantity: number;
    rejectedQuantity: number;

    batchNumber: string | null;
    expiryDate: string | null;
}

export interface CreateReceiptItemDto {
    purchaseOrderItemId: number;
    productId: number;

    receivedQuantity?: number;
    acceptedQuantity?: number;
    quarantineQuantity?: number;
    rejectedQuantity?: number;

    batchNumber?: string | null;
    expiryDate?: string | null;
}

export interface UpdateReceiptItemDto {
    receivedQuantity: number;
    acceptedQuantity: number;
    quarantineQuantity: number;
    rejectedQuantity: number;

    batchNumber?: string | null;
    expiryDate?: string | null;
}

export interface ReceiptActionResponseDto {
    message: string;
    receiptId: number;
    status: string;
}

export interface UpdateReceiptResponseDto {
    message: string;
}

export interface AddReceiptItemResponseDto {
    message: string;
    receiptItemId: number;
}

export interface ReceiptItemActionResponseDto {
    message: string;
}

export interface ReceiptListItem {
    receiptId: number;
    receiptNumber: string;

    poNumber: string;
    supplierName: string;
    expectedDate: string | null;

    itemsCount: number;
    expectedQty: number;
    receivedQty: number;

    receiptStatus: string;
}