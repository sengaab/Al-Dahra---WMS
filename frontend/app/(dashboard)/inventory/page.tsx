"use client";

import { useMemo, useState } from "react";
import Card from "@/components/card";
import Dropdown from "@/components/dropdown";
import ProductDetails from "@/components/product-details";
import AddProduct from "@/components/popups/add-product";
import SearchBar from "@/components/searchbar";
import Button from "@/components/button";
import Status from "@/components/status";
import { useStock } from "@/hooks/useStock";

export default function Inventory() {
    const [selectedStatus, setSelectedStatus] = useState("All");
    const [selectedCategory, setSelectedCategory] = useState("All");
    const [search, setSearch] = useState("");
    const [selectedProductId, setSelectedProductId] =
        useState<number | null>(null);

    const [addProductOpen, setAddProductOpen] =
        useState(false);

    const {
        stock,
        loading,
        error,
    } = useStock();

    const statuses = [
        { label: "All Statuses", value: "All" },
        { label: "Available", value: "Available" },
        { label: "Out of Stock", value: "Out of Stock" },
        { label: "Low Stock", value: "Low Stock" },
        { label: "Quarantined", value: "Quarantined" },
        { label: "Expired", value: "Expired" },
        { label: "Damaged", value: "Damaged" },
    ];

    const categories = useMemo(() => {
        const uniqueCategories = Array.from(
            new Set(
                stock
                    .map((item) => item.categoryName)
                    .filter(Boolean)
            )
        );

        return [
            { label: "All Categories", value: "All" },
            ...uniqueCategories.map((category) => ({
                label: category,
                value: category,
            })),
        ];
    }, [stock]);

    const filteredData = useMemo(() => {
        const query = search.toLowerCase().trim();

        return stock.filter((item) => {
            const matchesCategory =
                selectedCategory === "All" ||
                item.categoryName === selectedCategory;

            const matchesSearch =
                !query ||
                item.sku
                    .toLowerCase()
                    .includes(query) ||
                item.productName
                    .toLowerCase()
                    .includes(query) ||
                item.stockCode
                    .toLowerCase()
                    .includes(query) ||
                (item.locationName ?? "")
                    .toLowerCase()
                    .includes(query) ||
                (item.batchNumber ?? "")
                    .toLowerCase()
                    .includes(query);

            return (
                matchesCategory &&
                matchesSearch
            );
        });
    }, [
        stock,
        selectedCategory,
        search,
    ]);

    const inventoryData = useMemo(() => {
        const grouped = new Map<string, typeof stock>();

        filteredData.forEach((item) => {
            const existing =
                grouped.get(item.sku) ?? [];

            existing.push(item);

            grouped.set(item.sku, existing);
        });

        return Array.from(grouped.values())
            .map((items) => {
                const first = items[0];

                const averageUnitPrice =
                    items.reduce(
                        (total, item) =>
                            total + item.unitPrice,
                        0
                    ) / items.length;

                const totalQuantity =
                    items.reduce(
                        (total, item) =>
                            total + item.quantity,
                        0
                    );

                const totalAvailableQuantity =
                    items.reduce(
                        (total, item) =>
                            total +
                            item.availableQuantity,
                        0
                    );

                const totalReservedQuantity =
                    items.reduce(
                        (total, item) =>
                            total +
                            item.reservedQuantity,
                        0
                    );

                const locationCount =
                    new Set(
                        items
                            .map(
                                (item) =>
                                    item.locationName
                            )
                            .filter(Boolean)
                    ).size;

                const stockValue =
                    items.reduce(
                        (total, item) =>
                            total +
                            item.quantity *
                                item.unitPrice,
                        0
                    );

                const hasQuarantined =
                    items.some(
                        (item) =>
                            item.stockStatus ===
                            "Quarantined"
                    );

                const hasDamaged =
                    items.some(
                        (item) =>
                            item.stockStatus ===
                            "Damaged"
                    );

                const hasExpired =
                    items.some(
                        (item) =>
                            item.stockStatus ===
                            "Expired"
                    );

                let stockStatus = "Available";

                if (hasQuarantined) {
                    stockStatus =
                        "Quarantined";
                } else if (hasDamaged) {
                    stockStatus = "Damaged";
                } else if (hasExpired) {
                    stockStatus = "Expired";
                } else if (
                    totalQuantity === 0
                ) {
                    stockStatus =
                        "Out of Stock";
                } else if (
                    totalQuantity <
                    first.minimumStock
                ) {
                    stockStatus =
                        "Low Stock";
                }

                return {
                    ...first,
                    quantity: totalQuantity,
                    availableQuantity:
                        totalAvailableQuantity,
                    reservedQuantity:
                        totalReservedQuantity,
                    locationCount,
                    unitPrice:
                        averageUnitPrice,
                    stockValue,
                    stockStatus,
                };
            })
            .filter((item) => {
                return (
                    selectedStatus === "All" ||
                    item.stockStatus ===
                        selectedStatus
                );
            });
    }, [
        filteredData,
        selectedStatus,
    ]);

    const formatNumber = (value: number) =>
        new Intl.NumberFormat("en-US").format(
            value
        );

    const formatCurrency = (value: number) =>
        new Intl.NumberFormat("en-US", {
            style: "currency",
            currency: "USD",
            maximumFractionDigits: 2,
        }).format(value);

    const getStatusVariant = (
        status: string
    ): "green" | "red" | "orange" | "purple" => {
        switch (status) {
            case "Available":
                return "green";

            case "Out of Stock":
                return "red";

            case "Low Stock":
                return "orange";

            case "Quarantined":
                return "purple";

            default:
                return "red";
        }
    };

    const detailsOpen =
        selectedProductId !== null;

    return (
        <>
            <div
                style={{
                    width: "100%",
                    minHeight: "100vh",
                    display: "flex",
                    alignItems: "flex-start",
                    padding: "var(--content-padding)",
                    boxSizing: "border-box",
                    gap: "var(--content-gap)",
                    overflow: "hidden",
                }}
            >
                {/* Inventory Card */}
                <div
                    style={{
                        flex: detailsOpen
                            ? "1 1 0"
                            : "1 1 100%",
                        minWidth: 0,
                        width: detailsOpen
                            ? "calc(100% - var(--sidebar-width) - var(--content-gap))"
                            : "100%",
                        transition:
                            "width 0.25s ease, flex 0.25s ease",
                    }}
                >
                    <Card
                        title={`Product Inventory - ${inventoryData.length} Records`}
                        header={
                            <div
                                style={{
                                    display: "flex",
                                    alignItems: "center",
                                    gap: "var(--space-3)",
                                    width: "100%",
                                }}
                            >
                                <SearchBar
                                    placeholder="Search SKU, Product, Category..."
                                    value={search}
                                    onChange={setSearch}
                                    style={{
                                        flex: 1,
                                        minWidth: 0,
                                    }}
                                />

                                <Dropdown
                                    options={statuses}
                                    value={selectedStatus}
                                    onChange={
                                        setSelectedStatus
                                    }
                                    placeholder="Select Status"
                                    style={{
                                        width: "180px",
                                        flexShrink: 0,
                                    }}
                                />

                                <Dropdown
                                    options={categories}
                                    value={selectedCategory}
                                    onChange={
                                        setSelectedCategory
                                    }
                                    placeholder="Select Category"
                                    style={{
                                        width: "180px",
                                        flexShrink: 0,
                                    }}
                                />

                                <Button
                                    variant="secondary"
                                    onClick={() =>
                                        setAddProductOpen(
                                            true
                                        )
                                    }
                                >
                                    <p className="nav-item">
                                        +
                                    </p>
                                    Add Product
                                </Button>
                            </div>
                        }
                    >
                        {loading && (
                            <div
                                style={{
                                    padding:
                                        "var(--space-6)",
                                    textAlign: "center",
                                }}
                            >
                                Loading inventory...
                            </div>
                        )}

                        {error && !loading && (
                            <div
                                style={{
                                    padding:
                                        "var(--space-6)",
                                    textAlign: "center",
                                    color: "var(--blood-red)",
                                }}
                            >
                                Failed to load
                                inventory.
                                <br />
                                {error}
                            </div>
                        )}

                        {!loading && !error && (
                            <div
                                style={{
                                    width: "100%",
                                    overflowX: "auto",
                                }}
                            >
                                <table
                                    style={{
                                        width: "100%",
                                        minWidth:
                                            "1200px",
                                        borderCollapse:
                                            "collapse",
                                    }}
                                >
                                    <thead>
                                        <tr
                                            style={{
                                                width: "100%",
                                                backgroundColor:
                                                    "var(--beige)",
                                                borderTop:
                                                    "var(--border-default)",
                                                borderBottom:
                                                    "var(--border-default)",
                                            }}
                                        >
                                            <th
                                                style={{
                                                    ...headerStyle,
                                                    textAlign:
                                                        "left",
                                                }}
                                            >
                                                SKU
                                            </th>

                                            <th
                                                style={
                                                    headerStyle
                                                }
                                            >
                                                Product
                                            </th>

                                            <th
                                                style={
                                                    headerStyle
                                                }
                                            >
                                                Category
                                            </th>

                                            <th
                                                style={
                                                    headerStyle
                                                }
                                            >
                                                Total Qty
                                            </th>

                                            <th
                                                style={
                                                    headerStyle
                                                }
                                            >
                                                Available
                                            </th>

                                            <th
                                                style={
                                                    headerStyle
                                                }
                                            >
                                                Reserved
                                            </th>

                                            <th
                                                style={
                                                    headerStyle
                                                }
                                            >
                                                Locations
                                            </th>

                                            <th
                                                style={
                                                    headerStyle
                                                }
                                            >
                                                Status
                                            </th>

                                            <th
                                                style={
                                                    headerStyle
                                                }
                                            >
                                                Min Stock
                                            </th>

                                            <th
                                                style={
                                                    headerStyle
                                                }
                                            >
                                                Avg Unit Price
                                            </th>

                                            <th
                                                style={
                                                    headerStyle
                                                }
                                            >
                                                Stock Value
                                            </th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        {inventoryData.map(
                                            (item) => (
                                                <tr
                                                    key={
                                                        item.stockId
                                                    }
                                                    onClick={() =>
                                                        setSelectedProductId(
                                                            item.productId
                                                        )
                                                    }
                                                    style={{
                                                        borderBottom:
                                                            "var(--border-default)",
                                                        cursor: "pointer",
                                                    }}
                                                >
                                                    <td
                                                        style={
                                                            cellStyle
                                                        }
                                                    >
                                                        <span
                                                            style={{
                                                                fontWeight:
                                                                    700,
                                                            }}
                                                        >
                                                            {
                                                                item.sku
                                                            }
                                                        </span>
                                                    </td>

                                                    <td
                                                        style={
                                                            cellStyle
                                                        }
                                                    >
                                                        {
                                                            item.productName
                                                        }
                                                    </td>

                                                    <td
                                                        style={
                                                            cellStyle
                                                        }
                                                    >
                                                        {item.categoryName ??
                                                            "—"}
                                                    </td>

                                                    <td
                                                        style={
                                                            cellStyle
                                                        }
                                                    >
                                                        {formatNumber(
                                                            item.quantity
                                                        )}
                                                    </td>

                                                    <td
                                                        style={
                                                            cellStyle
                                                        }
                                                    >
                                                        {formatNumber(
                                                            item.availableQuantity
                                                        )}
                                                    </td>

                                                    <td
                                                        style={
                                                            cellStyle
                                                        }
                                                    >
                                                        {formatNumber(
                                                            item.reservedQuantity
                                                        )}
                                                    </td>

                                                    <td
                                                        style={
                                                            cellStyle
                                                        }
                                                    >
                                                        {formatNumber(
                                                            item.locationCount
                                                        )}
                                                    </td>

                                                    <td
                                                        style={
                                                            cellStyle
                                                        }
                                                    >
                                                        <Status
                                                            text={
                                                                item.stockStatus
                                                            }
                                                            variant={getStatusVariant(
                                                                item.stockStatus
                                                            )}
                                                        />
                                                    </td>

                                                    <td
                                                        style={
                                                            cellStyle
                                                        }
                                                    >
                                                        {formatNumber(
                                                            item.minimumStock
                                                        )}
                                                    </td>

                                                    <td
                                                        style={
                                                            cellStyle
                                                        }
                                                    >
                                                        {formatCurrency(
                                                            item.unitPrice
                                                        )}
                                                    </td>

                                                    <td
                                                        style={{
                                                            ...cellStyle,
                                                            fontWeight: 500,
                                                        }}
                                                    >
                                                        {formatCurrency(
                                                            item.stockValue
                                                        )}
                                                    </td>
                                                </tr>
                                            )
                                        )}

                                        {inventoryData.length ===
                                            0 && (
                                                <tr>
                                                    <td
                                                        colSpan={
                                                            11
                                                        }
                                                        style={{
                                                            padding:
                                                                "var(--space-6)",
                                                            textAlign:
                                                                "center",
                                                        }}
                                                    >
                                                        No
                                                        inventory
                                                        records
                                                        found.
                                                    </td>
                                                </tr>
                                            )}
                                    </tbody>
                                </table>
                            </div>
                        )}
                    </Card>
                </div>

                {/* Product Details */}
                {detailsOpen && (
                    <div
                        style={{
                            flex: `0 0 var(--sidebar-width)`,
                            width: "var(--sidebar-width)",
                            maxWidth:
                                "var(--sidebar-width)",
                            minWidth: 0,
                            maxHeight:
                                "calc(100vh - 2 * var(--content-padding))",
                            overflowY: "auto",
                        }}
                    >
                        <ProductDetails
                            productId={
                                selectedProductId
                            }
                            onClose={() =>
                                setSelectedProductId(
                                    null
                                )
                            }
                            status={
                                inventoryData.find(
                                    (item) =>
                                        item.productId ===
                                        selectedProductId
                                )?.stockStatus ??
                                "Available"
                            }
                        />
                    </div>
                )}
            </div>

            {/* Add Product Popup */}
            {addProductOpen && (
                <div
                    style={{
                        position: "fixed",
                        inset: 0,
                        zIndex: 1000,
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                        backgroundColor:
                            "rgba(0, 0, 0, 0.35)",
                        padding: "var(--content-padding)",
                        boxSizing: "border-box",
                    }}
                    onClick={() =>
                        setAddProductOpen(false)
                    }
                >
                    <div
                        onClick={(event) =>
                            event.stopPropagation()
                        }
                    >
                        <AddProduct
                            onClose={() =>
                                setAddProductOpen(
                                    false
                                )
                            }
                        />
                    </div>
                </div>
            )}
        </>
    );
}

const headerStyle: React.CSSProperties = {
    padding: "var(--space-3)",
    textAlign: "left",
    whiteSpace: "nowrap",
};

const cellStyle: React.CSSProperties = {
    padding: "var(--space-3)",
    textAlign: "left",
    whiteSpace: "nowrap",
};