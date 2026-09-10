"use client";

import {
    useSearchParams,
    useRouter,
} from "next/navigation";

import ReceiveStepper from "../components/receive-stepper";
import Button from "@/components/button";
import Status from "@/components/status";

import { useConfirmReceipt } from "@/hooks/use-confirm-receipt";

export default function ConfirmReceipt() {
    const router = useRouter();
    const searchParams =
        useSearchParams();

    const receiptIdParam =
        searchParams.get("receiptId");

    const receiptId = receiptIdParam
        ? Number(receiptIdParam)
        : null;

    const {
        lines,
        receiptStatus,
        loading,
        posting,
        error,
        success,
        postReceiptToInspection,
    } = useConfirmReceipt(
        receiptId
    );

    const formatCurrency = (
        value: number
    ) =>
        value
            ? `E£ ${value.toLocaleString(
                  "en-US",
                  {
                      minimumFractionDigits: 2,
                      maximumFractionDigits: 2,
                  }
              )}`
            : "-";

    return (
        <div
            className="page-column"
            style={{
                gap: "var(--space-4)",
            }}
        >
            <ReceiveStepper />

            <div
                className="card column-container"
                style={{
                    width: "100%",
                    paddingInline: 0,
                    overflow: "hidden",
                }}
            >
                <div
                    style={{
                        display: "flex",
                        justifyContent:
                            "space-between",
                        alignItems: "center",
                        paddingInline:
                            "var(--space-3)",
                        marginBottom:
                            "var(--space-3)",
                    }}
                >
                    <p
                        className="body-title"
                        style={{
                            margin: 0,
                        }}
                    >
                        Confirm Receipt
                    </p>

                    {receiptStatus && (
                        <Status
                            text={
                                receiptStatus
                            }
                        />
                    )}
                </div>

                {loading && (
                    <div
                        style={{
                            padding:
                                "var(--space-4)",
                            color:
                                "var(--grey)",
                        }}
                    >
                        Loading receipt...
                    </div>
                )}

                {!loading && error && (
                    <div
                        style={{
                            padding:
                                "var(--space-3)",
                            color:
                                "var(--blood-red)",
                        }}
                    >
                        {error}
                    </div>
                )}

                {!loading &&
                    !error &&
                    success && (
                        <div
                            style={{
                                padding:
                                    "var(--space-3)",
                                color:
                                    "var(--dark-green)",
                            }}
                        >
                            {success}
                        </div>
                    )}

                {!loading &&
                    !error &&
                    lines.length === 0 && (
                        <div
                            style={{
                                padding:
                                    "var(--space-4)",
                                color:
                                    "var(--grey)",
                            }}
                        >
                            No receipt items
                            found.
                        </div>
                    )}

                {!loading &&
                    lines.length > 0 && (
                        <div
                            style={{
                                width: "100%",
                                overflowX:
                                    "auto",
                            }}
                        >
                            <table
                                style={{
                                    width: "100%",
                                    borderCollapse:
                                        "collapse",
                                    minWidth:
                                        "1100px",
                                }}
                            >
                                <thead>
                                    <tr
                                        style={{
                                            backgroundColor:
                                                "var(--beige)",
                                            borderBlock:
                                                "1px solid var(--light-grey)",
                                            height:
                                                "44px",
                                        }}
                                    >
                                        {[
                                            "SKU",
                                            "Product",
                                            "Expected",
                                            "Received",
                                            "Damaged",
                                            "Net",
                                            "Batch",
                                            "Expiry",
                                            "Unit Price",
                                            "Line Total",
                                            "Inspection",
                                        ].map(
                                            (
                                                header
                                            ) => (
                                                <th
                                                    key={
                                                        header
                                                    }
                                                    style={{
                                                        padding:
                                                            "var(--space-2) var(--space-3)",
                                                        textAlign:
                                                            "left",
                                                        whiteSpace:
                                                            "nowrap",
                                                    }}
                                                >
                                                    {
                                                        header
                                                    }
                                                </th>
                                            )
                                        )}
                                    </tr>
                                </thead>

                                <tbody>
                                    {lines.map(
                                        (
                                            line
                                        ) => (
                                            <tr
                                                key={
                                                    line.receiptItemId
                                                }
                                                style={{
                                                    borderBottom:
                                                        "1px solid var(--light-grey)",
                                                }}
                                            >
                                                <td
                                                    style={{
                                                        padding:
                                                            "var(--space-3)",
                                                        whiteSpace:
                                                            "nowrap",
                                                    }}
                                                >
                                                    {
                                                        line.sku
                                                    }
                                                </td>

                                                <td
                                                    style={{
                                                        padding:
                                                            "var(--space-3)",
                                                        whiteSpace:
                                                            "nowrap",
                                                    }}
                                                >
                                                    {
                                                        line.product
                                                    }
                                                </td>

                                                <td
                                                    style={{
                                                        padding:
                                                            "var(--space-3)",
                                                    }}
                                                >
                                                    {
                                                        line.expected
                                                    }
                                                </td>

                                                <td
                                                    style={{
                                                        padding:
                                                            "var(--space-3)",
                                                    }}
                                                >
                                                    {
                                                        line.received
                                                    }
                                                </td>

                                                <td
                                                    style={{
                                                        padding:
                                                            "var(--space-3)",
                                                    }}
                                                >
                                                    {line.damaged ||
                                                        "-"}
                                                </td>

                                                <td
                                                    style={{
                                                        padding:
                                                            "var(--space-3)",
                                                        fontWeight:
                                                            700,
                                                    }}
                                                >
                                                    {
                                                        line.net
                                                    }
                                                </td>

                                                <td
                                                    style={{
                                                        padding:
                                                            "var(--space-3)",
                                                        whiteSpace:
                                                            "nowrap",
                                                    }}
                                                >
                                                    {line.batch ||
                                                        "-"}
                                                </td>

                                                <td
                                                    style={{
                                                        padding:
                                                            "var(--space-3)",
                                                        whiteSpace:
                                                            "nowrap",
                                                    }}
                                                >
                                                    {line.expiry ||
                                                        "-"}
                                                </td>

                                                <td
                                                    style={{
                                                        padding:
                                                            "var(--space-3)",
                                                        whiteSpace:
                                                            "nowrap",
                                                    }}
                                                >
                                                    {formatCurrency(
                                                        line.unitPrice
                                                    )}
                                                </td>

                                                <td
                                                    style={{
                                                        padding:
                                                            "var(--space-3)",
                                                        fontWeight:
                                                            700,
                                                        whiteSpace:
                                                            "nowrap",
                                                    }}
                                                >
                                                    {formatCurrency(
                                                        line.lineTotal
                                                    )}
                                                </td>

                                                <td
                                                    style={{
                                                        padding:
                                                            "var(--space-3)",
                                                    }}
                                                >
                                                    <Status
                                                        text={
                                                            line.inspection
                                                        }
                                                    />
                                                </td>
                                            </tr>
                                        )
                                    )}
                                </tbody>
                            </table>
                        </div>
                    )}

                <div
                    style={{
                        display: "flex",
                        justifyContent:
                            "space-between",
                        gap: "var(--space-3)",
                        padding:
                            "var(--space-3)",
                        width: "100%",
                    }}
                >
                    <Button
                        variant="outline"
                        onClick={() =>
                            router.back()
                        }
                        disabled={posting}
                    >
                        Cancel
                    </Button>

                    <Button
                        variant="secondary"
                        onClick={
                            postReceiptToInspection
                        }
                        disabled={
                            posting ||
                            loading ||
                            lines.length ===
                                0 ||
                            receiptStatus ===
                                "Pending"
                        }
                    >
                        {posting
                            ? "Posting..."
                            : "Post Receipt to Inspection"}
                    </Button>
                </div>
            </div>
        </div>
    );
}