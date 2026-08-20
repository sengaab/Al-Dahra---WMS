"use client";

import { useState } from "react";

interface SearchBarProps {
    placeholder?: string;
    onSearch?: (value: string) => void;
}

export default function SearchBar({
    placeholder = "Search...",
    onSearch,
}: SearchBarProps) {
    const [value, setValue] = useState("");

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
                maxWidth: "500px",
                height: "var(--input-height)",
                border: "var(--border-default)",
                borderColor: "var(--grey)",
                borderRadius: "var(--input-radius)",
                backgroundColor: "var(--white)",
                overflow: "hidden",
                boxSizing: "border-box",
            }}
        >
            {/* Search Icon */}
            <div
                style={{
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                    width: "var(--input-height)",
                    height: "100%",
                    color: "var(--grey)",
                    flexShrink: 0,
                }}
            >
                <svg
                    width="20"
                    height="20"
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
                value={value}
                onChange={(e) => setValue(e.target.value)}
                placeholder={placeholder}
                style={{
                    flex: 1,
                    minWidth: 0,
                    height: "100%",
                    border: "none",
                    outline: "none",
                    fontFamily: "var(--font-roboto)",
                    fontSize: "var(--font-placeholder)",
                    fontWeight: "var(--weight-placeholder)",
                    color: "black",
                    backgroundColor: "transparent",
                    padding: "0 var(--space-3) 0 0",
                }}
            />
        </form>
    );
}