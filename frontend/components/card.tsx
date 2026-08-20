import React from "react";
import Dropdown from "@/components/dropdown";
import ViewAll from "@/components/viewbutton";

type DropdownOption = {
    label: string;
    value: string;
};

type CardProps = {
    title: string;
    children: React.ReactNode;

    width?: string;
    minHeight?: string;

    dropdown?: boolean;
    dropdownPlaceholder?: string;
    dropdownValue?: string;
    dropdownOnChange?: (value: string) => void;
    dropdownOptions?: DropdownOption[];
    dropdownWidth?: string;

    viewAll?: boolean;
    viewAllLabel?: string;
    onViewAll?: () => void;
    viewAllPosition?: "header" | "footer";
    flex?: number;
    overflow?: boolean;
};

export default function Card({
    title,
    children,

    width = "var(--card-lg-width)",
    minHeight = "var(--card-lg-height)",

    dropdown = false,
    dropdownPlaceholder = "Select",
    dropdownValue = "",
    dropdownOnChange,
    dropdownOptions = [],
    dropdownWidth = "40%",
    overflow = false,
    flex = 1,

    viewAll = false,
    viewAllLabel = "View All",
    onViewAll,
    viewAllPosition = "footer",
}: CardProps) {
    const showHeaderViewAll =
        viewAll && viewAllPosition === "header";

    const showFooterViewAll =
        viewAll && viewAllPosition === "footer";

    return (
        <div
            style={{
                backgroundColor: "white",
                padding: "var(--card-padding)",
                borderRadius: "var(--radius-md)",
                boxShadow: "var(--shadow-md)",
                minWidth:"var(--card-lg-width)",
                flex:flex,
                minHeight,
                boxSizing: "border-box",

                display: "flex",
                flexDirection: "column",
                gap: "var(--space-5)",
            }}
        >
            {/* Header */}
            <div
                style={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                    gap: "var(--space-4)",
                }}
            >
                <h3
                    style={{
                        margin: 0,
                    }}
                >
                    {title}
                </h3>

                <div
                    style={{
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "flex-end",
                        gap: "var(--space-3)",
                        flex: 1,
                    }}
                >
                    {/* Optional Dropdown */}
                    {dropdown && (
                        <Dropdown
                            placeholder={dropdownPlaceholder}
                            value={dropdownValue}
                            onChange={
                                dropdownOnChange ?? (() => {})
                            }
                            options={dropdownOptions}
                            width={"100%"}
                        />
                    )}

                    {/* Optional Header View All */}
                    {showHeaderViewAll && (
                        <ViewAll
                            label={viewAllLabel}
                            onClick={
                                onViewAll ??
                                (() =>
                                    console.log(
                                        "View all clicked"
                                    ))
                            }
                        />
                    )}
                </div>
            </div>

            {/* Content */}
            <div
                style={{
                    width: "100%",
                    flex: 1,
                    maxHeight:overflow?"var(--card-height-md)":"none",
                    overflowY:"auto",
                }}
            >
                {children}
            </div>

            {/* Optional Footer View All */}
            {showFooterViewAll && (
                <div
                    style={{
                        display: "flex",
                        justifyContent: "flex-end",
                    }}
                >
                    <ViewAll
                        label={viewAllLabel}
                        onClick={
                            onViewAll ??
                            (() =>
                                console.log(
                                    "View all clicked"
                                ))
                        }
                    />
                </div>
            )}
        </div>
    );
}