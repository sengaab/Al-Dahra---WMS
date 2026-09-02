interface StatsCardProps {
    value: string | number;
    title: string;
    subtitle?: string;
    valueColor?: string;
}

export default function StatsCard({
    value,
    title,
    subtitle,
    valueColor = "var(--midnight-blue)",
}: StatsCardProps) {
    return (
        <div
            style={{
                display: "flex",
                flexDirection: "column",
                justifyContent: "flex-start",
                alignItems: "flex-start",
                padding: "var(--space-3) var(--space-4)",
                backgroundColor: "white",
                borderRadius: "var(--radius-md)",
                boxShadow: "var(--shadow-md)",
                boxSizing: "border-box",
                gap:"var(--space-1)",
                minHeight: "96px",
                minWidth:"var(--card-width-sm)",
            }}
        >

            <p className="nav-item" style={{color:valueColor}}>{value}</p>
            <p className="body-title">{title}</p>
            <p className="body">{subtitle}</p>
        </div>
    );
}