"use client";

import { useState, useEffect } from "react";
import { useSearchParams, useRouter } from "next/navigation";

import ReceiveStepper from "../components/receive-stepper";
import Button from "@/components/button";

import useReceiving, {
    RECEIVING_STEPS,
} from "@/hooks/useReceiving";

import { createClient } from "@/lib/supabase/client";

export default function POLines() {
    const router = useRouter();
    const searchParams = useSearchParams();



    const [currentUser, setCurrentUser] = useState("");
    const [currentUserId, setCurrentUserId] = useState<string | null>(null);

    useEffect(() => {
        async function loadCurrentUser() {
            const supabase = createClient();

            const {
                data: { user },
            } = await supabase.auth.getUser();

            if (!user) {
                setCurrentUser("Unknown User");
                return;
            }

            setCurrentUserId(user.id);

            setCurrentUser(
                user.user_metadata?.name ||
                user.user_metadata?.full_name ||
                user.email ||
                "Current User"
            );
        }

        loadCurrentUser();
    }, []);

    /*
     * =========================================================
     * GET PO ID FROM URL
     *
     * /receiving/new/po-lines?poId=3
     *                         ↑
     *                       poId
     * =========================================================
     */
    const poIdParam = searchParams.get("poId");

    const poId = poIdParam
        ? Number(poIdParam)
        : null;

    const [scanValue, setScanValue] =
        useState("");

    const [
        creatingProduct,
        setCreatingProduct,
    ] = useState(false);

    /*
     * Invalid / missing PO ID
     */
    if (
        poId === null ||
        Number.isNaN(poId) ||
        poId <= 0
    ) {
        return (
            <div className="page-column">
                <p
                    className="body-title"
                    style={{
                        color: "var(--blood-red)",
                    }}
                >
                    Invalid purchase order ID.
                </p>
            </div>
        );
    }

    /*
     * =========================================================
     * RECEIVING HOOK
     * =========================================================
     */
    const {
        po,
        poItems,
        receipt,
        receiptItems,

        warehouses,
        categories,
        units,

        selectedWarehouseId,
        setSelectedWarehouseId,

        receivedBy,

        lines,
        selectedLine,
        selectedLineId,

        currentStep,
        setCurrentStep,

        loading,
        saving,

        error,
        scanError,

        scannedLines,
        completedLines,
        partialLines,
        allLinesSaved,

        selectLine,
        updateLine,

        scanProduct,
        createNewReceivingProduct,

        verifyProduct,

        saveCurrentLine,

        nextStep,
        previousStep,

        reload,
    } = useReceiving({ poId });

    /*
     * =========================================================
     * LOADING
     * =========================================================
     */
    if (loading) {
        return (
            <div className="page-column">
                <ReceiveStepper />

                <div
                    className="card"
                    style={{
                        width: "100%",
                    }}
                >
                    <p className="body-title">
                        Loading purchase order...
                    </p>
                </div>
            </div>
        );
    }

    /*
     * =========================================================
     * PO NOT FOUND
     * =========================================================
     */
    if (!po) {
        return (
            <div className="page-column">
                <ReceiveStepper />

                <div
                    className="card"
                    style={{
                        width: "100%",
                    }}
                >
                    <p
                        className="body-title"
                        style={{
                            color:
                                "var(--blood-red)",
                        }}
                    >
                        {error ||
                            "Purchase order could not be loaded."}
                    </p>
                </div>
            </div>
        );
    }

    /*
     * =========================================================
     * MAIN PAGE
     * =========================================================
     */

    return (
        <div className="page-column">
            <ReceiveStepper />

            {/* =====================================================
                CONTENT
            ====================================================== */}

            <div
                className="row-container"
                style={{
                    width: "100%",
                    alignItems: "stretch",
                }}
            >
                {/* =================================================
                    LEFT — PO LINES
                ================================================== */}

                <div
                    className="card column-container"
                    style={{
                        width:
                            "var(--sidedetails-width)",
                        paddingInline: 0,
                        flexShrink: 0,
                    }}
                >
                    {/* Header */}

                    <div
                        className="column-container"
                        style={{
                            paddingInline:
                                "var(--space-3)",
                        }}
                    >
                        <p className="nav-item">
                            PO Lines
                        </p>

                        <p
                            className="body-title"
                            style={{
                                color:
                                    "var(--grey)",
                            }}
                        >
                            {scannedLines} of{" "}
                            {lines.length}{" "}
                            scanned
                        </p>
                    </div>

                    {/* Lines */}

                    <div
                        style={{
                            width: "100%",
                            borderTop:
                                "var(--border-default)",
                        }}
                    >
                        {lines.map(
                            (line) => {
                                const isSelected =
                                    selectedLineId ===
                                    line.purchaseOrderItemId;

                                return (
                                    <div
                                        key={
                                            line.purchaseOrderItemId
                                        }
                                        onClick={() =>
                                            selectLine(
                                                line.purchaseOrderItemId
                                            )
                                        }
                                        style={{
                                            width: "100%",
                                            padding:
                                                "var(--space-3)",
                                            cursor:
                                                "pointer",
                                            backgroundColor:
                                                isSelected
                                                    ? "var(--beige)"
                                                    : "transparent",
                                            borderBottom:
                                                "var(--border-default)",
                                        }}
                                    >
                                        <div
                                            className="column-container"
                                            style={{
                                                gap:
                                                    "var(--space-1)",
                                            }}
                                        >
                                            <p className="body-title">
                                                {
                                                    line.productName
                                                }
                                            </p>

                                            <p
                                                className="small-body"
                                                style={{
                                                    color:
                                                        "var(--grey)",
                                                }}
                                            >
                                                SKU:{" "}
                                                {
                                                    line.sku
                                                }
                                            </p>

                                            <div className="row-container">
                                                <p className="small-body">
                                                    Ordered:{" "}
                                                    {
                                                        line.orderedQuantity
                                                    }
                                                </p>

                                                <p className="small-body">
                                                    Remaining:{" "}
                                                    {
                                                        line.remainingQuantity
                                                    }
                                                </p>
                                            </div>

                                            {line.isSaved && (
                                                <p
                                                    className="small-body"
                                                    style={{
                                                        color:
                                                            line.receivingStatus ===
                                                                "Complete"
                                                                ? "var(--dark-green)"
                                                                : "var(--grey)",
                                                    }}
                                                >
                                                    {
                                                        line.receivingStatus
                                                    }
                                                </p>
                                            )}
                                        </div>
                                    </div>
                                );
                            }
                        )}
                    </div>

                    {/* Review */}

                    <div
                        style={{
                            marginTop: "auto",
                            padding:
                                "var(--space-3)",
                        }}
                    >
                        <Button
                            style={{
                                width: "100%",
                            }}
                            disabled={
                                scannedLines ===
                                0
                            }
                            onClick={() =>
                                router.push(
                                    `/receiving/confirm-receipt?receiptId=${receipt?.receiptId}`
                                )
                            }
                        >
                            Review{" "}
                            {scannedLines} Lines
                        </Button>
                    </div>
                </div>

                {/* =================================================
                    RIGHT — RECEIVING STEPS
                ================================================== */}

                <div
                    className="card column-container"
                    style={{
                        width: "100%",
                        minWidth: 0,
                    }}
                >
                    {/* =================================================
                        STEP NAVIGATION
                    ================================================== */}

                    <div
                        className="row-container"
                        style={{
                            width: "100%",
                            flexWrap: "wrap",
                        }}
                    >
                        {RECEIVING_STEPS.map(
                            (
                                step,
                                index
                            ) => {
                                const isActive =
                                    currentStep ===
                                    index;

                                return (
                                    <Button
                                        key={
                                            step.title
                                        }
                                        size="sm"
                                        variant={
                                            isActive
                                                ? "secondary"
                                                : "beige"
                                        }
                                        onClick={() =>
                                            setCurrentStep(
                                                index
                                            )
                                        }
                                        disabled={
                                            !selectedLine ||
                                            saving
                                        }
                                    >
                                        {
                                            step.title
                                        }
                                    </Button>
                                );
                            }
                        )}
                    </div>

                    {/* =================================================
                        CURRENT STEP HEADER
                    ================================================== */}

                    <div
                        className="column-container"
                        style={{
                            gap:
                                "var(--space-1)",
                        }}
                    >
                        <p className="nav-item">
                            {
                                RECEIVING_STEPS[
                                    currentStep
                                ].title
                            }
                        </p>

                        <p
                            className="body-title"
                            style={{
                                color:
                                    "var(--grey)",
                            }}
                        >
                            {
                                RECEIVING_STEPS[
                                    currentStep
                                ].subtitle
                            }
                        </p>
                    </div>

                    {/* =================================================
                        ERROR
                    ================================================== */}

                    {(error ||
                        scanError) && (
                            <div
                                className="card"
                                style={{
                                    width: "100%",
                                    border:
                                        "1px solid var(--blood-red)",
                                }}
                            >
                                <p
                                    className="small-body"
                                    style={{
                                        color:
                                            "var(--blood-red)",
                                    }}
                                >
                                    {error ||
                                        scanError}
                                </p>
                            </div>
                        )}

                    {/* =================================================
                        STEP CONTENT
                    ================================================== */}

                    <div
                        style={{
                            width: "100%",
                            flex: 1,
                        }}
                    >
                        {!selectedLine ? (
                            <div
                                className="column-container"
                                style={{
                                    alignItems:
                                        "center",
                                    justifyContent:
                                        "center",
                                    minHeight:
                                        "300px",
                                }}
                            >
                                <p className="body-title">
                                    Select a PO
                                    line to
                                    start
                                    receiving.
                                </p>
                            </div>
                        ) : (
                            <>
                                {/* =====================================
                                    STEP 1
                                ====================================== */}

                                {currentStep ===
                                    0 && (
                                        <div
                                            className="column-container"
                                            style={{
                                                width:
                                                    "100%",
                                                gap:
                                                    "var(--space-3)",
                                            }}
                                        >
                                            <div
                                                className="card"
                                                style={{
                                                    width:
                                                        "100%",
                                                }}
                                            >
                                                <p className="body-title">
                                                    {
                                                        selectedLine.productName
                                                    }
                                                </p>

                                                <p
                                                    className="small-body"
                                                    style={{
                                                        color:
                                                            "var(--grey)",
                                                    }}
                                                >
                                                    SKU:{" "}
                                                    {
                                                        selectedLine.sku
                                                    }
                                                </p>
                                            </div>

                                            <div
                                                className="card row-container"
                                                style={{
                                                    width:
                                                        "100%",
                                                    alignItems:
                                                        "center",
                                                }}
                                            >
                                                <input
                                                    type="text"
                                                    value={
                                                        scanValue
                                                    }
                                                    onChange={(
                                                        e
                                                    ) =>
                                                        setScanValue(
                                                            e
                                                                .target
                                                                .value
                                                        )
                                                    }
                                                    onKeyDown={(
                                                        e
                                                    ) => {
                                                        if (
                                                            e.key ===
                                                            "Enter"
                                                        ) {
                                                            scanProduct(
                                                                scanValue
                                                            );

                                                            setScanValue(
                                                                ""
                                                            );
                                                        }
                                                    }}
                                                    placeholder="Scan barcode or enter SKU"
                                                    className="body-title"
                                                    style={{
                                                        width:
                                                            "100%",
                                                        border:
                                                            "none",
                                                        outline:
                                                            "none",
                                                        background:
                                                            "transparent",
                                                    }}
                                                />

                                                <Button
                                                    size="sm"
                                                    variant="secondary"
                                                    onClick={() => {
                                                        scanProduct(
                                                            scanValue
                                                        );

                                                        setScanValue(
                                                            ""
                                                        );
                                                    }}
                                                    disabled={
                                                        !scanValue.trim() ||
                                                        saving
                                                    }
                                                >
                                                    Verify
                                                </Button>
                                            </div>
                                        </div>
                                    )}

                                {/* =====================================
    STEP 2
====================================== */}

                                {currentStep === 1 && (
                                    <div
                                        className="column-container"
                                        style={{
                                            width: "100%",
                                            gap: "var(--space-3)",
                                        }}
                                    >
                                        <div
                                            className="card column-container"
                                            style={{
                                                width: "100%",
                                                gap: "var(--space-3)",
                                            }}
                                        >
                                            {/* Product Name */}

                                            <div
                                                className="row-container"
                                                style={{
                                                    width: "100%",
                                                    justifyContent: "space-between",
                                                    alignItems: "center",
                                                }}
                                            >
                                                <p className="body-title">
                                                    Product Name
                                                </p>

                                                <input
                                                    type="text"
                                                    value={
                                                        selectedLine.productName ?? ""
                                                    }
                                                    onChange={(e) =>
                                                        updateLine({
                                                            productName:
                                                                e.target.value,
                                                        })
                                                    }
                                                    className="body-title"
                                                    style={{
                                                        width: "60%",
                                                        padding:
                                                            "var(--space-1)",
                                                        border:
                                                            "var(--border-default)",
                                                        borderRadius:
                                                            "var(--radius-md)",
                                                        background:
                                                            "var(--beige)",
                                                        outline: "none",
                                                        textAlign: "left",
                                                    }}
                                                />
                                            </div>

                                            {/* SKU */}

                                            <div
                                                className="row-container"
                                                style={{
                                                    width: "100%",
                                                    justifyContent:
                                                        "space-between",
                                                    alignItems: "center",
                                                }}
                                            >
                                                <p className="body-title">
                                                    SKU
                                                </p>

                                                <input
                                                    type="text"
                                                    value={
                                                        selectedLine.sku ?? ""
                                                    }
                                                    onChange={(e) =>
                                                        updateLine({
                                                            sku: e.target.value,
                                                        })
                                                    }
                                                    className="body-title"
                                                    style={{
                                                        width: "60%",
                                                        padding:
                                                            "var(--space-1)",
                                                        border:
                                                            "var(--border-default)",
                                                        borderRadius:
                                                            "var(--radius-md)",
                                                        background:
                                                            "var(--beige)",
                                                        outline: "none",
                                                        textAlign: "left",
                                                    }}
                                                />
                                            </div>

                                            {/* Barcode */}

                                            <div
                                                className="row-container"
                                                style={{
                                                    width: "100%",
                                                    justifyContent:
                                                        "space-between",
                                                    alignItems: "center",
                                                }}
                                            >
                                                <p className="body-title">
                                                    Barcode
                                                </p>

                                                <input
                                                    type="text"
                                                    value={
                                                        selectedLine.barcode ?? ""
                                                    }
                                                    onChange={(e) =>
                                                        updateLine({
                                                            barcode:
                                                                e.target.value ||
                                                                null,
                                                        })
                                                    }
                                                    className="body-title"
                                                    style={{
                                                        width: "60%",
                                                        padding:
                                                            "var(--space-1)",
                                                        border:
                                                            "var(--border-default)",
                                                        borderRadius:
                                                            "var(--radius-md)",
                                                        background:
                                                            "var(--beige)",
                                                        outline: "none",
                                                        textAlign: "left",
                                                    }}
                                                />
                                            </div>

                                            {/* Category */}

                                            <div
                                                className="row-container"
                                                style={{
                                                    width: "100%",
                                                    justifyContent:
                                                        "space-between",
                                                    alignItems: "center",
                                                }}
                                            >
                                                <p className="body-title">
                                                    Category
                                                </p>

                                                <select
                                                    value={
                                                        selectedLine.categoryId ??
                                                        ""
                                                    }
                                                    onChange={(e) => {
                                                        const categoryId =
                                                            e.target.value
                                                                ? Number(
                                                                    e.target
                                                                        .value
                                                                )
                                                                : null;

                                                        const selectedCategory =
                                                            categories.find(
                                                                (category) =>
                                                                    category.categoryId ===
                                                                    categoryId
                                                            );

                                                        updateLine({
                                                            categoryId,
                                                            categoryName:
                                                                selectedCategory?.name ??
                                                                null,
                                                        });
                                                    }}
                                                    className="body-title"
                                                    style={{
                                                        width: "60%",
                                                        padding:
                                                            "var(--space-1)",
                                                        border:
                                                            "var(--border-default)",
                                                        borderRadius:
                                                            "var(--radius-md)",
                                                        background:
                                                            "var(--beige)",
                                                        outline: "none",
                                                    }}
                                                >
                                                    <option value="">
                                                        Select category
                                                    </option>

                                                    {categories.map(
                                                        (category) => (
                                                            <option
                                                                key={
                                                                    category.categoryId
                                                                }
                                                                value={
                                                                    category.categoryId
                                                                }
                                                            >
                                                                {category.name}
                                                            </option>
                                                        )
                                                    )}
                                                </select>
                                            </div>

                                            {/* Unit of Measure */}

                                            <div
                                                className="row-container"
                                                style={{
                                                    width: "100%",
                                                    justifyContent:
                                                        "space-between",
                                                    alignItems: "center",
                                                }}
                                            >
                                                <p className="body-title">
                                                    Unit of Measure
                                                </p>

                                                <select
                                                    value={
                                                        selectedLine.unitId ??
                                                        ""
                                                    }
                                                    onChange={(e) => {
                                                        const unitId =
                                                            e.target.value
                                                                ? Number(
                                                                    e.target
                                                                        .value
                                                                )
                                                                : null;

                                                        const selectedUnit =
                                                            units.find(
                                                                (unit) =>
                                                                    unit.unitId ===
                                                                    unitId
                                                            );

                                                        updateLine({
                                                            unitId,
                                                            unitName:
                                                                selectedUnit?.name ??
                                                                null,
                                                        });
                                                    }}
                                                    className="body-title"
                                                    style={{
                                                        width: "60%",
                                                        padding:
                                                            "var(--space-1)",
                                                        border:
                                                            "var(--border-default)",
                                                        borderRadius:
                                                            "var(--radius-md)",
                                                        background:
                                                            "var(--beige)",
                                                        outline: "none",
                                                    }}
                                                >
                                                    <option value="">
                                                        Select unit
                                                    </option>

                                                    {units.map((unit) => (
                                                        <option
                                                            key={
                                                                unit.unitId
                                                            }
                                                            value={
                                                                unit.unitId
                                                            }
                                                        >
                                                            {unit.name}
                                                        </option>
                                                    ))}
                                                </select>
                                            </div>

                                            {/* Minimum Stock */}

                                            <div
                                                className="row-container"
                                                style={{
                                                    width: "100%",
                                                    justifyContent:
                                                        "space-between",
                                                    alignItems: "center",
                                                }}
                                            >
                                                <p className="body-title">
                                                    Minimum Stock
                                                </p>

                                                <input
                                                    type="number"
                                                    min="0"
                                                    value={
                                                        selectedLine.minimumStock ??
                                                        0
                                                    }
                                                    onChange={(e) =>
                                                        updateLine({
                                                            minimumStock:
                                                                Math.max(
                                                                    0,
                                                                    Number(
                                                                        e.target
                                                                            .value
                                                                    )
                                                                ),
                                                        })
                                                    }
                                                    className="body-title"
                                                    style={{
                                                        width: "60%",
                                                        padding:
                                                            "var(--space-1)",
                                                        border:
                                                            "var(--border-default)",
                                                        borderRadius:
                                                            "var(--radius-md)",
                                                        background:
                                                            "var(--beige)",
                                                        outline: "none",
                                                        textAlign: "left",
                                                    }}
                                                />
                                            </div>

                                            {/* Warehouse */}

                                            <div
                                                className="row-container"
                                                style={{
                                                    width: "100%",
                                                    justifyContent:
                                                        "space-between",
                                                    alignItems: "center",
                                                }}
                                            >
                                                <p className="body-title">
                                                    Warehouse
                                                </p>

                                                <select
                                                    value={
                                                        selectedWarehouseId ??
                                                        ""
                                                    }
                                                    onChange={(e) =>
                                                        setSelectedWarehouseId(
                                                            e.target.value
                                                                ? Number(
                                                                    e.target
                                                                        .value
                                                                )
                                                                : null
                                                        )
                                                    }
                                                    className="body-title"
                                                    style={{
                                                        width: "60%",
                                                        padding:
                                                            "var(--space-1)",
                                                        border:
                                                            "var(--border-default)",
                                                        borderRadius:
                                                            "var(--radius-md)",
                                                        background:
                                                            "var(--beige)",
                                                        outline: "none",
                                                    }}
                                                >
                                                    <option value="">
                                                        Select warehouse
                                                    </option>

                                                    {warehouses.map(
                                                        (warehouse) => (
                                                            <option
                                                                key={
                                                                    warehouse.warehouseId
                                                                }
                                                                value={
                                                                    warehouse.warehouseId
                                                                }
                                                            >
                                                                {warehouse.name}
                                                            </option>
                                                        )
                                                    )}
                                                </select>
                                            </div>

                                            {/* Received By */}

                                            <div
                                                className="row-container"
                                                style={{
                                                    width: "100%",
                                                    justifyContent:
                                                        "space-between",
                                                    alignItems: "center",
                                                }}
                                            >
                                                <p className="body-title">
                                                    Received By
                                                </p>

                                                <input
                                                    type="text"
                                                    value={
                                                        currentUser ||
                                                        "Current User"
                                                    }
                                                    readOnly
                                                    className="body-title"
                                                    style={{
                                                        width: "60%",
                                                        padding:
                                                            "var(--space-1)",
                                                        border:
                                                            "var(--border-default)",
                                                        borderRadius:
                                                            "var(--radius-md)",
                                                        background:
                                                            "var(--light-grey)",
                                                        color: "var(--grey)",
                                                        outline: "none",
                                                        textAlign: "left",
                                                        cursor: "not-allowed",
                                                    }}
                                                />
                                            </div>
                                        </div>

                                        {/* Actions */}

                                        <div
                                            className="row-container"
                                            style={{
                                                gap: "var(--space-2)",
                                            }}
                                        >
                                            {selectedLine.isExistingProduct ? (
                                                <Button
                                                    size="sm"
                                                    variant="outline"
                                                    onClick={() => {
                                                        setCurrentStep(0);
                                                    }}
                                                >
                                                    Re-Scan
                                                </Button>
                                            ) : (
                                                <Button
                                                    size="sm"
                                                    variant="secondary"
                                                    disabled={
                                                        creatingProduct ||
                                                        saving
                                                    }
                                                    onClick={async () => {
                                                        setCreatingProduct(
                                                            true
                                                        );

                                                        await createNewReceivingProduct(
                                                            {
                                                                sku:
                                                                    selectedLine.sku,

                                                                barcode:
                                                                    selectedLine.barcode,

                                                                qrValue: null,

                                                                name:
                                                                    selectedLine.productName,

                                                                categoryId:
                                                                    selectedLine.categoryId,

                                                                unitId:
                                                                    selectedLine.unitId,

                                                                unitPrice:
                                                                    selectedLine.unitPrice,

                                                                minimumStock:
                                                                    selectedLine.minimumStock,

                                                                description:
                                                                    selectedLine.description,
                                                            }
                                                        );

                                                        setCreatingProduct(
                                                            false
                                                        );
                                                    }}
                                                >
                                                    {creatingProduct
                                                        ? "Creating..."
                                                        : "Create Product"}
                                                </Button>
                                            )}

                                            <Button
                                                size="sm"
                                                variant="secondary"
                                                onClick={verifyProduct}
                                                disabled={saving}
                                            >
                                                Confirm
                                            </Button>
                                        </div>
                                    </div>
                                )}

                                {/* =====================================
                                    STEP 3 — QUANTITY
                                ====================================== */}

                                {currentStep ===
                                    2 && (
                                        <div
                                            className="column-container"
                                            style={{
                                                gap:
                                                    "var(--space-3)",
                                            }}
                                        >
                                            <div className="row-container">
                                                <input
                                                    type="number"
                                                    min="0"
                                                    max={
                                                        selectedLine.remainingQuantity
                                                    }
                                                    value={
                                                        selectedLine.receivedQuantity ||
                                                        ""
                                                    }
                                                    onChange={(
                                                        e
                                                    ) => {
                                                        const value =
                                                            Math.max(
                                                                0,
                                                                Number(
                                                                    e
                                                                        .target
                                                                        .value
                                                                )
                                                            );

                                                        updateLine(
                                                            {
                                                                receivedQuantity:
                                                                    Math.min(
                                                                        value,
                                                                        selectedLine.remainingQuantity
                                                                    ),
                                                            }
                                                        );
                                                    }}
                                                    className="nav-item"
                                                    style={{
                                                        width:
                                                            "160px",
                                                        padding:
                                                            "var(--space-2)",
                                                        border:
                                                            "var(--border-default)",
                                                        borderRadius:
                                                            "var(--radius-md)",
                                                        background:
                                                            "var(--beige)",
                                                    }}
                                                />

                                                <p className="body-title">
                                                    Piece
                                                </p>
                                            </div>

                                            <p
                                                className="small-body"
                                                style={{
                                                    color:
                                                        "var(--grey)",
                                                }}
                                            >
                                                Remaining
                                                quantity:
                                                {" "}
                                                {
                                                    selectedLine.remainingQuantity
                                                }
                                            </p>

                                            {selectedLine.receivedQuantity ===
                                                selectedLine.remainingQuantity &&
                                                selectedLine.receivedQuantity >
                                                0 && (
                                                    <p
                                                        className="body-title"
                                                        style={{
                                                            color:
                                                                "var(--dark-green)",
                                                        }}
                                                    >
                                                        Full
                                                        receiving
                                                    </p>
                                                )}

                                            {selectedLine.receivedQuantity >
                                                0 &&
                                                selectedLine.receivedQuantity <
                                                selectedLine.remainingQuantity && (
                                                    <p
                                                        className="body-title"
                                                        style={{
                                                            color:
                                                                "var(--grey)",
                                                        }}
                                                    >
                                                        Partial
                                                        receiving
                                                    </p>
                                                )}
                                        </div>
                                    )}

                                {/* =====================================
                                    STEP 4 — BATCH / LOT
                                ====================================== */}

                                {currentStep ===
                                    3 && (
                                        <div
                                            className="column-container"
                                            style={{
                                                width:
                                                    "100%",
                                            }}
                                        >
                                            <input
                                                type="text"
                                                value={
                                                    selectedLine.batchNumber
                                                }
                                                onChange={(
                                                    e
                                                ) =>
                                                    updateLine(
                                                        {
                                                            batchNumber:
                                                                e
                                                                    .target
                                                                    .value,
                                                        }
                                                    )
                                                }
                                                placeholder="Enter batch / lot number"
                                                className="body-title"
                                                style={{
                                                    width:
                                                        "100%",
                                                    padding:
                                                        "var(--space-2)",
                                                    border:
                                                        "var(--border-default)",
                                                    borderRadius:
                                                        "var(--radius-md)",
                                                    background:
                                                        "var(--beige)",
                                                    outline:
                                                        "none",
                                                }}
                                            />
                                        </div>
                                    )}

                                {/* =====================================
                                    STEP 5 — EXPIRY DATE
                                ====================================== */}

                                {currentStep ===
                                    4 && (
                                        <div
                                            className="column-container"
                                            style={{
                                                width:
                                                    "100%",
                                            }}
                                        >
                                            <input
                                                type="date"
                                                value={
                                                    selectedLine.expiryDate
                                                }
                                                onChange={(
                                                    e
                                                ) =>
                                                    updateLine(
                                                        {
                                                            expiryDate:
                                                                e
                                                                    .target
                                                                    .value,
                                                        }
                                                    )
                                                }
                                                className="body-title"
                                                style={{
                                                    width:
                                                        "100%",
                                                    padding:
                                                        "var(--space-2)",
                                                    border:
                                                        "var(--border-default)",
                                                    borderRadius:
                                                        "var(--radius-md)",
                                                    background:
                                                        "var(--beige)",
                                                    outline:
                                                        "none",
                                                }}
                                            />
                                        </div>
                                    )}

                                {/* =====================================
                                    STEP 6 — UNIT PRICE
                                ====================================== */}

                                {currentStep ===
                                    5 && (
                                        <div
                                            className="column-container"
                                            style={{
                                                gap:
                                                    "var(--space-2)",
                                            }}
                                        >
                                            <div className="row-container">
                                                <input
                                                    type="number"
                                                    min="0"
                                                    step="0.01"
                                                    value={
                                                        selectedLine.unitPrice
                                                    }
                                                    onChange={(
                                                        e
                                                    ) =>
                                                        updateLine(
                                                            {
                                                                unitPrice:
                                                                    Math.max(
                                                                        0,
                                                                        Number(
                                                                            e
                                                                                .target
                                                                                .value
                                                                        )
                                                                    ),
                                                            }
                                                        )
                                                    }
                                                    className="nav-item"
                                                    style={{
                                                        width:
                                                            "160px",
                                                        padding:
                                                            "var(--space-2)",
                                                        border:
                                                            "var(--border-default)",
                                                        borderRadius:
                                                            "var(--radius-md)",
                                                        background:
                                                            "var(--beige)",
                                                    }}
                                                />

                                                <p className="body-title">
                                                    Per Piece
                                                </p>
                                            </div>
                                        </div>
                                    )}

                                {/* =====================================
                                    STEP 7 — DAMAGED QUANTITY
                                ====================================== */}

                                {currentStep ===
                                    6 && (
                                        <div
                                            className="column-container"
                                            style={{
                                                gap:
                                                    "var(--space-3)",
                                            }}
                                        >
                                            <div className="row-container">
                                                <input
                                                    type="number"
                                                    min="0"
                                                    max={
                                                        selectedLine.receivedQuantity
                                                    }
                                                    value={
                                                        selectedLine.damagedQuantity ||
                                                        ""
                                                    }
                                                    onChange={(
                                                        e
                                                    ) => {
                                                        const value =
                                                            Math.max(
                                                                0,
                                                                Number(
                                                                    e
                                                                        .target
                                                                        .value
                                                                )
                                                            );

                                                        updateLine(
                                                            {
                                                                damagedQuantity:
                                                                    Math.min(
                                                                        value,
                                                                        selectedLine.receivedQuantity
                                                                    ),
                                                            }
                                                        );
                                                    }}
                                                    className="nav-item"
                                                    style={{
                                                        width:
                                                            "160px",
                                                        padding:
                                                            "var(--space-2)",
                                                        border:
                                                            "var(--border-default)",
                                                        borderRadius:
                                                            "var(--radius-md)",
                                                        background:
                                                            "var(--beige)",
                                                    }}
                                                />

                                                <p className="body-title">
                                                    Piece
                                                </p>
                                            </div>

                                            {/* Summary */}

                                            <div className="row-container">
                                                <div
                                                    className="column-container"
                                                    style={{
                                                        width:
                                                            "110px",
                                                        height:
                                                            "110px",
                                                        alignItems:
                                                            "center",
                                                        justifyContent:
                                                            "center",
                                                        padding:
                                                            "var(--space-2)",
                                                        gap:
                                                            "var(--space-1)",
                                                        backgroundColor:
                                                            "var(--beige)",
                                                        borderRadius:
                                                            "var(--radius-md)",
                                                    }}
                                                >
                                                    <p className="small-body">
                                                        Received
                                                    </p>

                                                    <p className="nav-item">
                                                        {
                                                            selectedLine.receivedQuantity
                                                        }
                                                    </p>

                                                    <p className="small-body">
                                                        Units
                                                    </p>
                                                </div>

                                                <div
                                                    className="column-container"
                                                    style={{
                                                        width:
                                                            "110px",
                                                        height:
                                                            "110px",
                                                        alignItems:
                                                            "center",
                                                        justifyContent:
                                                            "center",
                                                        padding:
                                                            "var(--space-2)",
                                                        gap:
                                                            "var(--space-1)",
                                                        backgroundColor:
                                                            "var(--beige)",
                                                        borderRadius:
                                                            "var(--radius-md)",
                                                    }}
                                                >
                                                    <p className="small-body">
                                                        Damaged
                                                    </p>

                                                    <p
                                                        className="nav-item"
                                                        style={{
                                                            color:
                                                                "var(--blood-red)",
                                                        }}
                                                    >
                                                        {
                                                            selectedLine.damagedQuantity
                                                        }
                                                    </p>

                                                    <p className="small-body">
                                                        Units
                                                    </p>
                                                </div>

                                                <div
                                                    className="column-container"
                                                    style={{
                                                        width:
                                                            "110px",
                                                        height:
                                                            "110px",
                                                        alignItems:
                                                            "center",
                                                        justifyContent:
                                                            "center",
                                                        padding:
                                                            "var(--space-2)",
                                                        gap:
                                                            "var(--space-1)",
                                                        backgroundColor:
                                                            "var(--beige)",
                                                        borderRadius:
                                                            "var(--radius-md)",
                                                    }}
                                                >
                                                    <p className="small-body">
                                                        Good
                                                    </p>

                                                    <p
                                                        className="nav-item"
                                                        style={{
                                                            color:
                                                                "var(--dark-green)",
                                                        }}
                                                    >
                                                        {Math.max(
                                                            0,
                                                            selectedLine.receivedQuantity -
                                                            selectedLine.damagedQuantity
                                                        )}
                                                    </p>

                                                    <p className="small-body">
                                                        Units
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    )}
                            </>
                        )}
                    </div>

                    {/* =================================================
                        BOTTOM SEPARATOR
                    ================================================== */}

                    <div className="separator" />

                    {/* =================================================
                        PREVIOUS / NEXT
                    ================================================== */}

                    <div
                        className="row-container"
                        style={{
                            width: "100%",
                            justifyContent:
                                "space-between",
                        }}
                    >
                        <Button
                            size="sm"
                            variant="outline"
                            onClick={
                                previousStep
                            }
                            disabled={
                                saving ||
                                !selectedLine ||
                                (
                                    currentStep ===
                                    0 &&
                                    lines.findIndex(
                                        (line) =>
                                            line.purchaseOrderItemId ===
                                            selectedLineId
                                    ) === 0
                                )
                            }
                        >
                            Previous
                        </Button>

                        <Button
                            size="sm"
                            variant="secondary"
                            onClick={
                                nextStep
                            }
                            disabled={
                                saving ||
                                !selectedLine
                            }
                        >
                            {saving
                                ? "Saving..."
                                : currentStep ===
                                    RECEIVING_STEPS.length -
                                    1
                                    ? "Save & Next Line"
                                    : "Next"}
                        </Button>
                    </div>
                </div>
            </div>
        </div>
    );
}