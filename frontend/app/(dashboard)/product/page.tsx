"use client";

import { useState } from "react";
import { useSearchParams } from "next/navigation";

import Status from "@/components/status";
import Button from "@/components/button";
import Tabs from "@/components/tabs";
import StatsCard from "@/components/stats-card";
import StockDistributionItem from "@/components/stock-distribution-item";
import { useProductInventory } from "@/hooks/useProductInventory";

export default function Product() {
    const searchParams = useSearchParams();

    const productIdParam = searchParams.get("productId");

    const productId =
        productIdParam &&
        !Number.isNaN(Number(productIdParam))
            ? Number(productIdParam)
            : null;

    const [activeTab, setActiveTab] = useState(
        "Product Information"
    );

    const {
        data,
        loading,
        error,
    } = useProductInventory(productId);

    const tabs = [
        {
            label: "Product Information",
        },
        {
            label: "Stock By Location",
            count: data?.stock.length ?? 0,
        },
    ];

    const formatCurrency = (value: number) =>
        `EGP ${value.toLocaleString("en-EG", {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
        })}`;

    const formatDate = (date: string | null) => {
        if (!date) return "-";

        return new Date(date).toLocaleDateString("en-GB");
    };

    if (!productId) {
        return (
            <div className="page-column">
                <div
                    className="card"
                    style={{
                        width: "100%",
                        boxSizing: "border-box",
                    }}
                >
                    <p>
                        Invalid or missing product ID.
                    </p>
                </div>
            </div>
        );
    }

    if (loading) {
        return (
            <div className="page-column">
                <div
                    className="card"
                    style={{
                        width: "100%",
                        boxSizing: "border-box",
                    }}
                >
                    <p>Loading product...</p>
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="page-column">
                <div
                    className="card"
                    style={{
                        width: "100%",
                        boxSizing: "border-box",
                    }}
                >
                    <p>{error}</p>
                </div>
            </div>
        );
    }

    if (!data) {
        return (
            <div className="page-column">
                <div
                    className="card"
                    style={{
                        width: "100%",
                        boxSizing: "border-box",
                    }}
                >
                    <p>Product not found.</p>
                </div>
            </div>
        );
    }

    const productInfo = [
        ["Product Name", data.productName],
        ["SKU", data.sku],
        ["Barcode", data.barcode],
        ["Category", data.category],
        ["Unit of Measure", data.unit],
        [
            "Minimum Stock Level",
            data.minimumStock.toLocaleString(),
        ],
        [
            "Unit Price",
            data.stock.length > 0
                ? formatCurrency(data.stock[0].price)
                : "-",
        ],
        ["Status", data.status],
    ];

    const stats = [
        {
            value: data.totalQuantity.toLocaleString(),
            title: "Total Quantity",
            subtitle: data.unit,
        },
        {
            value: data.totalAvailable.toLocaleString(),
            title: "Available",
            subtitle: data.unit,
            valueColor: "var(--dahra-green)",
        },
        {
            value: data.totalReserved.toLocaleString(),
            title: "Reserved",
            subtitle: data.unit,
            valueColor: "var(--orange)",
        },
        {
            value: formatCurrency(data.stockValue),
            title: "Stock Value",
            valueColor: "var(--dark-green)",
        },
        {
            value: data.numberOfLocations.toString(),
            title: "Locations",
            subtitle: "Stock Records",
        },
    ];

    const stockDistribution = data.stock.map(
        (stock) => ({
            location: [
                stock.location.warehouse,
                stock.location.room,
                stock.location.rack,
                stock.location.shelf,
            ]
                .filter(Boolean)
                .join(" / "),

            bin: stock.location.bin ?? "-",

            units: stock.quantity,

            available: stock.available,

            reserved: stock.reserved,

            batch: stock.batch ?? "-",
            totalQuantity: data.totalQuantity,
        })
    );

    const getStatusVariant = (
        status: string
    ): "green" | "red" | "orange" | "purple" => {
        switch (status.toLowerCase()) {
            case "available":
                return "green";
            case "low stock":
                return "orange";
            case "out of stock":
                return "red";
            case "quarantined":
                return "purple";
            default:
                return "red";
        }
    };

    return (
        <div className="page-column">

            {/* Product Header */}
            <div
                className="card column-container"
                style={{
                    width: "100%",
                }}
            >
                <div
                    className="row-container"
                    style={{
                        justifyContent: "space-between",
                        width: "100%",
                    }}
                >
                    <div className="row-container">
                        <p className="page-title">
                            {data.productName}
                        </p>

                        <Status text={data.status} variant={getStatusVariant(data.status)} />
                    </div>

                    <div className="row-container">
                        <Button>Transfer</Button>
                        <Button>Adjust</Button>
                        <Button>Receive Stock</Button>
                    </div>
                </div>

                <div className="row-container">
                    <p
                        className="body-title"
                        style={{
                            color: "var(--dark-green)",
                        }}
                    >
                        {data.sku}
                    </p>

                    <p className="body-title">
                        •
                    </p>

                    <p className="body-title">
                        {data.barcode}
                    </p>

                    <p className="body-title">
                        •
                    </p>

                    <p className="body-title">
                        {data.category}
                    </p>

                    <p className="body-title">
                        •
                    </p>

                    <p className="body-title">
                        Unit: {data.unit}
                    </p>
                </div>
            </div>

            {/* Stats */}
            <div
                className="row-container"
                style={{
                    width: "100%",
                }}
            >
                {stats.map((stat) => (
                    <StatsCard
                        key={stat.title}
                        value={stat.value}
                        title={stat.title}
                        subtitle={stat.subtitle}
                        valueColor={stat.valueColor}
                    />
                ))}
            </div>

            {/* Tabs */}
            <Tabs
                tabs={tabs}
                activeTab={activeTab}
                onChange={setActiveTab}
            />

            {/* Product Information */}
            {activeTab === "Product Information" && (
                <div
                    className="row-container"
                    style={{
                        width: "100%",
                        alignItems: "stretch",
                        display: "grid",
                        gridTemplateColumns:
                            "repeat(2, minmax(0, 1fr))",
                    }}
                >
                    {/* Product Information Card */}
                    <div
                        className="card column-container"
                        style={{
                            flex: 1,
                            boxSizing: "border-box",
                            height: "100%",
                        }}
                    >
                        <p className="card-title">
                            Product Information
                        </p>

                        <div
                            className="column-container"
                            style={{
                                width: "100%",
                                gap: "var(--space-2)",
                            }}
                        >
                            {productInfo.map(
                                ([label, value]) => (
                                    <div
                                        key={label}
                                        className="row-container"
                                        style={{
                                            width: "100%",
                                            justifyContent:
                                                "space-between",
                                            borderBottom:
                                                "var(--border-default)",
                                            marginBottom:
                                                "var(--space-2)",
                                        }}
                                    >
                                        <p>{label}</p>

                                        <p
                                            className="body-title"
                                            style={{
                                                color:
                                                    "var(--midnight-blue)",
                                            }}
                                        >
                                            {value}
                                        </p>
                                    </div>
                                )
                            )}
                        </div>
                    </div>

                    {/* Stock Distribution */}
                    <div
                        className="card column-container"
                        style={{
                            flex: 1,
                            boxSizing: "border-box",
                            height: "100%",
                        }}
                    >
                        <p className="card-title">
                            Stock Distribution
                        </p>

                        <div
                            className="column-container"
                            style={{
                                width: "100%",
                                gap: "var(--space-2)",
                            }}
                        >
                            <p>
                                Inventory is distributed
                                across{" "}
                                <span className="body-title">
                                    {data.stock.length} stock
                                    record
                                    {data.stock.length !== 1
                                        ? "s"
                                        : ""}
                                </span>{" "}
                                in{" "}
                                <span className="body-title">
                                    {
                                        data.numberOfLocations
                                    }{" "}
                                    location
                                    {data.numberOfLocations !==
                                    1
                                        ? "s"
                                        : ""}
                                </span>
                            </p>

                            {stockDistribution.map(
                                (stock, index) => (
                                    <StockDistributionItem
                                        key={`${stock.bin}-${index}`}
                                        {...stock}
                                    />
                                )
                            )}
                        </div>
                    </div>
                </div>
            )}

            {/* Stock By Location */}
            {activeTab === "Stock By Location" && (
                <div
                    className="card column-container"
                    style={{
                        width: "100%",
                        padding: 0,
                        gap: 0,
                        overflow: "hidden",
                        boxSizing: "border-box",
                    }}
                >
                    {/* Table Title */}
                    <div
                        style={{
                            width: "100%",
                            padding:
                                "var(--space-2) var(--space-3)",
                            borderBottom:
                                "var(--border-default)",
                            boxSizing: "border-box",
                        }}
                    >
                        <p className="card-title">
                            Stock By Location -{" "}
                            {data.productName}
                        </p>
                    </div>

                    {/* Table */}
                    <div
                        style={{
                            width: "100%",
                            overflowX: "auto",
                        }}
                    >
                        <table
                            style={{
                                width: "100%",
                                minWidth: "1200px",
                                borderCollapse: "collapse",
                            }}
                        >
                            <thead>
                                <tr
                                    style={{
                                        backgroundColor:
                                            "var(--beige)",
                                        borderBottom:
                                            "var(--border-default)",
                                    }}
                                >
                                    {[
                                        "Stock Id",
                                        "Warehouse",
                                        "Room",
                                        "Rack",
                                        "Shelf",
                                        "Bin",
                                        "Batch / Lot",
                                        "Qty",
                                        "Reserved",
                                        "Available",
                                        "Unit Price",
                                        "Expiry",
                                        "Status",
                                    ].map((header) => (
                                        <th
                                            key={header}
                                            style={{
                                                padding:
                                                    "var(--space-2)",
                                                textAlign:
                                                    "left",
                                                whiteSpace:
                                                    "nowrap",
                                            }}
                                        >
                                            {header}
                                        </th>
                                    ))}
                                </tr>
                            </thead>

                            <tbody>
                                {data.stock.map((stock) => (
                                    <tr
                                        key={stock.stockId}
                                        style={{
                                            borderBottom:
                                                "var(--border-default)",
                                        }}
                                    >
                                        {/* Stock ID */}
                                        <td
                                            style={{
                                                padding:
                                                    "var(--space-2)",
                                                whiteSpace:
                                                    "nowrap",
                                            }}
                                        >
                                            {stock.stockId}
                                        </td>

                                        {/* Warehouse */}
                                        <td
                                            style={{
                                                padding:
                                                    "var(--space-2)",
                                                whiteSpace:
                                                    "nowrap",
                                            }}
                                        >
                                            {
                                                stock.location
                                                    .warehouse
                                            }
                                        </td>

                                        {/* Room */}
                                        <td
                                            style={{
                                                padding:
                                                    "var(--space-2)",
                                                whiteSpace:
                                                    "nowrap",
                                            }}
                                        >
                                            {
                                                stock.location
                                                    .room
                                            }
                                        </td>

                                        {/* Rack */}
                                        <td
                                            style={{
                                                padding:
                                                    "var(--space-2)",
                                                whiteSpace:
                                                    "nowrap",
                                            }}
                                        >
                                            {
                                                stock.location
                                                    .rack
                                            }
                                        </td>

                                        {/* Shelf */}
                                        <td
                                            style={{
                                                padding:
                                                    "var(--space-2)",
                                                whiteSpace:
                                                    "nowrap",
                                            }}
                                        >
                                            {
                                                stock.location
                                                    .shelf
                                            }
                                        </td>

                                        {/* Bin */}
                                        <td
                                            style={{
                                                padding:
                                                    "var(--space-2)",
                                            }}
                                        >
                                            <span
                                                style={{
                                                    display:
                                                        "inline-flex",
                                                    padding:
                                                        "2px 10px",
                                                    borderRadius:
                                                        "4px",
                                                    backgroundColor:
                                                        "#B8CCF8",
                                                    color:
                                                        "var(--midnight-blue)",
                                                }}
                                            >
                                                {
                                                    stock.location
                                                        .bin
                                                }
                                            </span>
                                        </td>

                                        {/* Batch */}
                                        <td
                                            style={{
                                                padding:
                                                    "var(--space-2)",
                                                whiteSpace:
                                                    "nowrap",
                                            }}
                                        >
                                            {stock.batch ??
                                                "-"}
                                        </td>

                                        {/* Quantity */}
                                        <td
                                            style={{
                                                padding:
                                                    "var(--space-2)",
                                                whiteSpace:
                                                    "nowrap",
                                            }}
                                        >
                                            {stock.quantity.toLocaleString()}
                                        </td>

                                        {/* Reserved */}
                                        <td
                                            style={{
                                                padding:
                                                    "var(--space-2)",
                                                whiteSpace:
                                                    "nowrap",
                                            }}
                                        >
                                            {stock.reserved.toLocaleString()}
                                        </td>

                                        {/* Available */}
                                        <td
                                            style={{
                                                padding:
                                                    "var(--space-2)",
                                                whiteSpace:
                                                    "nowrap",
                                            }}
                                        >
                                            {stock.available.toLocaleString()}
                                        </td>

                                        {/* Unit Price */}
                                        <td
                                            style={{
                                                padding:
                                                    "var(--space-2)",
                                                whiteSpace:
                                                    "nowrap",
                                            }}
                                        >
                                            {formatCurrency(
                                                stock.price
                                            )}
                                        </td>

                                        {/* Expiry */}
                                        <td
                                            style={{
                                                padding:
                                                    "var(--space-2)",
                                                whiteSpace:
                                                    "nowrap",
                                            }}
                                        >
                                            {formatDate(
                                                stock.expiry
                                            )}
                                        </td>

                                        {/* Status */}
                                        <td
                                            style={{
                                                padding:
                                                    "var(--space-2)",
                                            }}
                                        >
                                            <Status
                                                text={
                                                    stock.status
                                                }
                                                variant={getStatusVariant(stock.status)}
                                            />
                                        </td>
                                    </tr>
                                ))}

                                {data.stock.length === 0 && (
                                    <tr>
                                        <td
                                            colSpan={13}
                                            style={{
                                                padding:
                                                    "var(--space-4)",
                                                textAlign:
                                                    "center",
                                            }}
                                        >
                                            No stock records
                                            found.
                                        </td>
                                    </tr>
                                )}
                            </tbody>
                        </table>
                    </div>
                </div>
            )}
        </div>
    );
}