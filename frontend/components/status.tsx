"use client";

interface StatusProps {
    variant?:
        | "green"
        | "orange"
        | "red"
        | "blue"
        | "grey"
        | "yellow"
        | "green-stroke"
        | "orange-stroke"
        | "red-stroke"
        | "blue-stroke"
        | "grey-stroke"
        | "yellow-stroke";
    text?: string;
}

export default function Status({
    variant = "green",
    text = "Active",
}: StatusProps) {
    const isStroke = variant.endsWith("-stroke");
    const color = variant.replace("-stroke", "");

    return (
        <p
            className="body"
            style={{
                color: isStroke
                    ? `var(--dark-${color})`
                    : "var(--midnight-blue)",
                backgroundColor: isStroke
                    ? "transparent"
                    : `var(--light-${color})`,
                border: isStroke
                    ? `1px solid var(--light-${color})`
                    : "none",
                textAlign: "center",
                width: "fit-content",
                paddingInline: "var(--space-2)",
                paddingBlock: "var(--space-1)",
                borderRadius: "var(--radius-md)",
            }}
        >
            {text}
        </p>
    );
}