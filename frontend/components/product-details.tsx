"use client";

import Card from "@/components/card";
import Status from "@/components/status";
import { useStockByProductId } from "@/hooks/useStock";

interface ProductDetailsProps {
    productId: number;
    onClose?: () => void;
    status: string;
}

export default function ProductDetails({
    productId,
    onClose,
    status,
}: ProductDetailsProps) {
    const {
        stock,
        loading,
        error,
    } = useStockByProductId(productId);

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

    if (loading) {
        return (
            <Card
                title="Product Details"
                maxWidth="var(--sidebar-width)"
            >
                <div
                    style={{
                        padding: "var(--space-3)",
                    }}
                >
                    <p className="body">
                        Loading product details...
                    </p>
                </div>
            </Card>
        );
    }

    if (error) {
        return (
            <Card
                title="Product Details"
                maxWidth="var(--sidebar-width)"
            >
                <div
                    style={{
                        padding: "var(--space-3)",
                    }}
                >
                    <p className="body">{error}</p>
                </div>
            </Card>
        );
    }

    if (!stock) {
        return (
            <Card
                title="Product Details"
                maxWidth="var(--sidebar-width)"
            >
                <div
                    style={{
                        padding: "var(--space-3)",
                    }}
                >
                    <p className="body">
                        Product not found.
                    </p>
                </div>
            </Card>
        );
    }

    const stockRecords = Array.isArray(stock)
        ? stock
        : [stock];

    if (stockRecords.length === 0) {
        return (
            <Card
                title="Product Details"
                maxWidth="var(--sidebar-width)"
            >
                <div
                    style={{
                        padding: "var(--space-3)",
                    }}
                >
                    <p className="body">
                        No stock found for this product.
                    </p>
                </div>
            </Card>
        );
    }

    const firstStock = stockRecords[0];

    const totalQuantity = stockRecords.reduce(
        (total, item) => total + item.quantity,
        0
    );

    const totalAvailable = stockRecords.reduce(
        (total, item) => total + item.availableQuantity,
        0
    );

    const totalReserved = stockRecords.reduce(
        (total, item) => total + item.reservedQuantity,
        0
    );

    const totalStockValue = stockRecords.reduce(
        (total, item) =>
            total + item.quantity * item.unitPrice,
        0
    );

    return (
        <Card
            title="Product Details"
            header={
                <button
                    onClick={onClose}
                    style={{
                        marginLeft: "auto",
                        fontSize: "var(--icon-sm)",
                        fontWeight: "var(--weight-regular)",
                        fontFamily:
                            "var(--font-roboto-serif)",
                        cursor: "pointer",
                        border: "none",
                        background: "transparent",
                    }}
                >
                    X
                </button>
            }
            maxWidth="var(--sidebar-width)"
        >
            <div
                style={{
                    borderTop: "var(--border-default)",
                    padding: "var(--space-3)",
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "flex-start",
                    justifyContent: "flex-start",
                    gap: "var(--space-3)",
                }}
            >
                {/* Product Name */}
                <p className="card-title">
                    {firstStock.productName}
                </p>

                {/* Product Info */}
                <div
                    style={{
                        width: "100%",
                        display: "flex",
                        flexDirection: "column",
                        gap: "var(--space-3)",
                    }}
                >
                    <div
                        style={{
                            width: "100%",
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "space-between",
                        }}
                    >
                        <p className="body">SKU</p>
                        <p className="body-title">
                            {firstStock.sku}
                        </p>
                    </div>

                    {"barcode" in firstStock && (
                        <div
                            style={{
                                width: "100%",
                                display: "flex",
                                alignItems: "center",
                                justifyContent:
                                    "space-between",
                            }}
                        >
                            <p className="body">Barcode</p>
                            <p className="body-title">
                                {firstStock.barcode || "-"}
                            </p>
                        </div>
                    )}

                    <div
                        style={{
                            width: "100%",
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "space-between",
                        }}
                    >
                        <p className="body">Category</p>
                        <p className="body-title">
                            {firstStock.categoryName}
                        </p>
                    </div>

                    {"unitName" in firstStock && (
                        <div
                            style={{
                                width: "100%",
                                display: "flex",
                                alignItems: "center",
                                justifyContent:
                                    "space-between",
                            }}
                        >
                            <p className="body">Unit</p>
                            <p className="body-title">
                                {firstStock.unitName || "-"}
                            </p>
                        </div>
                    )}

                    <div
                        style={{
                            width: "100%",
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "space-between",
                        }}
                    >
                        <p className="body">
                            Minimum Stock
                        </p>
                        <p className="body-title">
                            {firstStock.minimumStock}
                        </p>
                    </div>
                </div>

                {/* Inventory Summary */}
                <div
                    style={{
                        width: "100%",
                        border: "var(--border-default)",
                        borderRadius: "var(--radius-md)",
                        backgroundColor: "var(--beige)",
                        padding: "var(--space-3)",
                        display: "flex",
                        flexDirection: "column",
                        alignItems: "flex-start",
                        justifyContent: "flex-start",
                        gap: "var(--space-1)",
                        boxSizing: "border-box",
                    }}
                >
                    <p className="card-title">
                        Inventory Summary
                    </p>

                    <div
                        style={{
                            width: "100%",
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "space-between",
                        }}
                    >
                        <p className="body">Total Qty</p>
                        <p className="body-title">
                            {totalQuantity}
                        </p>
                    </div>

                    <div
                        style={{
                            width: "100%",
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "space-between",
                        }}
                    >
                        <p className="body">Available</p>
                        <p
                            className="body-title"
                            style={{
                                color: "var(--dark-green)",
                            }}
                        >
                            {totalAvailable}
                        </p>
                    </div>

                    <div
                        style={{
                            width: "100%",
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "space-between",
                        }}
                    >
                        <p className="body">Reserved</p>
                        <p
                            className="body-title"
                            style={{
                                color: "var(--red)",
                            }}
                        >
                            {totalReserved}
                        </p>
                    </div>

                    <div
                        style={{
                            width: "100%",
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "space-between",
                        }}
                    >
                        <p className="body">
                            Stock Value
                        </p>
                        <p className="body-title">
                            {totalStockValue.toLocaleString(
                                undefined,
                                {
                                    minimumFractionDigits: 2,
                                    maximumFractionDigits: 2,
                                }
                            )}
                        </p>
                    </div>
                </div>

                {/* Stock Status */}
                <div
                    style={{
                        width: "100%",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "flex-start",
                        gap: "var(--space-1)",
                    }}
                >
                    <p className="body">
                        Stock Status:
                    </p>

                    <Status
                        variant={getStatusVariant(status)}
                        text={status}
                    />
                </div>

                {/* Stock By Location */}
                <div
                    style={{
                        width: "100%",
                        display: "flex",
                        flexDirection: "column",
                        gap: "var(--space-3)",
                    }}
                >
                    <p className="card-title">
                        Stock by Location (
                        {stockRecords.length}{" "}
                        {stockRecords.length === 1
                            ? "Record"
                            : "Records"}
                        )
                    </p>

                    {stockRecords.map((item) => {
                        const locationStatus =
                            item.quantity === 0
                                ? "Out of Stock"
                                : item.stockStatus;

                        return (
                            <div
                                key={item.stockId}
                                style={{
                                    width: "100%",
                                    border: "var(--border-default)",
                                    borderRadius:
                                        "var(--radius-md)",
                                    backgroundColor:
                                        "var(--beige)",
                                    padding:
                                        "var(--space-3)",
                                    display: "flex",
                                    flexDirection:
                                        "column",
                                    alignItems:
                                        "flex-start",
                                    justifyContent:
                                        "flex-start",
                                    gap: "var(--space-1)",
                                    boxSizing:
                                        "border-box",
                                }}
                            >
                                {/* Stock Code + Status */}
                                <div
                                    style={{
                                        width: "100%",
                                        display: "flex",
                                        alignItems:
                                            "center",
                                        justifyContent:
                                            "space-between",
                                    }}
                                >
                                    <p className="card-title">
                                        {item.stockCode}
                                    </p>

                                    <Status
                                        variant={getStatusVariant(
                                            locationStatus
                                        )}
                                        text={locationStatus}
                                    />
                                </div>

                                {/* Location */}
                                <p className="small-body">
                                    {item.warehouseName} /{" "}
                                    {item.locationName}
                                </p>

                                {/* Quantity */}
                                <div
                                    style={{
                                        width: "100%",
                                        display: "flex",
                                        alignItems:
                                            "center",
                                        justifyContent:
                                            "flex-start",
                                        gap: "var(--space-3)",
                                    }}
                                >
                                    <div
                                        style={{
                                            display: "flex",
                                            flexDirection:
                                                "column",
                                            alignItems:
                                                "center",
                                        }}
                                    >
                                        <p className="small-body">
                                            QTY
                                        </p>
                                        <p className="small-body">
                                            {item.quantity}
                                        </p>
                                    </div>

                                    <div
                                        style={{
                                            display: "flex",
                                            flexDirection:
                                                "column",
                                            alignItems:
                                                "center",
                                        }}
                                    >
                                        <p className="small-body">
                                            AVAIL
                                        </p>
                                        <p
                                            className="small-body"
                                            style={{
                                                color: "var(--dark-green)",
                                            }}
                                        >
                                            {item.availableQuantity}
                                        </p>
                                    </div>

                                    <div
                                        style={{
                                            display: "flex",
                                            flexDirection:
                                                "column",
                                            alignItems:
                                                "center",
                                        }}
                                    >
                                        <p className="small-body">
                                            RESV
                                        </p>
                                        <p
                                            className="small-body"
                                            style={{
                                                color: "var(--red)",
                                            }}
                                        >
                                            {item.reservedQuantity}
                                        </p>
                                    </div>
                                </div>
                            </div>
                        );
                    })}
                </div>
            </div>
        </Card>
    );
}