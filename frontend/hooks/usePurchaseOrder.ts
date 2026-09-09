"use client";

import { useCallback, useEffect, useState } from "react";

import {
    getPurchaseOrderById,
    getPurchaseOrderItems,
} from "@/lib/api/purchase-orders";

import type {
    PurchaseOrderDto,
    PurchaseOrderItemDto,
} from "@/types/purchase-order";

export function usePurchaseOrder(poId: number) {
    const [purchaseOrder, setPurchaseOrder] =
        useState<PurchaseOrderDto | null>(null);

    const [items, setItems] =
        useState<PurchaseOrderItemDto[]>([]);

    const [loading, setLoading] =
        useState(true);

    const [error, setError] =
        useState("");

    const refreshPurchaseOrder =
        useCallback(async () => {
            if (!poId) {
                return;
            }

            try {
                setLoading(true);
                setError("");

                const [
                    order,
                    orderItems,
                ] = await Promise.all([
                    getPurchaseOrderById(poId),
                    getPurchaseOrderItems(poId),
                ]);

                setPurchaseOrder(order);
                setItems(orderItems);
            } catch (err: any) {
                console.error(
                    "Load PO error:",
                    err
                );

                setError(
                    err?.message ||
                        "Failed to load purchase order."
                );
            } finally {
                setLoading(false);
            }
        }, [poId]);

    useEffect(() => {
        let mounted = true;

        const loadPurchaseOrder =
            async () => {
                if (!poId) {
                    return;
                }

                try {
                    setLoading(true);
                    setError("");

                    const [
                        order,
                        orderItems,
                    ] = await Promise.all([
                        getPurchaseOrderById(
                            poId
                        ),

                        getPurchaseOrderItems(
                            poId
                        ),
                    ]);

                    if (!mounted) {
                        return;
                    }

                    setPurchaseOrder(order);
                    setItems(orderItems);
                } catch (err: any) {
                    if (!mounted) {
                        return;
                    }

                    console.error(
                        "Load PO error:",
                        err
                    );

                    setError(
                        err?.message ||
                            "Failed to load purchase order."
                    );
                } finally {
                    if (mounted) {
                        setLoading(false);
                    }
                }
            };

        loadPurchaseOrder();

        return () => {
            mounted = false;
        };
    }, [poId]);

    return {
        purchaseOrder,
        items,
        loading,
        error,
        refreshPurchaseOrder,
    };
}