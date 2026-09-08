"use client";

import { useEffect, useMemo, useState } from "react";
import { useRouter } from "next/navigation";

import NavigationCard from "@/components/navigation-card";
import StatsCard from "@/components/stats-card";
import Button from "@/components/button";

import { getPurchaseOrders } from "@/lib/api/purchase-orders";
import { getSuppliers } from "@/lib/api/suppliers";

import type { PurchaseOrderDto } from "@/types/purchase-order";
import type { SupplierDto } from "@/types/supplier";

export default function Procurement() {
    const router = useRouter();

    const [purchaseOrders, setPurchaseOrders] = useState<
        PurchaseOrderDto[]
    >([]);

    const [recentPurchaseOrders, setRecentPurchaseOrders] =
        useState<PurchaseOrderDto[]>([]);

    const [suppliers, setSuppliers] = useState<SupplierDto[]>(
        []
    );

    const [loading, setLoading] = useState(true);

    useEffect(() => {
        async function loadData() {
            try {
                setLoading(true);

                const [
                    allPurchaseOrders,
                    recentOrders,
                    supplierData,
                ] = await Promise.all([
                    // Used for dashboard statistics
                    getPurchaseOrders({
                        page: 1,
                        pageSize: 1000,
                    }),

                    // Only fetch the first 5 for the table
                    getPurchaseOrders({
                        page: 1,
                        pageSize: 5,
                    }),

                    getSuppliers(),
                ]);

                setPurchaseOrders(allPurchaseOrders);
                setRecentPurchaseOrders(recentOrders);
                setSuppliers(supplierData);
            } catch (error) {
                console.error(
                    "Failed to load procurement data:",
                    error
                );
            } finally {
                setLoading(false);
            }
        }

        loadData();
    }, []);

    const stats = useMemo(() => {
        const pendingApproval =
            purchaseOrders.filter(
                (po) =>
                    po.status === "PendingApproval"
            ).length;

        const activeOrders =
            purchaseOrders.filter(
                (po) =>
                    po.status === "Ordered" ||
                    po.status === "PartiallyReceived"
            ).length;

        const now = new Date();

        const monthlyPurchaseOrders =
            purchaseOrders.filter((po) => {
                if (!po.orderDate) {
                    return false;
                }

                const date = new Date(po.orderDate);

                return (
                    date.getFullYear() ===
                        now.getFullYear() &&
                    date.getMonth() ===
                        now.getMonth()
                );
            });

        const monthlyValue =
            monthlyPurchaseOrders.reduce(
                (total, po) =>
                    total + Number(po.totalValue || 0),
                0
            );

        const averageMonthlyPOValue =
            monthlyPurchaseOrders.length > 0
                ? monthlyValue /
                  monthlyPurchaseOrders.length
                : 0;

        return {
            totalPOs: purchaseOrders.length,
            pendingApproval,
            activeOrders,
            suppliers: suppliers.length,
            averageMonthlyPOValue,
        };
    }, [purchaseOrders, suppliers]);

    function formatCurrency(value: number) {
        return `E£ ${value.toLocaleString("en-EG", {
            minimumFractionDigits: 0,
            maximumFractionDigits: 2,
        })}`;
    }

    function formatDate(date: string | null) {
        if (!date) {
            return "—";
        }

        return new Date(date).toLocaleDateString(
            "en-GB",
            {
                day: "2-digit",
                month: "short",
                year: "numeric",
            }
        );
    }

    function getStatusLabel(status: string) {
        switch (status) {
            case "PendingApproval":
                return "Pending Approval";

            case "PartiallyReceived":
                return "Partially Received";

            case "PendingApproval":
                return "Pending Approval";

            default:
                return status;
        }
    }

    return (
        <div className="page-column">
            <div
                className="row-container"
                style={{
                    width: "100%",
                    flexWrap: "wrap",
                }}
            >
                <StatsCard
                    title="Total POs"
                    value={
                        loading
                            ? "..."
                            : stats.totalPOs.toString()
                    }
                    valueColor=""
                    minHeight
                />

                <StatsCard
                    title="Pending Approval"
                    value={
                        loading
                            ? "..."
                            : stats.pendingApproval.toString()
                    }
                    valueColor="var(--orange)"
                    minHeight
                />

                <StatsCard
                    title="Active Orders"
                    value={
                        loading
                            ? "..."
                            : stats.activeOrders.toString()
                    }
                    valueColor="var(--blue)"
                    minHeight
                />

                <StatsCard
                    title="Suppliers"
                    value={
                        loading
                            ? "..."
                            : stats.suppliers.toString()
                    }
                    valueColor=""
                    minHeight
                />

                <StatsCard
                    title="Avg PO Value (Month)"
                    value={
                        loading
                            ? "..."
                            : formatCurrency(
                                  stats.averageMonthlyPOValue
                              )
                    }
                    valueColor="var(--dark-green)"
                    minHeight
                />
            </div>

            <div
                className="row-container"
                style={{
                    width: "100%",
                    flexWrap: "wrap",
                }}
            >
                <NavigationCard
                    title="Purchase Orders"
                    subtitle="Create, Approve, and Track POs"
                    icon="/nav/purchase-orders.svg"
                    onClick={() =>
                        router.push(
                            "/purchase-orders"
                        )
                    }
                />

                <NavigationCard
                    title="Receiving"
                    subtitle="Process Incoming Shipments"
                    icon="/nav/receiving.svg"
                    onClick={() =>
                        router.push("/receiving")
                    }
                />

                <NavigationCard
                    title="Suppliers"
                    subtitle="Manage Supplier Directory"
                    icon="/nav/suppliers.svg"
                    onClick={() =>
                        router.push("/suppliers")
                    }
                />
            </div>

            <div
                className="card column-container"
                style={{
                    width: "100%",
                    paddingInline: "0",
                }}
            >
                <div
                    className="row-container"
                    style={{
                        width: "100%",
                        justifyContent:
                            "space-between",
                        paddingInline:
                            "var(--space-3)",
                    }}
                >
                    <p className="body-title">
                        Recent Purchase Orders
                    </p>

                    <Button
                        size="sm"
                        variant="secondary"
                        onClick={() =>
                            router.push(
                                "/purchase-orders"
                            )
                        }
                    >
                        View All
                    </Button>
                </div>

                <table
                    style={{
                        width: "100%",
                        borderCollapse: "separate",
                        borderSpacing: 0,
                    }}
                >
                    <thead>
                        <tr
                            style={{
                                height: "40px",
                                backgroundColor:
                                    "var(--beige)",
                                border:
                                    "var(--border-default)",
                            }}
                        >
                            <th
                                style={{
                                    width: "20%",
                                    padding: "0 24px",
                                    textAlign: "left",
                                    verticalAlign:
                                        "middle",
                                    borderTop:
                                        "var(--border-default)",
                                    borderBottom:
                                        "var(--border-default)",
                                    backgroundColor:
                                        "var(--beige)",
                                }}
                            >
                                PO-Number
                            </th>

                            <th
                                style={{
                                    width: "20%",
                                    padding: "0 24px",
                                    textAlign: "left",
                                    verticalAlign:
                                        "middle",
                                    borderTop:
                                        "var(--border-default)",
                                    borderBottom:
                                        "var(--border-default)",
                                    backgroundColor:
                                        "var(--beige)",
                                }}
                            >
                                Supplier
                            </th>

                            <th
                                style={{
                                    width: "25%",
                                    padding: "0 24px",
                                    textAlign: "left",
                                    verticalAlign:
                                        "middle",
                                    borderTop:
                                        "var(--border-default)",
                                    borderBottom:
                                        "var(--border-default)",
                                    backgroundColor:
                                        "var(--beige)",
                                }}
                            >
                                Expected Date
                            </th>

                            <th
                                style={{
                                    width: "15%",
                                    padding: "0 24px",
                                    textAlign: "left",
                                    verticalAlign:
                                        "middle",
                                    borderTop:
                                        "var(--border-default)",
                                    borderBottom:
                                        "var(--border-default)",
                                    backgroundColor:
                                        "var(--beige)",
                                }}
                            >
                                Value
                            </th>

                            <th
                                style={{
                                    width: "20%",
                                    padding: "0 24px",
                                    textAlign: "left",
                                    verticalAlign:
                                        "middle",
                                    borderTop:
                                        "var(--border-default)",
                                    borderBottom:
                                        "var(--border-default)",
                                    backgroundColor:
                                        "var(--beige)",
                                }}
                            >
                                Status
                            </th>
                        </tr>
                    </thead>

                    <tbody>
                        {loading ? (
                            <tr>
                                <td
                                    colSpan={5}
                                    style={{
                                        height: "100px",
                                        textAlign: "center",
                                    }}
                                >
                                    Loading...
                                </td>
                            </tr>
                        ) : recentPurchaseOrders.length ===
                          0 ? (
                            <tr>
                                <td
                                    colSpan={5}
                                    style={{
                                        height: "100px",
                                        textAlign: "center",
                                    }}
                                >
                                    No purchase orders found
                                </td>
                            </tr>
                        ) : (
                            recentPurchaseOrders.map(
                                (po) => (
                                    <tr
                                        key={
                                            po.purchaseOrderId
                                        }
                                        onClick={() =>
                                            router.push(
                                                `/purchase-orders/${po.purchaseOrderId}`
                                            )
                                        }
                                        style={{
                                            height: "44px",
                                            cursor: "pointer",
                                        }}
                                    >
                                        <td
                                            style={{
                                                textAlign:
                                                    "left",
                                                padding:
                                                    "0 24px",
                                                borderBottom:
                                                    "var(--border-default)",
                                            }}
                                        >
                                            {po.poNumber}
                                        </td>

                                        <td
                                            style={{
                                                textAlign:
                                                    "left",
                                                padding:
                                                    "0 24px",
                                                borderBottom:
                                                    "var(--border-default)",
                                            }}
                                        >
                                            {
                                                po.supplierName
                                            }
                                        </td>

                                        <td
                                            style={{
                                                textAlign:
                                                    "left",
                                                padding:
                                                    "0 24px",
                                                borderBottom:
                                                    "var(--border-default)",
                                            }}
                                        >
                                            {formatDate(
                                                po.expectedDate
                                            )}
                                        </td>

                                        <td
                                            style={{
                                                textAlign:
                                                    "left",
                                                padding:
                                                    "0 24px",
                                                borderBottom:
                                                    "var(--border-default)",
                                            }}
                                        >
                                            {formatCurrency(
                                                Number(
                                                    po.totalValue ||
                                                        0
                                                )
                                            )}
                                        </td>

                                        <td
                                            style={{
                                                textAlign:
                                                    "left",
                                                padding:
                                                    "0 24px",
                                                borderBottom:
                                                    "var(--border-default)",
                                            }}
                                        >
                                            {getStatusLabel(
                                                po.status
                                            )}
                                        </td>
                                    </tr>
                                )
                            )
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    );
}