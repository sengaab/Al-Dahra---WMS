"use client";

import { useState, useRef, useEffect } from "react";

interface DropdownOption {
    label: string;
    value: string;
}

interface DropdownProps {
    options: DropdownOption[];
    placeholder?: string;
    value?: string;
    onChange?: (value: string) => void;
    width?: string;
}

export default function Dropdown({
    options,
    placeholder = "Select...",
    value,
    onChange,
    width = "200px",
}: DropdownProps) {
    const [open, setOpen] = useState(false);
    const dropdownRef = useRef<HTMLDivElement>(null);

    const selectedOption = options.find(
        (option) => option.value === value
    );

    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            if (
                dropdownRef.current &&
                !dropdownRef.current.contains(event.target as Node)
            ) {
                setOpen(false);
            }
        };

        document.addEventListener("mousedown", handleClickOutside);

        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, []);

    const handleSelect = (option: DropdownOption) => {
        onChange?.(option.value);
        setOpen(false);
    };

    return (
        <div
            ref={dropdownRef}
            style={{
                position: "relative",
                width,
            }}
        >
            {/* Trigger */}
            <button
                type="button"
                onClick={() => setOpen((prev) => !prev)}
                style={{
                    width: "100%",
                    height: "fit-content",
                    padding: "0 5px",
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "space-between",
                    backgroundColor: "white",
                    border: "1px solid #ddd",
                    borderRadius: "var(--radius-md)",
                    cursor: "pointer",
                    outline: "none",
                    fontFamily: "var(--font-roboto)",
                    color: selectedOption
                        ? "var(--midnight-blue)"
                        : "var(--grey)",
                }}
            >
                <h6>
                    {selectedOption?.label || placeholder}
                </h6>

                <img
                    src={"/down arrow.svg"}
                />
            </button>

            {/* Menu */}
            {open && (
                <div
                    style={{
                        position: "absolute",
                        top: "calc(100% + 6px)",
                        left: 0,
                        width: "100%",
                        backgroundColor: "white",
                        borderRadius: "var(--radius-md)",
                        boxShadow: "var(--shadow-md)",
                        border: "1px solid #eee",
                        padding: "6px",
                        zIndex: 1000,
                        boxSizing: "border-box",
                    }}
                >
                    {options.map((option) => (
                        <button
                            key={option.value}
                            type="button"
                            onClick={() => handleSelect(option)}
                            style={{
                                width: "100%",
                                padding: "10px 12px",
                                border: "none",
                                borderRadius: "6px",
                                backgroundColor:
                                    value === option.value
                                        ? "var(--beige)"
                                        : "transparent",
                                color:
                                    value === option.value
                                        ? "var(--dark-green)"
                                        : "var(--midnight-blue)",
                                textAlign: "left",
                                cursor: "pointer",
                                fontFamily: "var(--font-roboto)",
                                fontSize: "14px",
                            }}
                        >
                            {option.label}
                        </button>
                    ))}
                </div>
            )}
        </div>
    );
}