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
    style?: React.CSSProperties;
    disabled?: boolean;
}

export default function Dropdown({
    options,
    placeholder = "Select...",
    value,
    onChange,
    style,
    disabled = false,
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

    // Close dropdown if it becomes disabled
    useEffect(() => {
        if (disabled) {
            setOpen(false);
        }
    }, [disabled]);

    const handleSelect = (option: DropdownOption) => {
        if (disabled) return;

        onChange?.(option.value);
        setOpen(false);
    };

    const handleToggle = () => {
        if (disabled) return;

        setOpen((prev) => !prev);
    };

    return (
        <div
            ref={dropdownRef}
            style={{
                position: "relative",
                ...style,
            }}
        >
            {/* Trigger */}
            <button
                type="button"
                disabled={disabled}
                onClick={handleToggle}
                style={{
                    display: "flex",
                    alignItems: "center",
                    width: "100%",
                    border: "var(--border-default)",
                    borderRadius: "var(--input-radius)",
                    backgroundColor: disabled
                        ? "var(--light-grey)"
                        : "white",
                    height: "var(--input-height)",
                    paddingInline: "var(--input-padding-x)",
                    justifyContent: "space-between",
                    boxSizing: "border-box",
                    cursor: disabled ? "not-allowed" : "pointer",
                    outline: "none",
                    color: selectedOption
                        ? "var(--midnight-blue)"
                        : "var(--grey)",
                    opacity: disabled ? 0.6 : 1,
                }}
            >
                <p className="placeholder">
                    {selectedOption?.label || placeholder}
                </p>

                <img
                    src="/down arrow.svg"
                    style={{
                        opacity: disabled ? 0.5 : 1,
                    }}
                />
            </button>

            {/* Menu */}
            {open && !disabled && (
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