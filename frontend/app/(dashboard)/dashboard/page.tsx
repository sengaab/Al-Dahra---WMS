"use client";

import StatsCard from "@/components/stats-card";

export default function Dashboard() {
    const stats = [
        {
            value: "18,000",
            title: "Total SKUs",
            subtitle: "Across All Warehouses",
            valueColor: "var(--blue)",
        },
        {
            value: "52,000",
            title: "Total Stock Units",
            subtitle: "On Hand",
            valueColor: "var(--blue)",
        },
        {
            value: "E£1,000,000",
            title: "Stock Value",
            subtitle: "Current Valuation",
            valueColor: "var(--dark-green)",
        },
        {
            value: "44,000",
            title: "Available Stock",
            subtitle: "Ready For Use",
            valueColor: "var(--dark-green)",
        },
        {
            value: "8,000",
            title: "Reserved Stock",
            subtitle: "Committed To Requests",
            valueColor: "var(--red)",
        },
        {
            value: "70",
            title: "Low Stock",
            subtitle: "SKUs Below Minimum",
            valueColor: "var(--orange)",
        },
        {
            value: "12",
            title: "Out Of Stock",
            subtitle: "SKUs Depleted",
            valueColor: "var(--red)",
        },
        {
            value: "6",
            title: "Pending POs",
            subtitle: "Awaiting Processing",
            valueColor: "var(--orange)",
        },
        {
            value: "4",
            title: "Pending Receipts",
            subtitle: "Expected Deliveries",
            valueColor: "var(--orange)",
        },
        {
            value: "3",
            title: "Pending Requests",
            subtitle: "Awaiting Approval",
            valueColor: "var(--orange)",
        },
        {
            value: "2",
            title: "Pending Picks",
            subtitle: "Pick Tasks Open",
            valueColor: "var(--orange)",
        },
        {
            value: "1",
            title: "Pending Transfers",
            subtitle: "In Transit",
            valueColor: "var(--orange)",
        },
    ];

    const workflow = [
        {
            count: "6",
            label: "Procurement",
            type: "orange",
        },
        {
            count: "4",
            label: "Receiving",
            type: "blue",
        },
        {
            count: "3",
            label: "Putaway",
            type: "red",
        },
        {
            count: "1k+",
            label: "Inventory",
            type: "green",
        },
        {
            count: "3",
            label: "Requests",
            type: "orange",
        },
        {
            count: "2",
            label: "Picking",
            type: "red",
        },
        {
            count: "1",
            label: "Issue",
            type: "blue",
        },
    ];



    return (
        <div className="page-column">
            <div className="column-container">
                <p className="nav-item-serif">Key Performance Indicators</p>
                <div
                    className="row-container"
                    style={{
                        width: "100%",
                        flexWrap: "wrap",
                    }}
                >
                    {stats.map((stat) => (
                        <StatsCard
                            key={stat.title}
                            value={stat.value}
                            title={stat.title}
                            subtitle={stat.subtitle}
                            valueColor={stat.valueColor}
                        />
                    ))}
                </div>
            </div>

            <div className="column-container">
                <p className="nav-item-serif">Process Chain Status</p>
                <div
                    className="row-container"
                    style={{
                        width: "100%",
                    }}
                >
                    <div className="column-container">

                        <div className="card row-container" style={{ width: "100%", gap: "var(--space-1)" }}>

                            {workflow.map((item, index) => (
                                <div
                                    key={item.label}
                                    style={{
                                        display: "flex",
                                        alignItems: "center",
                                        flex: index < workflow.length - 1 ? 1 : "none",
                                        minWidth: 0,
                                    }}
                                >
                                    <div
                                        style={{
                                            display: "flex",
                                            flexDirection: "column",
                                            alignItems: "center",
                                            minWidth: "100px",
                                        }}
                                    >
                                        <div
                                            className="nav-item-serif"
                                            style={{
                                                width: "45px",
                                                height: "45px",
                                                borderRadius: "50%",
                                                backgroundColor:
                                                    item.type === "orange"
                                                        ? "var(--light-orange)"
                                                        : item.type === "blue"
                                                            ? "var(--light-blue)"
                                                            : item.type === "red"
                                                                ? "var(--light-red)"
                                                                : "var(--light-green)",
                                                display: "flex",
                                                alignItems: "center",
                                                justifyContent: "center",
                                            }}
                                        >
                                            {item.count}
                                        </div>

                                        <span
                                            className="body"
                                            style={{
                                                whiteSpace: "nowrap",
                                            }}
                                        >
                                            {item.label}
                                        </span>
                                    </div>

                                    {index < workflow.length - 1 && (
                                        <div
                                            style={{
                                                height: "1px",
                                                backgroundColor: "var(--grey)",
                                                flex: 1,
                                                marginBottom: "var(--line-body)"
                                            }}
                                        />
                                    )}
                                </div>
                            ))}

                        </div>
                        <div className="card column-container" style={{ paddingInline: "0" }}>
                            <p className="nav-item-serif" style={{ paddingInline: "var(--space-3)" }}>Warehouse Overview</p>

                            <table
                                style={{
                                    width: "100%",
                                    borderCollapse: "collapse",
                                }}
                            >
                                <thead>
                                    <tr>
                                        {[
                                            "Warehouse",
                                            "Site",
                                            "SKUs",
                                            "Units",
                                            "Available",
                                            "Reserved",
                                            "Low Stock",
                                            "Status",
                                        ].map((header) => (
                                            <th
                                                key={header}
                                                style={{
                                                    padding: "var(--space-2) var(--space-3)",
                                                    textAlign: [
                                                        "SKUs",
                                                        "Units",
                                                        "Available",
                                                        "Reserved",
                                                        "Low Stock",
                                                    ].includes(header)
                                                        ? "right"
                                                        : "left",
                                                    borderBottom: "1px solid var(--light-grey)",
                                                    fontFamily: "Roboto, sans-serif",
                                                    fontSize: "var(--font-body)",
                                                    lineHeight: "var(--line-body)",
                                                    fontWeight: 700,
                                                    color: "var(--midnight-blue)",
                                                    whiteSpace: "nowrap",
                                                }}
                                            >
                                                {header}
                                            </th>
                                        ))}
                                    </tr>
                                </thead>

                                <tbody>
                                    {[
                                        {
                                            warehouse: "Warehouse A",
                                            site: "Toshka",
                                            skus: 412,
                                            units: 18430,
                                            available: 14200,
                                            reserved: 4230,
                                            lowStock: 23,
                                            status: "Active",
                                        },
                                        {
                                            warehouse: "Warehouse A",
                                            site: "Toshka",
                                            skus: 412,
                                            units: 18430,
                                            available: 14200,
                                            reserved: 4230,
                                            lowStock: 23,
                                            status: "Active",
                                        },
                                        {
                                            warehouse: "Warehouse A",
                                            site: "Toshka",
                                            skus: 412,
                                            units: 18430,
                                            available: 14200,
                                            reserved: 4230,
                                            lowStock: 23,
                                            status: "Active",
                                        },
                                        {
                                            warehouse: "Warehouse A",
                                            site: "Toshka",
                                            skus: 412,
                                            units: 18430,
                                            available: 14200,
                                            reserved: 4230,
                                            lowStock: 23,
                                            status: "Active",
                                        },
                                    ].map((warehouse, index) => (
                                        <tr key={index}>
                                            <td
                                                style={{
                                                    padding: "var(--space-2) var(--space-3)",
                                                    borderBottom: "1px solid var(--light-grey)",
                                                    fontFamily: "Roboto, sans-serif",
                                                    fontSize: "var(--font-body)",
                                                    lineHeight: "var(--line-body)",
                                                    color: "var(--midnight-blue)",
                                                    whiteSpace: "nowrap",
                                                }}
                                            >
                                                {warehouse.warehouse}
                                            </td>

                                            <td
                                                style={{
                                                    padding: "var(--space-2) var(--space-3)",
                                                    borderBottom: "1px solid var(--light-grey)",
                                                    fontFamily: "Roboto, sans-serif",
                                                    fontSize: "var(--font-body)",
                                                    lineHeight: "var(--line-body)",
                                                    color: "var(--grey)",
                                                    whiteSpace: "nowrap",
                                                }}
                                            >
                                                {warehouse.site}
                                            </td>

                                            <td
                                                style={{
                                                    padding: "var(--space-2) var(--space-3)",
                                                    textAlign: "right",
                                                    borderBottom: "1px solid var(--light-grey)",
                                                    fontFamily: "Roboto, sans-serif",
                                                    fontSize: "var(--font-body)",
                                                    lineHeight: "var(--line-body)",
                                                    color: "var(--midnight-blue)",
                                                }}
                                            >
                                                {warehouse.skus.toLocaleString()}
                                            </td>

                                            <td
                                                style={{
                                                    padding: "var(--space-2) var(--space-3)",
                                                    textAlign: "right",
                                                    borderBottom: "1px solid var(--light-grey)",
                                                    fontFamily: "Roboto, sans-serif",
                                                    fontSize: "var(--font-body)",
                                                    lineHeight: "var(--line-body)",
                                                    color: "var(--midnight-blue)",
                                                }}
                                            >
                                                {warehouse.units.toLocaleString()}
                                            </td>

                                            <td
                                                style={{
                                                    padding: "var(--space-2) var(--space-3)",
                                                    textAlign: "right",
                                                    borderBottom: "1px solid var(--light-grey)",
                                                    fontFamily: "Roboto, sans-serif",
                                                    fontSize: "var(--font-body)",
                                                    lineHeight: "var(--line-body)",
                                                    color: "var(--dark-green)",
                                                }}
                                            >
                                                {warehouse.available.toLocaleString()}
                                            </td>

                                            <td
                                                style={{
                                                    padding: "var(--space-2) var(--space-3)",
                                                    textAlign: "right",
                                                    borderBottom: "1px solid var(--light-grey)",
                                                    fontFamily: "Roboto, sans-serif",
                                                    fontSize: "var(--font-body)",
                                                    lineHeight: "var(--line-body)",
                                                    color: "var(--red)",
                                                }}
                                            >
                                                {warehouse.reserved.toLocaleString()}
                                            </td>

                                            <td
                                                style={{
                                                    padding: "var(--space-2) var(--space-3)",
                                                    textAlign: "right",
                                                    borderBottom: "1px solid var(--light-grey)",
                                                    fontFamily: "Roboto, sans-serif",
                                                    fontSize: "var(--font-body)",
                                                    lineHeight: "var(--line-body)",
                                                    color: "var(--orange)",
                                                }}
                                            >
                                                {warehouse.lowStock}
                                            </td>

                                            <td
                                                style={{
                                                    padding: "var(--space-2) var(--space-3)",
                                                    borderBottom: "1px solid var(--light-grey)",
                                                }}
                                            >
                                                <span
                                                    style={{
                                                        display: "inline-flex",
                                                        alignItems: "center",
                                                        justifyContent: "center",
                                                        padding: "3px 10px",
                                                        borderRadius: "999px",
                                                        backgroundColor: "var(--light-green)",
                                                        color: "var(--dark-green)",
                                                        fontFamily: "Roboto, sans-serif",
                                                        fontSize: "var(--font-small-body)",
                                                        lineHeight: "var(--line-small-body)",
                                                        whiteSpace: "nowrap",
                                                    }}
                                                >
                                                    {warehouse.status}
                                                </span>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>

                        </div>

                    </div>

                    <div
                        className="card column-container"
                        style={{
                            width: "var(--sidedetails-width-lg)",
                            height: "100%",
                        }}
                    >
                        <p className="nav-item-serif">Recent Activity</p>

                        <div
                            className="column-container"
                            style={{
                                width: "100%",
                                gap: "0",
                            }}
                        >
                            {[
                                {
                                    time: "09:00",
                                    user: "Ahmed",
                                    action: "received",
                                    quantity: "1,200",
                                    item: "units of fertilizer X",
                                    location: "TK - Fertilizers 1",
                                    type: "blue",
                                },
                                {
                                    time: "09:00",
                                    user: "Maged",
                                    action: "transferred",
                                    quantity: "20",
                                    item: "units of Bearing 6205",
                                    location: "from TK-Main to TK-Spare Parts",
                                    type: "orange",
                                },
                                {
                                    time: "09:00",
                                    user: "Sara",
                                    action: "issued",
                                    quantity: "15",
                                    item: "units of Laptop Dell",
                                    location: "from TK-Electronics",
                                    type: "blue",
                                },
                                {
                                    time: "09:00",
                                    user: "Ahmed",
                                    action: "adjusted stock of",
                                    quantity: "",
                                    item: "Hydraulic Filter by -2 units",
                                    location: "in EO-Parts",
                                    type: "green",
                                },
                                {
                                    time: "09:00",
                                    user: "",
                                    action: "Stock count completed",
                                    quantity: "",
                                    item: "",
                                    location: "in EO-Fertilizers 1",
                                    type: "green",
                                },
                                {
                                    time: "09:00",
                                    user: "",
                                    action: "Stock count completed",
                                    quantity: "",
                                    item: "",
                                    location: "in EO-Fertilizers 1",
                                    type: "green",
                                },
                            ].map((activity, index, activities) => (
                                <div
                                    key={index}
                                    style={{
                                        display: "flex",
                                        width: "100%",
                                        minHeight: "66px",
                                    }}
                                >
                                    {/* Time */}
                                    <div
                                        style={{
                                            width: "32px",
                                            flexShrink: 0,
                                            paddingTop: "var(--space-1)",
                                        }}
                                    >
                                        <span
                                            className="body"
                                            style={{
                                                color: "var(--grey)",
                                                whiteSpace: "nowrap",
                                            }}
                                        >
                                            {activity.time}
                                        </span>
                                    </div>

                                    {/* Timeline */}
                                    <div
                                        style={{
                                            width: "18px",
                                            flexShrink: 0,
                                            position: "relative",
                                            display: "flex",
                                            justifyContent: "center",
                                        }}
                                    >
                                        {/* Vertical line */}
                                        {index < activities.length - 1 && (
                                            <div
                                                style={{
                                                    position: "absolute",
                                                    top: "10px",
                                                    bottom: "-10px",
                                                    width: "3px",
                                                    backgroundColor: "var(--light-grey)",
                                                }}
                                            />
                                        )}

                                        {/* Dot */}
                                        <div
                                            style={{
                                                position: "relative",
                                                zIndex: 1,
                                                width: "14px",
                                                height: "14px",
                                                marginTop: "4px",
                                                borderRadius: "50%",
                                                backgroundColor:
                                                    activity.type === "orange"
                                                        ? "var(--orange)"
                                                        : activity.type === "blue"
                                                            ? "var(--blue)"
                                                            : "var(--dark-green)",
                                            }}
                                        />
                                    </div>

                                    {/* Activity */}
                                    <div
                                        className="column-container"
                                        style={{
                                            flex: 1,
                                            minWidth: 0,
                                            paddingLeft: "var(--space-2)",
                                            paddingBottom:
                                                index < activities.length - 1
                                                    ? "var(--space-2)"
                                                    : "0",
                                            gap: "0",
                                        }}
                                    >
                                        <p
                                            className="body"
                                            style={{
                                                margin: 0,
                                                color: "var(--grey)",
                                                lineHeight: "var(--line-body)",
                                            }}
                                        >
                                            {activity.user && (
                                                <strong
                                                    style={{
                                                        color: "var(--midnight-blue)",
                                                    }}
                                                >
                                                    {activity.user}
                                                </strong>
                                            )}{" "}
                                            <span>{activity.action}</span>{" "}
                                            {activity.quantity && (
                                                <span>{activity.quantity} </span>
                                            )}
                                            {activity.item && <span>{activity.item}</span>}
                                        </p>

                                        <p
                                            className="body"
                                            style={{
                                                margin: 0,
                                                color: "var(--grey)",
                                                lineHeight: "var(--line-body)",
                                            }}
                                        >
                                            {activity.location}
                                        </p>
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}