"use client";

import { useState } from "react";

import {
    submitPurchaseOrder,
    approvePurchaseOrder,
    orderPurchaseOrder,
} from "@/lib/api/purchase-orders";

import type {
    PurchaseOrderDto,
    PurchaseOrderItemDto,
} from "@/types/purchase-order";

export function usePurchaseOrderActions(
    poId: number,
    purchaseOrder: PurchaseOrderDto | null,
    items: PurchaseOrderItemDto[],
    refreshPurchaseOrder: () => Promise<void>
) {
    const [actionLoading, setActionLoading] = useState(false);
    const [actionError, setActionError] = useState("");

    const handleSubmit = async () => {
        try {
            setActionLoading(true);
            setActionError("");

            await submitPurchaseOrder(poId);

            await refreshPurchaseOrder();
        } catch (err: any) {
            console.error("Submit PO error:", err);

            setActionError(
                err?.message ||
                    "Failed to submit purchase order."
            );
        } finally {
            setActionLoading(false);
        }
    };

    const handleApprove = async () => {
        try {
            setActionLoading(true);
            setActionError("");

            await approvePurchaseOrder(poId);

            await refreshPurchaseOrder();
        } catch (err: any) {
            console.error("Approve PO error:", err);

            setActionError(
                err?.message ||
                    "Failed to approve purchase order."
            );
        } finally {
            setActionLoading(false);
        }
    };

    const handleOrder = async () => {
        try {
            setActionLoading(true);
            setActionError("");

            await orderPurchaseOrder(poId);

            await refreshPurchaseOrder();
        } catch (err: any) {
            console.error("Order purchase order error:", err);

            setActionError(
                err?.message ||
                    "Failed to order purchase order."
            );
        } finally {
            setActionLoading(false);
        }
    };

    return {
        actionLoading,
        actionError,
        handleSubmit,
        handleApprove,
        handleOrder,
    };
}