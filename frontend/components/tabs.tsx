interface Tab {
    label: string;
    count?: number;
}

interface TabsProps {
    tabs: Tab[];
    activeTab: string;
    onChange: (tab: string) => void;
}

export default function Tabs({
    tabs,
    activeTab,
    onChange,
}: TabsProps) {
    return (
        <div
            style={{
                display: "flex",
                alignItems: "flex-end",
                width: "100%",
                borderBottom: "var(--border-default)",
                height: "40px",
            }}
        >
            {tabs.map((tab) => {
                const isActive = activeTab === tab.label;

                return (
                    <button
                        key={tab.label}
                        type="button"
                        onClick={() => onChange(tab.label)}
                        className="body-title"
                        style={{
                            position: "relative",
                            height: "40px",
                            padding: "0 23px",
                            border: "none",
                            background: "transparent",
                            color: isActive
                                ? "var(--dark-green)"
                                : "var(--midnight-blue)",
                            cursor: "pointer",
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "center",
                        }}
                    >
                        {tab.label}
                        {tab.count !== undefined && ` (${tab.count})`}

                        {isActive && (
                            <span
                                style={{
                                    position: "absolute",
                                    bottom: "-1px",
                                    left: "0px",
                                    right: "0px",
                                    height: "4px",
                                    backgroundColor: "var(--dark-green)",
                                    borderRadius: "var(--radius-lg)",
                                }}
                            />
                        )}
                    </button>
                );
            })}
        </div>
    );
}