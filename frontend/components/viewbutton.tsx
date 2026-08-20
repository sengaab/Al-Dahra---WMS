"use client";

interface ViewAllProps {
    label?: string;
    onClick?: () => void;
}

export default function ViewAll({
    label = "View all Categories",
    onClick,
}: ViewAllProps) {
    return (
        <div
            onClick={onClick}
            style={{
                display: "flex",
                alignItems: "center",
                gap: "var(--space-3)",
                cursor: onClick ? "pointer" : "default",
            }}
        >
            <h4
                style={{
                    color: "var(--dark-green)",
                    margin: 0,
                }}
            >
                {label}
            </h4>

            <img
                src="/side arrow.svg"
                alt=""
                style={{
                    height: "var(--icon-xs)",
                }}
            />
        </div>
    );
}