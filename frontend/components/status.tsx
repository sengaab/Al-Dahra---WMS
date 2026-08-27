"use-client";
interface StatusProps {
    variant?: "green" | "orange" | "red" | "blue" | "grey" | "purple";
    text?: string;
}
export default function Status({
    variant = "green",
    text = "Active",
}: StatusProps) {
    return (
        <p
            className="body"
            style={{
                color: "var(--midnight-blue)",
                backgroundColor: `var(--light-${variant})`,
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