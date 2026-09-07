interface NavigationCardProps {
    icon: string;
    title: string;
    subtitle?: string;
    onClick?: () => void;
}

export default function NavigationCard({
    icon,
    title,
    subtitle,
    onClick,
}: NavigationCardProps) {
    return (
        <button
            type="button"
            onClick={onClick}
            style={{
                display: "flex",
                justifyContent: "flex-start",
                alignItems: "center",
                padding: "var(--space-3) var(--space-4)",
                backgroundColor: "white",
                border: "none",
                borderRadius: "var(--radius-md)",
                boxShadow: "var(--shadow-md)",
                boxSizing: "border-box",
                gap: "var(--space-5)",
                minHeight: "96px",
                minWidth: "var(--card-width-md)",
                flex: 1,
                cursor: "pointer",
                textAlign: "left",
            }}
        >
            <div className="card"
                style={{
                    width:"fit-content",
                    height:"fit-content",
                    padding:"var(--space-2)"
                }}
            >

            {/* Icon */}
            <img
                src={icon}
                alt=""
                style={{
                    width: "var(--icon-xl)",
                    height: "var(--icon-xl)",
                    flexShrink: 0,
                    filter:"invert(1)"
                }}
            />
                </div>

            {/* Text */}
            <div className="column-container"
             style={{
                width:"fit-content",
             }}
            >
                <p className="body-title">{title}</p>

                {subtitle && (
                    <p className="body">{subtitle}</p>
                )}
            </div>

            {/* arrow */}
            <img
                src="/down arrow.svg"
                alt=""
                style={{
                    flexShrink: 0,
                    transform:"rotate(-90deg)",
                    marginLeft:"auto",
                }}
            />
        </button>
    );
}