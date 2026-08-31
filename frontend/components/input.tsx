"use client";

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
}: InputProps) {
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
                {!optional && <span style={{color:"var(--red)"}}> * </span>}
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
                    value={value}
                    onChange={(e) =>
                        onChange?.(
                            e.target.value
                        )
                    }
                    style={{
                        backgroundColor: "white",
                        border: "var(--border-default)",
                        borderRadius:
                            "var(--radius-md)",
                        height: "var(--input-height)",
                        width: "100%",
                        paddingInline:
                            "var(--space-3)",
                        boxSizing: "border-box",
                    }}
                />
            )}
        </div>
    );
}