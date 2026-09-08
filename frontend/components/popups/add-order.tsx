"use client";

import Input from "@/components/input";
import Button from "@/components/button";
import { useEffect, useMemo, useState } from "react";

import { getSuppliers } from "@/lib/api/suppliers";
import { getWarehouses } from "@/lib/api/warehouses";

import {
    createPurchaseOrder,
    addPurchaseOrderItem,
    submitPurchaseOrder,
    updatePurchaseOrder,
    updatePurchaseOrderItem,
    deletePurchaseOrderItem,
} from "@/lib/api/purchase-orders";

import {
    getProductBySku,
    createProduct,
} from "@/lib/api/products";

import type { SupplierDto } from "@/types/supplier";
import type { WarehouseDto } from "@/types/warehouse";

import type {
    PurchaseOrderDto,
    PurchaseOrderItemDto,
} from "@/types/purchase-order";

interface AddOrderProps {
    onClose?: () => void;

    /*
     * When editOrder is provided,
     * this component works in EDIT mode.
     */
    editOrder?: PurchaseOrderDto;

    /*
     * Existing items belonging to the PO.
     */
    editItems?: PurchaseOrderItemDto[];
}

interface LineItem {
    id: number;

    sku: string;

    description: string;

    quantity: string;

    unitPrice: string;
}

export default function AddOrder({
    onClose,
    editOrder,
    editItems = [],
}: AddOrderProps) {
    const isEditMode = !!editOrder;

    const [loading, setLoading] = useState(false);

    const [loadingOptions, setLoadingOptions] =
        useState(true);

    const [error, setError] = useState("");

    const [success, setSuccess] = useState("");

    const [supplier, setSupplier] = useState("");

    const [warehouse, setWarehouse] = useState("");

    const [
        expectedDeliveryDate,
        setExpectedDeliveryDate,
    ] = useState("");

    const [notes, setNotes] = useState("");

    const [suppliers, setSuppliers] =
        useState<SupplierDto[]>([]);

    const [warehouses, setWarehouses] =
        useState<WarehouseDto[]>([]);

    const [lineItems, setLineItems] =
        useState<LineItem[]>([
            {
                id: 1,
                sku: "",
                description: "",
                quantity: "",
                unitPrice: "",
            },
        ]);

    /*
     * ----------------------------------------------------
     * Load suppliers and warehouses
     * ----------------------------------------------------
     */

    useEffect(() => {
        const loadOptions = async () => {
            try {
                setLoadingOptions(true);

                setError("");

                const [
                    supplierData,
                    warehouseData,
                ] = await Promise.all([
                    getSuppliers(),
                    getWarehouses(),
                ]);

                setSuppliers(
                    supplierData.filter(
                        (item) =>
                            item.isActive &&
                            item.supplierStatus ===
                                "Active"
                    )
                );

                setWarehouses(
                    warehouseData.filter(
                        (item) => item.isActive
                    )
                );
            } catch (err) {
                console.error(err);

                setError(
                    "Failed to load suppliers and warehouses."
                );
            } finally {
                setLoadingOptions(false);
            }
        };

        loadOptions();
    }, []);

    /*
     * ----------------------------------------------------
     * Load existing PO data when editing
     * ----------------------------------------------------
     */

    useEffect(() => {
        if (!editOrder) {
            return;
        }

        /*
         * Supplier
         */
        setSupplier(
            editOrder.supplierId.toString()
        );

        /*
         * Warehouse / Site
         *
         * Purchase orders store siteId while the UI
         * uses the warehouse ID.
         *
         * We first try to find a warehouse whose
         * siteId matches the PO siteId.
         */
        const matchingWarehouse =
            warehouses.find(
                (item) =>
                    item.siteId ===
                    editOrder.siteId
            );

        if (matchingWarehouse) {
            setWarehouse(
                matchingWarehouse.warehouseId.toString()
            );
        }

        /*
         * Expected delivery date
         */
        setExpectedDeliveryDate(
            editOrder.expectedDate
                ? editOrder.expectedDate.split("T")[0]
                : ""
        );

        /*
         * Existing line items
         */
        if (editItems.length > 0) {
            setLineItems(
                editItems.map(
                    (item, index) => ({
                        id:
                            item.purchaseOrderItemId ||
                            index + 1,

                        sku: item.sku,

                        description:
                            item.productName,

                        quantity:
                            item.orderedQuantity.toString(),

                        unitPrice:
                            item.unitPrice.toString(),
                    })
                )
            );
        } else {
            setLineItems([
                {
                    id: 1,
                    sku: "",
                    description: "",
                    quantity: "",
                    unitPrice: "",
                },
            ]);
        }
    }, [
        editOrder,
        editItems,
        warehouses,
    ]);

    /*
     * ----------------------------------------------------
     * Add line
     * ----------------------------------------------------
     */

    const addLine = () => {
        setLineItems((current) => [
            ...current,

            {
                id: Date.now(),

                sku: "",

                description: "",

                quantity: "",

                unitPrice: "",
            },
        ]);
    };

    /*
     * ----------------------------------------------------
     * Remove line
     * ----------------------------------------------------
     */

    const removeLine = (id: number) => {
        setLineItems((current) =>
            current.filter(
                (item) => item.id !== id
            )
        );
    };

    /*
     * ----------------------------------------------------
     * Update line
     * ----------------------------------------------------
     */

    const updateLine = async (
        id: number,
        field: keyof LineItem,
        value: string
    ) => {
        setLineItems((items) =>
            items.map((item) =>
                item.id === id
                    ? {
                          ...item,
                          [field]: value,
                      }
                    : item
            )
        );

        /*
         * Automatically find product when SKU
         * is entered.
         */
        if (field === "sku") {
            const sku = value.trim();

            if (!sku) {
                setLineItems((items) =>
                    items.map((item) =>
                        item.id === id
                            ? {
                                  ...item,
                                  description: "",
                              }
                            : item
                    )
                );

                return;
            }

            try {
                const product =
                    await getProductBySku(
                        sku
                    );

                setLineItems((items) =>
                    items.map((item) =>
                        item.id === id
                            ? {
                                  ...item,

                                  description:
                                      product.name,
                              }
                            : item
                    )
                );
            } catch {
                /*
                 * SKU doesn't exist.
                 *
                 * Clear the description so the
                 * user can enter a new product name.
                 */
                setLineItems((items) =>
                    items.map((item) =>
                        item.id === id
                            ? {
                                  ...item,

                                  description: "",
                              }
                            : item
                    )
                );
            }
        }
    };

    /*
     * ----------------------------------------------------
     * Get line total
     * ----------------------------------------------------
     */

    const getLineTotal = (
        item: LineItem
    ) => {
        const quantity =
            Number(item.quantity) || 0;

        const unitPrice =
            Number(item.unitPrice) || 0;

        return quantity * unitPrice;
    };

    /*
     * ----------------------------------------------------
     * Order total
     * ----------------------------------------------------
     */

    const orderTotal = useMemo(() => {
        return lineItems.reduce(
            (total, item) =>
                total +
                getLineTotal(item),

            0
        );
    }, [lineItems]);

    /*
     * ----------------------------------------------------
     * Currency
     * ----------------------------------------------------
     */

    const formatCurrency = (
        value: number
    ) =>
        new Intl.NumberFormat(
            "en-US",
            {
                style: "currency",

                currency: "EGP",

                currencyDisplay:
                    "narrowSymbol",

                maximumFractionDigits: 2,
            }
        )
            .format(value)
            .replace(
                "EGP",
                "E£"
            );

    /*
     * ----------------------------------------------------
     * Generate PO number
     * ----------------------------------------------------
     *
     * Only used when creating a NEW PO.
     *
     * Edit mode keeps the existing PO number.
     */

    const generatePONumber = () => {
        const timestamp =
            Date.now();

        return `PO-${timestamp}`;
    };

    /*
     * ----------------------------------------------------
     * Validate form
     * ----------------------------------------------------
     */

    const validateForm = () => {
        if (!supplier) {
            return "Please select a supplier.";
        }

        if (!warehouse) {
            return "Please select a warehouse.";
        }

        if (lineItems.length === 0) {
            return "Please add at least one line item.";
        }

        for (const item of lineItems) {
            if (!item.sku.trim()) {
                return "SKU is required for every line.";
            }

            if (!item.description.trim()) {
                return (
                    "Product description is required for every line."
                );
            }

            const quantity =
                Number(item.quantity);

            if (
                !item.quantity ||
                !Number.isFinite(quantity) ||
                quantity <= 0
            ) {
                return "Quantity must be greater than 0.";
            }

            const unitPrice =
                Number(item.unitPrice);

            if (
                !item.unitPrice ||
                !Number.isFinite(unitPrice) ||
                unitPrice < 0
            ) {
                return "Unit price must be 0 or greater.";
            }
        }

        return "";
    };

    /*
     * ----------------------------------------------------
     * Resolve product
     * ----------------------------------------------------
     *
     * Existing SKU:
     *      use existing product.
     *
     * New SKU:
     *      create product.
     */

    const resolveProduct = async (
        item: LineItem
    ) => {
        const sku =
            item.sku.trim();

        const productName =
            item.description.trim();

        if (!sku) {
            throw new Error(
                "SKU is required."
            );
        }

        if (!productName) {
            throw new Error(
                `Product name is required for SKU "${sku}".`
            );
        }

        try {
            const product =
                await getProductBySku(
                    sku
                );

            return product;
        } catch (err: any) {
            const status =
                err?.status ??
                err?.response?.status;

            /*
             * SKU doesn't exist.
             *
             * Create a new product.
             */
            if (status === 404) {
                return await createProduct(
                    {
                        sku,

                        barcode: null,

                        qrValue: null,

                        name: productName,

                        unitPrice:
                            Number(
                                item.unitPrice
                            ),

                        minimumStock: 0,

                        description:
                            productName,
                    }
                );
            }

            throw err;
        }
    };

    /*
     * ----------------------------------------------------
     * Handle SAVE
     * ----------------------------------------------------
     */

    const handleSave = async (
        saveAsDraft: boolean
    ) => {
        setError("");

        setSuccess("");

        const validationError =
            validateForm();

        if (validationError) {
            setError(
                validationError
            );

            return;
        }

        const selectedSupplier =
            suppliers.find(
                (item) =>
                    item.supplierId.toString() ===
                    supplier
            );

        const selectedWarehouse =
            warehouses.find(
                (item) =>
                    item.warehouseId.toString() ===
                    warehouse
            );

        if (!selectedSupplier) {
            setError(
                "Selected supplier was not found."
            );

            return;
        }

        if (!selectedWarehouse) {
            setError(
                "Selected warehouse was not found."
            );

            return;
        }

        try {
            setLoading(true);

            /*
             * ------------------------------------------------
             * Resolve all products first
             * ------------------------------------------------
             */

            const resolvedProducts: {
                item: LineItem;
                product: any;
            }[] = [];

            for (const item of lineItems) {
                const product =
                    await resolveProduct(
                        item
                    );

                resolvedProducts.push({
                    item,
                    product,
                });
            }

            /*
             * =================================================
             * EDIT EXISTING PO
             * =================================================
             */

            if (editOrder) {
                /*
                 * Update PO header.
                 *
                 * IMPORTANT:
                 * We intentionally do NOT send poNumber.
                 *
                 * The existing PO number remains unchanged.
                 */
                await updatePurchaseOrder(
                    editOrder.purchaseOrderId,
                    {
                        supplierId:
                            selectedSupplier.supplierId,

                        siteId:
                            selectedWarehouse.siteId,

                        orderDate:
                            editOrder.orderDate,

                        expectedDate:
                            expectedDeliveryDate ||
                            null,
                    }
                );

                /*
                 * Existing item IDs.
                 */
                const existingItemIds =
                    new Set(
                        editItems.map(
                            (item) =>
                                item.purchaseOrderItemId
                        )
                    );

                /*
                 * IDs that still exist after
                 * the edit.
                 */
                const remainingItemIds =
                    new Set<number>();

                /*
                 * ------------------------------------------------
                 * Update existing items / add new items
                 * ------------------------------------------------
                 */

                for (const {
                    item,
                    product,
                } of resolvedProducts) {
                    /*
                     * Existing PO item
                     */
                    if (
                        existingItemIds.has(
                            item.id
                        )
                    ) {
                        await updatePurchaseOrderItem(
                            editOrder.purchaseOrderId,

                            item.id,

                            {
                                productId:
                                    product.productId,

                                orderedQuantity:
                                    Number(
                                        item.quantity
                                    ),

                                unitPrice:
                                    Number(
                                        item.unitPrice
                                    ),
                            }
                        );

                        remainingItemIds.add(
                            item.id
                        );
                    } else {
                        /*
                         * New line added while
                         * editing.
                         */
                        await addPurchaseOrderItem(
                            editOrder.purchaseOrderId,

                            {
                                productId:
                                    product.productId,

                                orderedQuantity:
                                    Number(
                                        item.quantity
                                    ),

                                unitPrice:
                                    Number(
                                        item.unitPrice
                                    ),
                            }
                        );
                    }
                }

                /*
                 * ------------------------------------------------
                 * Delete removed items
                 * ------------------------------------------------
                 */

                for (const existingItem of editItems) {
                    if (
                        !remainingItemIds.has(
                            existingItem.purchaseOrderItemId
                        )
                    ) {
                        await deletePurchaseOrderItem(
                            editOrder.purchaseOrderId,

                            existingItem.purchaseOrderItemId
                        );
                    }
                }

                /*
                 * ------------------------------------------------
                 * Submit if requested
                 * ------------------------------------------------
                 */

                if (!saveAsDraft) {
                    await submitPurchaseOrder(
                        editOrder.purchaseOrderId
                    );

                    setSuccess(
                        "Purchase order updated and submitted for approval successfully."
                    );
                } else {
                    setSuccess(
                        "Purchase order updated successfully."
                    );
                }

                /*
                 * Close after success.
                 */
                if (onClose) {
                    setTimeout(() => {
                        onClose();
                    }, 700);
                }

                return;
            }

            /*
             * =================================================
             * CREATE NEW PO
             * =================================================
             */

            const purchaseOrder =
                await createPurchaseOrder(
                    {
                        poNumber:
                            generatePONumber(),

                        supplierId:
                            selectedSupplier.supplierId,

                        siteId:
                            selectedWarehouse.siteId,

                        orderDate:
                            new Date().toISOString(),

                        expectedDate:
                            expectedDeliveryDate ||
                            null,
                    }
                );

            /*
             * ------------------------------------------------
             * Add PO items
             * ------------------------------------------------
             */

            for (const {
                item,
                product,
            } of resolvedProducts) {
                await addPurchaseOrderItem(
                    purchaseOrder.purchaseOrderId,

                    {
                        productId:
                            product.productId,

                        orderedQuantity:
                            Number(
                                item.quantity
                            ),

                        unitPrice:
                            Number(
                                item.unitPrice
                            ),
                    }
                );
            }

            /*
             * ------------------------------------------------
             * Submit new PO if requested
             * ------------------------------------------------
             */

            if (!saveAsDraft) {
                await submitPurchaseOrder(
                    purchaseOrder.purchaseOrderId
                );

                setSuccess(
                    "Purchase order submitted for approval successfully."
                );
            } else {
                setSuccess(
                    "Purchase order saved as draft successfully."
                );
            }

            /*
             * Close after successful creation.
             */
            if (onClose) {
                setTimeout(() => {
                    onClose();
                }, 700);
            }
        } catch (err: any) {
            console.error(
                "Purchase order error:",
                err
            );

            const apiMessage =
                err?.message ||
                err?.response?.data?.message;

            setError(
                apiMessage ||
                    "Failed to save purchase order."
            );
        } finally {
            setLoading(false);
        }
    };

    /*
     * ----------------------------------------------------
     * Render
     * ----------------------------------------------------
     */

    return (
        <div
            style={{
                display: "flex",

                flexDirection:
                    "column",

                paddingBlock:
                    "var(--space-5)",

                height: "fit-content",

                border:
                    "var(--border-default)",

                borderColor:
                    "var(--light-grey)",

                color:
                    "var(--midnight-blue)",

                width:
                    "var(--card-width-xl)",

                borderRadius:
                    "var(--radius-lg)",

                gap:
                    "var(--space-5)",

                backgroundColor:
                    "var(--beige)",
            }}
        >
            {/* Header */}

            <div
                style={{
                    display: "flex",

                    alignItems:
                        "center",

                    justifyContent:
                        "space-between",

                    paddingInline:
                        "var(--space-5)",
                }}
            >
                <p className="page-title">
                    {isEditMode
                        ? editOrder?.poNumber
                        : "New Purchase Order"}
                </p>

                <button
                    type="button"
                    onClick={onClose}
                    disabled={loading}
                    className="page-title"
                    style={{
                        border: "none",

                        background:
                            "transparent",

                        cursor: loading
                            ? "not-allowed"
                            : "pointer",

                        color:
                            "var(--midnight-blue)",

                        padding: 0,

                        opacity: loading
                            ? 0.5
                            : 1,
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

            {/* Content */}

            <div
                className="column-container"
                style={{
                    paddingInline:
                        "var(--space-3)",
                }}
            >
                {/* Order Details */}

                <div
                    className="row-container"
                    style={{
                        width: "100%",
                    }}
                >
                    <Input
                        label="Supplier"
                        placeholder={
                            loadingOptions
                                ? "Loading suppliers..."
                                : "Select Supplier..."
                        }
                        options={suppliers.map(
                            (item) => ({
                                label:
                                    item.name,

                                value:
                                    item.supplierId.toString(),
                            })
                        )}
                        value={
                            supplier
                        }
                        onChange={
                            setSupplier
                        }
                        disabled={
                            loading ||
                            loadingOptions
                        }
                    />

                    <Input
                        label="Expected Delivery Date"
                        placeholder="dd/mm/yyyy"
                        type="date"
                        value={
                            expectedDeliveryDate
                        }
                        onChange={
                            setExpectedDeliveryDate
                        }
                        disabled={
                            loading
                        }
                    />
                </div>

                <div
                    className="row-container"
                    style={{
                        width: "100%",
                    }}
                >
                    <Input
                        label="Deliver to (Warehouse)"
                        placeholder={
                            loadingOptions
                                ? "Loading warehouses..."
                                : "Select Warehouse..."
                        }
                        options={warehouses.map(
                            (item) => ({
                                label: `${item.name} (${item.code})`,

                                value:
                                    item.warehouseId.toString(),
                            })
                        )}
                        value={
                            warehouse
                        }
                        onChange={
                            setWarehouse
                        }
                        disabled={
                            loading ||
                            loadingOptions
                        }
                    />

                    <Input
                        label="Notes"
                        optional
                        placeholder="Optional Notes..."
                        value={notes}
                        onChange={
                            setNotes
                        }
                        disabled={
                            loading
                        }
                    />
                </div>

                {/* Line Items */}

                <div
                    className="column-container"
                    style={{
                        gap:
                            "var(--space-1)",
                    }}
                >
                    <div
                        className="row-container"
                        style={{
                            width:
                                "100%",

                            justifyContent:
                                "space-between",

                            alignItems:
                                "center",
                        }}
                    >
                        <p className="body-title">
                            Line Items
                        </p>

                        <Button
                            size="sm"
                            onClick={
                                addLine
                            }
                            disabled={
                                loading
                            }
                        >
                            Add Line
                        </Button>
                    </div>

                    <div
                        className="card"
                        style={{
                            padding: 0,

                            overflow:
                                "hidden",

                            width:
                                "100%",
                        }}
                    >
                        <div
                            style={{
                                maxHeight:
                                    "220px",

                                overflowY:
                                    "auto",
                            }}
                        >
                            <table
                                style={{
                                    width:
                                        "100%",

                                    borderCollapse:
                                        "collapse",
                                }}
                            >
                                <thead
                                    style={{
                                        position:
                                            "sticky",

                                        top: 0,

                                        zIndex: 2,

                                        backgroundColor:
                                            "var(--beige)",
                                    }}
                                >
                                    <tr
                                        style={{
                                            height:
                                                "40px",

                                            borderBottom:
                                                "var(--border-default)",
                                        }}
                                    >
                                        <th>
                                            SKU
                                        </th>

                                        <th>
                                            Product Description
                                        </th>

                                        <th>
                                            QTY
                                        </th>

                                        <th>
                                            Unit Price
                                        </th>

                                        <th>
                                            Total
                                        </th>

                                        <th></th>
                                    </tr>
                                </thead>

                                <tbody>
                                    {lineItems.map(
                                        (
                                            item
                                        ) => (
                                            <tr
                                                key={
                                                    item.id
                                                }
                                                style={{
                                                    height:
                                                        "45px",

                                                    borderBottom:
                                                        "var(--border-default)",
                                                }}
                                            >
                                                <td
                                                    style={{
                                                        padding:
                                                            "var(--space-2)",
                                                    }}
                                                >
                                                    <Input
                                                        label=""
                                                        placeholder="SKU"
                                                        maxWidth
                                                        value={
                                                            item.sku
                                                        }
                                                        onChange={(
                                                            value
                                                        ) =>
                                                            updateLine(
                                                                item.id,
                                                                "sku",
                                                                value
                                                            )
                                                        }
                                                        disabled={
                                                            loading
                                                        }
                                                    />
                                                </td>

                                                <td
                                                    style={{
                                                        padding:
                                                            "var(--space-2)",
                                                    }}
                                                >
                                                    <Input
                                                        label=""
                                                        placeholder="Product Name"
                                                        maxWidth
                                                        value={
                                                            item.description
                                                        }
                                                        onChange={(
                                                            value
                                                        ) =>
                                                            updateLine(
                                                                item.id,
                                                                "description",
                                                                value
                                                            )
                                                        }
                                                        disabled={
                                                            loading
                                                        }
                                                    />
                                                </td>

                                                <td
                                                    style={{
                                                        padding:
                                                            "var(--space-2)",

                                                        width:
                                                            "100px",
                                                    }}
                                                >
                                                    <Input
                                                        label=""
                                                        min={
                                                            0
                                                        }
                                                        placeholder="0"
                                                        type="number"
                                                        maxWidth
                                                        value={
                                                            item.quantity
                                                        }
                                                        onChange={(
                                                            value
                                                        ) =>
                                                            updateLine(
                                                                item.id,
                                                                "quantity",
                                                                value
                                                            )
                                                        }
                                                        disabled={
                                                            loading
                                                        }
                                                    />
                                                </td>

                                                <td
                                                    style={{
                                                        padding:
                                                            "var(--space-2)",

                                                        width:
                                                            "140px",
                                                    }}
                                                >
                                                    <Input
                                                        label=""
                                                        min={
                                                            0
                                                        }
                                                        placeholder="0.00"
                                                        type="number"
                                                        maxWidth
                                                        value={
                                                            item.unitPrice
                                                        }
                                                        onChange={(
                                                            value
                                                        ) =>
                                                            updateLine(
                                                                item.id,
                                                                "unitPrice",
                                                                value
                                                            )
                                                        }
                                                        disabled={
                                                            loading
                                                        }
                                                    />
                                                </td>

                                                <td
                                                    style={{
                                                        padding:
                                                            "var(--space-2)",

                                                        whiteSpace:
                                                            "nowrap",

                                                        fontWeight:
                                                            500,
                                                    }}
                                                >
                                                    {formatCurrency(
                                                        getLineTotal(
                                                            item
                                                        )
                                                    )}
                                                </td>

                                                <td
                                                    style={{
                                                        padding:
                                                            "var(--space-2)",

                                                        width:
                                                            "40px",
                                                    }}
                                                >
                                                    {lineItems.length >
                                                        1 && (
                                                        <button
                                                            type="button"
                                                            onClick={() =>
                                                                removeLine(
                                                                    item.id
                                                                )
                                                            }
                                                            disabled={
                                                                loading
                                                            }
                                                            style={{
                                                                border:
                                                                    "none",

                                                                background:
                                                                    "transparent",

                                                                color:
                                                                    "var(--red)",

                                                                cursor:
                                                                    loading
                                                                        ? "not-allowed"
                                                                        : "pointer",

                                                                fontSize:
                                                                    "18px",

                                                                opacity:
                                                                    loading
                                                                        ? 0.5
                                                                        : 1,
                                                            }}
                                                        >
                                                            ×
                                                        </button>
                                                    )}
                                                </td>
                                            </tr>
                                        )
                                    )}
                                </tbody>
                            </table>
                        </div>

                        {/* Total */}

                        <div
                            style={{
                                display:
                                    "flex",

                                justifyContent:
                                    "flex-end",

                                alignItems:
                                    "center",

                                gap:
                                    "var(--space-5)",

                                padding:
                                    "var(--space-3)",

                                borderTop:
                                    "var(--border-default)",
                            }}
                        >
                            <span
                                style={{
                                    fontWeight:
                                        600,
                                }}
                            >
                                Order Total
                            </span>

                            <span
                                style={{
                                    fontWeight:
                                        600,
                                }}
                            >
                                {formatCurrency(
                                    orderTotal
                                )}
                            </span>
                        </div>
                    </div>
                </div>

                {/* Error */}

                {error && (
                    <p
                        style={{
                            color:
                                "var(--red)",
                        }}
                    >
                        {error}
                    </p>
                )}

                {/* Success */}

                {success && (
                    <p
                        style={{
                            color:
                                "var(--green)",
                        }}
                    >
                        {success}
                    </p>
                )}

                {/* Actions */}

                <div
                    className="row-container"
                    style={{
                        justifyContent:
                            "flex-end",

                        width:
                            "100%",
                    }}
                >
                    <Button
                        variant="outline"
                        onClick={
                            onClose
                        }
                        disabled={
                            loading
                        }
                        style={{
                            width:
                                "100%",
                        }}
                    >
                        Cancel
                    </Button>

                    {!isEditMode && 
                    <Button
                    onClick={() =>
                            handleSave(
                                true
                            )
                        }
                        disabled={
                            loading ||
                            loadingOptions
                        }
                        style={{
                            width:
                                "100%",
                        }}
                    >
                        {loading
                            ? "Saving..."
                            : "Save as Draft"}
                    </Button>
                    }

                    <Button
                        variant="secondary"
                        onClick={() =>
                            handleSave(
                                false
                            )
                        }
                        disabled={
                            loading ||
                            loadingOptions
                        }
                        style={{
                            width:
                                "100%",
                        }}
                    >
                        {loading
                            ? "Submitting..."
                            : "Submit for Approval"}
                    </Button>
                </div>
            </div>
        </div>
    );
}