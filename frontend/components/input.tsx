"use client";

import { useRef } from "react";
import Dropdown from "./dropdown";

interface DropdownOption {
    label: string;
    value: string;
}

interface InputProps {
    label: string;
    placeholder: string;
    options?: DropdownOption[];
    value?: string;
    onChange?: (value: string) => void;
    maxWidth?: boolean;
    type?: "text" | "number" | "date";
    min?: number;
    step?: number | string;
    optional?: boolean;
    selectOnFocus?: boolean;
    selectAfterEnter?: boolean;
    autoScan?: boolean;
    minScanLength?: number;
    scanInterval?: number;
    disabled?: boolean;
}

export default function Input({
    label,
    placeholder,
    options,
    value,
    maxWidth = false,
    onChange,
    type = "text",
    min,
    step,
    optional = false,
    selectOnFocus = false,
    selectAfterEnter = false,
    autoScan = false,
    minScanLength = 4,
    scanInterval = 50,
    disabled = false,
}: InputProps) {
    const lastKeyTime = useRef<number>(0);
    const scanTimer = useRef<ReturnType<typeof setTimeout> | null>(null);

    const selectInput = (input: HTMLInputElement) => {
        if (disabled) return;

        requestAnimationFrame(() => {
            input.focus();
            input.select();
        });
    };

    const handleChange = (
        e: React.ChangeEvent<HTMLInputElement>
    ) => {
        if (disabled) return;

        const newValue = e.target.value;

        // Prevent negative numbers
        if (
            type === "number" &&
            newValue !== "" &&
            Number(newValue) < 0
        ) {
            return;
        }

        onChange?.(newValue);

        if (!autoScan) {
            return;
        }

        const now = Date.now();

        const timeSinceLastKey =
            now - lastKeyTime.current;

        lastKeyTime.current = now;

        if (scanTimer.current) {
            clearTimeout(scanTimer.current);
        }

        if (
            timeSinceLastKey <= scanInterval &&
            newValue.length >= minScanLength
        ) {
            scanTimer.current = setTimeout(() => {
                const input = e.target as HTMLInputElement;

                if (
                    !disabled &&
                    input.value.length >= minScanLength
                ) {
                    selectInput(input);
                }
            }, scanInterval + 10);
        }
    };

    const handleKeyDown = (
        e: React.KeyboardEvent<HTMLInputElement>
    ) => {
        if (disabled) return;

        // Prevent minus key for number inputs
        if (
            type === "number" &&
            (e.key === "-" || e.key === "Subtract")
        ) {
            e.preventDefault();
            return;
        }

        if (e.key === "Enter") {
            e.preventDefault();

            if (selectAfterEnter) {
                selectInput(e.currentTarget);
            } else {
                e.currentTarget.blur();
            }
        }
    };

    return (
        <div
            style={{
                display: "flex",
                flexDirection: "column",
                alignItems: "flex-start",
                maxWidth: maxWidth ? "100%" : "50%",
                width: "100%",
                flex: 1,
                opacity: disabled ? 0.6 : 1,
            }}
        >
            {label && (
                <p className="body-title">
                    {label}

                    {!optional && (
                        <span
                            style={{
                                color: "var(--red)",
                            }}
                        >
                            {" "}*{" "}
                        </span>
                    )}
                </p>
            )}

            {options ? (
                <Dropdown
                    options={options}
                    placeholder={placeholder}
                    value={value ?? ""}
                    onChange={onChange}
                    disabled={disabled}
                    style={{
                        width: "100%",
                    }}
                />
            ) : (
                <input
                    type={type}
                    placeholder={placeholder}
                    value={value ?? ""}
                    min={type === "number" ? min : undefined}
                    step={type === "number" ? step : undefined}
                    disabled={disabled}
                    onChange={handleChange}
                    onFocus={(e) => {
                        if (
                            !disabled &&
                            selectOnFocus &&
                            type !== "date"
                        ) {
                            e.currentTarget.select();
                        }
                    }}
                    onKeyDown={handleKeyDown}
                    style={{
                        backgroundColor: disabled
                            ? "var(--light-grey)"
                            : "white",
                        border: "var(--border-default)",
                        borderRadius: "var(--radius-md)",
                        height: "var(--input-height)",
                        width: "100%",
                        paddingInline: "var(--space-3)",
                        boxSizing: "border-box",
                        cursor: disabled
                            ? "not-allowed"
                            : "text",
                    }}
                />
            )}
        </div>
    );
}