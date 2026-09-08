"use client";

import Input from "@/components/input";
import Button from "@/components/button";
import { useMemo, useState } from "react";

interface TemplateProps {
    onClose?: () => void;
}

export default function Template({
    onClose,
}: TemplateProps) {

    const [loading, setLoading] = useState(false);
    const [loadingOptions, setLoadingOptions] = useState(true);

    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");

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
                    Template
                </p>

                <button
                    type="button"
                    onClick={onClose}
                    disabled={loading}
                    className="page-title"
                    style={{
                        border: "none",
                        background: "transparent",
                        cursor: loading
                            ? "not-allowed"
                            : "pointer",
                        color: "var(--midnight-blue)",
                        padding: 0,
                        opacity: loading ? 0.5 : 1,
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

        </div>
    );
}