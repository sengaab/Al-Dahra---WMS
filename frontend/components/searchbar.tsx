"use client";

import { useState } from "react";

interface SearchBarProps {
    placeholder?: string;
    value?: string;
    onChange?: (value: string) => void;
    onSearch?: (value: string) => void;
    style?: React.CSSProperties;
}

export default function SearchBar({
    placeholder = "Search...",
    value: controlledValue,
    onChange,
    onSearch,
    style,
}: SearchBarProps) {
    const [internalValue, setInternalValue] = useState("");

    const value =
        controlledValue !== undefined ? controlledValue : internalValue;

    const handleChange = (newValue: string) => {
        if (controlledValue === undefined) {
            setInternalValue(newValue);
        }

        onChange?.(newValue);
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        onSearch?.(value);
    };

    return (
        <form
            onSubmit={handleSubmit}
            style={{
                display: "flex",
                alignItems: "center",
                width: "100%",
                maxWidth: "600px",
                border: "var(--border-default)",
                borderRadius: "var(--input-radius)",
                backgroundColor: "var(--white)",
                overflow: "hidden",
                boxSizing: "border-box",
                padding: "var(--space-1)",
                gap: "var(--space-3)",
                height: "var(--input-height)",
                ...style,
            }}
        >
            {/* Search Icon */}
            <div
                style={{
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                    height: "100%",
                    color: "var(--grey)",
                    flexShrink: 0,
                }}
            >
                <svg
                    height="var(--icon-md)"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    strokeWidth="2"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                >
                    <circle cx="11" cy="11" r="8" />
                    <path d="m21 21-4.3-4.3" />
                </svg>
            </div>

            {/* Input */}
            <input
                type="text"
                className="placeholder"
                value={value}
                onChange={(e) => handleChange(e.target.value)}
                placeholder={placeholder}
                style={{
                    flex: 1,
                    minWidth: 0,
                    height: "100%",
                    border: "none",
                    outline: "none",
                    color: "black",
                    backgroundColor: "transparent",
                    padding: "0 var(--space-3) 0 0",
                }}
            />
        </form>
    );
}