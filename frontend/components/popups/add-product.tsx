"use client";

import { useEffect, useState } from "react";
import Input from "@/components/input";
import Button from "@/components/button";
import { createProduct } from "@/lib/api/products";
import { getCategories } from "@/lib/api/categories";
import { getUnits } from "@/lib/api/units";
import { getSuppliers } from "@/lib/api/suppliers";
import type {
    CreateProductDto,
    CategoryDto,
    UnitDto,
    SupplierDto,
} from "@/types";

interface AddProductProps {
    onClose?: () => void;
}

export default function AddProduct({
    onClose,
}: AddProductProps) {
    const [sku, setSku] = useState("");
    const [productName, setProductName] = useState("");
    const [barcode, setBarcode] = useState("");
    const [qrValue, setQrValue] = useState("");
    const [category, setCategory] = useState("");
    const [unitPrice, setUnitPrice] = useState("");
    const [minimumStock, setMinimumStock] = useState("");
    const [unit, setUnit] = useState("");
    const [description, setDescription] = useState("");

    const [categories, setCategories] = useState<CategoryDto[]>([]);
    const [units, setUnits] = useState<UnitDto[]>([]);
    const [suppliers, setSuppliers] = useState<SupplierDto[]>([]);

    const [loading, setLoading] = useState(false);
    const [loadingOptions, setLoadingOptions] = useState(true);
    const [error, setError] = useState("");

    // Load categories, units and suppliers
    useEffect(() => {
        const loadOptions = async () => {
            try {
                setLoadingOptions(true);
                setError("");

                const [
                    categoriesData,
                    unitsData,
                    suppliersData,
                ] = await Promise.all([
                    getCategories(),
                    getUnits(),
                    getSuppliers(),
                ]);

                setCategories(
                    categoriesData.filter(
                        (category) => category.isActive
                    )
                );

                setUnits(unitsData);

                setSuppliers(
                    suppliersData.filter(
                        (supplier) =>
                            supplier.isActive &&
                            supplier.supplierStatus === "Active"
                    )
                );
            } catch (err) {
                console.error(
                    "Failed to load product options:",
                    err
                );

                setError(
                    err instanceof Error
                        ? err.message
                        : "Failed to load categories, units and suppliers."
                );
            } finally {
                setLoadingOptions(false);
            }
        };

        loadOptions();
    }, []);

    const categoryOptions = categories.map(
        (category) => ({
            label: category.name,
            value: category.categoryId.toString(),
        })
    );

    const unitOptions = units.map((unit) => ({
        label: `${unit.name}${unit.abbreviation
                ? ` (${unit.abbreviation})`
                : ""
            }`,
        value: unit.unitId.toString(),
    }));


    const handleAddProduct = async () => {
        setError("");

        if (
            !sku ||
            !productName ||
            !barcode ||
            !category ||
            !unit ||
            !unitPrice ||
            !minimumStock
        ) {
            setError(
                "Please fill in all required fields."
            );
            return;
        }

        const product: CreateProductDto = {
            sku,
            barcode,
            qrValue,
            name: productName,
            categoryId: Number(category),
            unitId: Number(unit),
            unitPrice: Number(unitPrice),
            minimumStock: Number(minimumStock),
            description,
        };

        try {
            setLoading(true);

            await createProduct(product);

            console.log(
                "Product created:",
                product
            );

            onClose?.();
        } catch (err) {
            console.error(
                "Failed to create product:",
                err
            );

            setError(
                err instanceof Error
                    ? err.message
                    : "Failed to create product."
            );
        } finally {
            setLoading(false);
        }
    };

    return (
        <div
            style={{
                display: "flex",
                flexDirection: "column",
                paddingBlock: "var(--space-5)",
                height: "fit-content",
                border: "var(--border-default)",
                borderColor: "var(--light-grey)",
                color: "var(--midnight-blue)",
                width: "var(--card-width-xl)",
                borderRadius: "var(--radius-lg)",
                gap: "var(--space-5)",
                backgroundColor: "var(--beige)",
            }}
        >
            {/* Header */}
            <div
                style={{
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "space-between",
                    paddingInline: "var(--space-5)",
                }}
            >
                <p className="page-title">
                    Add Product
                </p>

                <button
                    type="button"
                    onClick={onClose}
                    className="page-title"
                    style={{
                        border: "none",
                        background: "transparent",
                        cursor: "pointer",
                        color: "var(--midnight-blue)",
                        padding: 0,
                    }}
                >
                    X
                </button>
            </div>

            {/* Divider */}
            <div
                style={{
                    width: "100%",
                    height: "1px",
                    backgroundColor:
                        "var(--light-grey)",
                }}
            />

            {/* Form */}
            <div
                style={{
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "center",
                    justifyContent: "space-between",
                    paddingInline: "var(--space-5)",
                    gap: "var(--space-5)",
                    width: "100%",
                }}
            >
                {/* Product Name */}
                <Input
                    label="Product Name"
                    placeholder="Enter Product Name"
                    value={productName}
                    onChange={setProductName}
                    maxWidth
                />

                {/*  SKU / Barcode */}
                <div
                    style={{
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "flex-start",
                        gap: "var(--space-5)",
                        width: "100%",
                    }}
                >
                    <Input
                        label="SKU"
                        placeholder="Enter Product SKU"
                        value={sku}
                        onChange={setSku}
                    />
                    <Input
                        label="Barcode"
                        placeholder="Scan Product Barcode"
                        value={barcode}
                        onChange={setBarcode}
                        maxWidth
                    />

                </div>


                {/* Category / Unit */}
                <div
                    style={{
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "flex-start",
                        gap: "var(--space-5)",
                        width: "100%",
                    }}
                >
                    <Input
                        label="Category"
                        placeholder={
                            loadingOptions
                                ? "Loading categories..."
                                : "Choose Category"
                        }
                        options={categoryOptions}
                        value={category}
                        onChange={setCategory}
                    />

                <Input
                    label="Unit of Measure"
                    placeholder={
                        loadingOptions
                            ? "Loading units..."
                            : "Choose Unit"
                    }
                    options={unitOptions}
                    value={unit}
                    onChange={setUnit}
                />
                   
                </div>

                {/* Minimum Stock / Unit Price */}
                <div
                    style={{
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "flex-start",
                        gap: "var(--space-5)",
                        width: "100%",
                    }}
                >
                    <Input
                        label="Minimum Stock"
                        placeholder="Enter Minimum Stock"
                        value={minimumStock}
                        onChange={setMinimumStock}
                        type="number"
                        maxWidth
                    />

                    <Input
                        label="Unit Price"
                        placeholder="Enter Unit Price"
                        value={unitPrice}
                        onChange={setUnitPrice}
                        type="number"
                    />

                </div>


                {/* Description */}
                <Input
                    label="Description"
                    placeholder="Enter Product Description"
                    value={description}
                    onChange={setDescription}
                    maxWidth
                    optional
                />

                {/* Error */}
                {error && (
                    <p
                        style={{
                            width: "100%",
                            color: "var(--blood-red)",
                            margin: 0,
                        }}
                    >
                        {error}
                    </p>
                )}

                {/* Actions */}
                <div
                    style={{
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "flex-start",
                        gap: "var(--space-5)",
                        width: "100%",
                    }}
                >
                    <Button
                        style={{ width: "100%" }}
                        onClick={onClose}
                    >
                        Cancel
                    </Button>

                    <Button
                        variant="secondary"
                        style={{ width: "100%" }}
                        onClick={handleAddProduct}
                        disabled={
                            loading ||
                            loadingOptions
                        }
                    >
                        {loading
                            ? "Adding..."
                            : "Add Product"}
                    </Button>
                </div>
            </div>
        </div>
    );
}