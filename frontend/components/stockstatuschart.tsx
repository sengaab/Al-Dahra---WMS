"use client";

import {
    PieChart,
    Pie,
    Cell,
    ResponsiveContainer,
} from "recharts";

type StockStatus = {
    name: string;
    value: number;
    color: string;
};

type StockStatusChartProps = {
    stockStatus: StockStatus[];
};

export default function StockStatusChart({
    stockStatus,
}: StockStatusChartProps) {
    return (
        <div
            style={{
                width: "100%",
                height: "100%",
                display: "flex",
                alignItems: "center",
                justifyContent:"space-between",
                gap: "var(--space-10)",
            }}
        >
            {/* DONUT */}
            <div
                style={{
                    height: "100%",
                    minWidth:0,
                    flex:1,
                }}
            >
                <ResponsiveContainer width="100%" height="100%">
                    <PieChart>
                        <Pie
                            data={stockStatus}
                            dataKey="value"
                            nameKey="name"
                            cx="50%"
                            cy="50%"
                            innerRadius="70%"
                            outerRadius="100%"
                            stroke="none"
                        >
                            {stockStatus.map((item) => (
                                <Cell
                                    key={item.name}
                                    fill={item.color}
                                />
                            ))}
                        </Pie>
                    </PieChart>
                </ResponsiveContainer>
            </div>

            {/* LEGEND */}
            <div
                style={{
                    flex: 1,
                    display: "flex",
                    flexDirection: "column",
                    justifyContent: "center",
                    gap: "var(--space-4)",
                }}
            >
                {stockStatus.map((item) => (
                    <div
                        key={item.name}
                        style={{
                            display: "grid",
                            gridTemplateColumns:
                                "14px minmax(0, 1fr) auto",
                            alignItems: "center",
                            gap: "var(--space-3)",
                        }}
                    >
                        <div
                            style={{
                                width: "20px",
                                height: "20px",
                                borderRadius: "50%",
                                backgroundColor: item.color,
                            }}
                        />

                        <h6
                            style={{
                                whiteSpace: "nowrap",
                            }}
                        >
                            {item.name}
                        </h6>

                        <h4
                            style={{
                                whiteSpace: "nowrap",
                            }}
                        >
                            {item.value}%
                        </h4>
                    </div>
                ))}
            </div>
        </div>
    );
}