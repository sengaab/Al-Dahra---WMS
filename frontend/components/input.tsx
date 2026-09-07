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
    type?: "text" | "number";
    optional?: boolean;
    selectOnFocus?: boolean;
    selectAfterEnter?: boolean;
    autoScan?: boolean;
    minScanLength?: number;
    scanInterval?: number;
}

export default function Input({
    label,
    placeholder,
    options,
    value,
    maxWidth = false,
    onChange,
    type = "text",
    optional = false,
    selectOnFocus = false,
    selectAfterEnter = false,
    autoScan = false,
    minScanLength = 4,
    scanInterval = 50,
}: InputProps) {
    const lastKeyTime = useRef<number>(0);
    const scanTimer = useRef<ReturnType<typeof setTimeout> | null>(null);

    const selectInput = (input: HTMLInputElement) => {
        requestAnimationFrame(() => {
            input.focus();
            input.select();
        });
    };

    const focusNextInput = (current: HTMLElement) => {
        const form = current.closest("form") ?? document;

        const focusableElements = Array.from(
            form.querySelectorAll<HTMLElement>(
                "input:not([disabled]), select:not([disabled]), textarea:not([disabled]), button:not([disabled])"
            )
        );

        const currentIndex = focusableElements.indexOf(current);

        if (currentIndex === -1) {
            return;
        }

        for (
            let i = currentIndex + 1;
            i < focusableElements.length;
            i++
        ) {
            const nextElement = focusableElements[i];

            if (
                nextElement instanceof HTMLInputElement ||
                nextElement instanceof HTMLSelectElement ||
                nextElement instanceof HTMLTextAreaElement
            ) {
                nextElement.focus();
                return;
            }
        }

        current.blur();
    };

    const handleChange = (
        e: React.ChangeEvent<HTMLInputElement>
    ) => {
        const newValue = e.target.value;

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

                if (input.value.length >= minScanLength) {
                    selectInput(input);
                }
            }, scanInterval + 10);
        }
    };

    const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
        e.preventDefault();
        e.currentTarget.blur();
    }
};
    return (
        <div
            style={{
                display: "flex",
                flexDirection: "column",
                alignItems: "flex-start",
                maxWidth: maxWidth
                    ? "100%"
                    : "50%",
                width: "100%",
                flex: 1,
            }}
        >
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

            {options ? (
                <Dropdown
                    options={options}
                    placeholder={placeholder}
                    value={value ?? ""}
                    onChange={onChange}
                    style={{
                        width: "100%",
                    }}
                />
            ) : (
                <input
                    type={type}
                    placeholder={placeholder}
                    value={value ?? ""}
                    onChange={handleChange}
                    onFocus={(e) => {
                        if (selectOnFocus) {
                            e.currentTarget.select();
                        }
                    }}
                    onKeyDown={handleKeyDown}
                    style={{
                        backgroundColor: "white",
                        border: "var(--border-default)",
                        borderRadius: "var(--radius-md)",
                        height: "var(--input-height)",
                        width: "100%",
                        paddingInline: "var(--space-3)",
                        boxSizing: "border-box",
                    }}
                />
            )}
        </div>
    );
}