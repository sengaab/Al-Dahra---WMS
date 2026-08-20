"use client";

import { Fragment, useState } from "react";
import Dropdown from "@/components/dropdown";
import Card from "@/components/card";
import StockStatusChart from "@/components/stockstatuschart";

const stats = [
    {
        title: "TOTAL SKUS",
        value: "18,000",
        color: "var(--dark-green)",
    },
    {
        title: "STOCK UNITS",
        value: "1,280,000",
        color: "var(--dark-green)",
    },
    {
        title: "STOCK VALUE",
        value: "$100,000,000",
        color: "var(--dark-green)",
    },
    {
        title: "LOW STOCK",
        value: "10",
        color: "var(--blood-red)",
    },
];

const incoming = [
    {
        ref: "PO-10231",
        supplier: "Supplier A",
        units: 1200,
        expectedDate: "Aug 18, 2026",
        status: "In Transit",
        statusColor: "var(--blue)",
    },
    {
        ref: "PO-10232",
        supplier: "Supplier A",
        units: 1200,
        expectedDate: "Aug 18, 2026",
        status: "Pending",
        statusColor: "var(--orange)",
    },
];

const activities = [
    {
        time: "09:00",
        user: "Ahmed",
        action: "received",
        quantity: "1,200 units",
        item: "of fertilizer X",
        location: "TK - Fertilizers 1",
        color: "var(--blue)",
    },
    {
        time: "09:00",
        user: "Maged",
        action: "transferred",
        quantity: "20 units",
        item: "of Bearing 6205",
        location: "from TK-Main to TK-Spare Parts",
        color: "var(--orange)",
    },
    {
        time: "09:00",
        user: "Sara",
        action: "issued",
        quantity: "15 units",
        item: "of Laptop Dell",
        location: "from TK-Electronics",
        color: "var(--blood-red)",
    },
    {
        time: "09:00",
        user: "Ahmed",
        action: "adjusted stock",
        quantity: "by -2 units",
        item: "of Hydraulic Filter",
        location: "in EO-Parts",
        color: "var(--dahra-green)",
    },
    {
        time: "09:00",
        user: "",
        action: "Stock count completed",
        quantity: "",
        item: "",
        location: "in EO-Fertilizers 1",
        color: "var(--midnight-blue)",
    },
];

const stockStatus = [
    { name: "Available", value: 70, color: "var(--dark-green)" },
    { name: "Reserved", value: 10, color: "var(--green-2)" },
    { name: "Damaged", value: 10, color: "var(--blood-red)" },
    { name: "Expired", value: 8, color: "var(--orange)" },
    { name: "Quarantined", value: 2, color: "var(--blue)" },
];

const sites = [
    {
        label: "Toshka",
        value: "tk",
        warehouses: [
            {
                label: "TK-1",
                value: "tk-1",
                skus: "9K",
                units: "1.1m",
                occupancy: 75,
                status: "Good",
            },
            {
                label: "TK-2",
                value: "tk-2",
                skus: "9K",
                units: "1.0m",
                occupancy: 85,
                status: "Good",
            },
        ],
    },
    {
        label: "Eoinat",
        value: "eo",
        warehouses: [
            {
                label: "EO-1",
                value: "eo-1",
                skus: "6K",
                units: "750K",
                occupancy: 65,
                status: "Good",
            },
            {
                label: "EO-2",
                value: "eo-2",
                skus: "4K",
                units: "500K",
                occupancy: 70,
                status: "Good",
            },
        ],
    },
];

const siteOptions = [
    {
        label: "All",
        value: "all",
    },
    ...sites.map((site) => ({
        label: site.label,
        value: site.value,
    })),
];

const departments = [
    {
        label: "People & Culture",
        value: "people-culture",
    },
    {
        label: "QHSSE",
        value: "qhsse",
    },
    {
        label: "Farming",
        value: "farming",
    },
    {
        label: "Workshop",
        value: "workshop",
    },
    {
        label: "Lab",
        value: "lab",
    },
];

const categoryValues = [
    {
        name: "Mechanical",
        value: 2.3,
        display: "$2.3M",
        color: "var(--green-1)",
    },
    {
        name: "Hydraulic",
        value: 1.8,
        display: "$1.8M",
        color: "var(--green-2)",
    },
    {
        name: "Electrical",
        value: 1.3,
        display: "$1.3M",
        color: "var(--green-3)",
    },
    {
        name: "Pumps",
        value: 0.9,
        display: "$900K",
        color: "var(--green-4)",
    },
    {
        name: "Other",
        value: 0.8,
        display: "$800K",
        color: "var(--grey)",
    },
];

const incomingStats = [
    {
        label: "Expected Today",
        value: 5,
    },
    {
        label: "Expected This Week",
        value: 5,
    },
    {
        label: "Pending Receiving",
        value: 5,
    },
    {
        label: "In Transit",
        value: 5,
    },
];

const maxCategoryValue = Math.max(
    ...categoryValues.map((item) => item.value)
);

export default function Dashboard() {
    const [site, setSite] = useState("all");
    const [department, setDepartment] = useState("");

    const [siteOpen, setSiteOpen] = useState({
        tk: true,
        eo: true,
    });

    const selectedSite = sites.find(
        (item) => item.value === site
    );

    const toggleSite = (siteValue: string) => {
        setSiteOpen((prev) => ({
            ...prev,
            [siteValue]: !prev[siteValue as keyof typeof prev],
        }));
    };

    return (
        <div
            style={{
                width: "100%",
                minHeight: "100vh",
                display: "flex",
                flexDirection: "column",
                padding: "var(--content-padding)",
                boxSizing: "border-box",
                gap: "var(--content-gap)",
            }}
        >
            {/* =========================================
                TOP SECTION
            ========================================= */}

            <div
                style={{
                    display: "flex",
                    alignItems: "stretch",
                    justifyContent: "flex-start",
                    flexWrap: "wrap",
                    gap: "var(--content-gap)",
                    width: "100%",
                }}
            >
                {/* Filters */}
                <div
                    style={{
                        backgroundColor: "var(--dahra-green)",
                        padding: "var(--card-padding)",
                        borderRadius: "var(--radius-md)",
                        boxShadow: "var(--shadow-md)",
                        width: "var(--card-md-width)",
                        minHeight: "var(--card-sm-height)",
                        flex: "1 1 var(--card-md-width)",
                        boxSizing: "border-box",
                        display: "flex",
                        flexDirection: "column",
                        justifyContent: "center",
                        gap: "var(--space-4)",
                    }}
                >
                    {/* Site */}
                    <div
                        style={{
                            display: "flex",
                            alignItems: "center",
                            flexWrap: "nowrap",
                            width: "100%",
                            gap: "var(--space-3)",
                        }}
                    >
                        <h3
                            style={{
                                color: "white",
                                margin: 0,
                                whiteSpace: "nowrap",
                            }}
                        >
                            SITE
                        </h3>

                        <Dropdown
                            placeholder="Select a Site"
                            value={site}
                            onChange={setSite}
                            options={siteOptions}
                            width="100%"
                        />
                    </div>

                    {/* Department */}
                    <div
                        style={{
                            display: "flex",
                            alignItems: "center",
                            flexWrap: "nowrap",
                            width: "100%",
                            gap: "var(--space-3)",
                        }}
                    >
                        <h3
                            style={{
                                color: "white",
                                margin: 0,
                                whiteSpace: "nowrap",
                            }}
                        >
                            DEPARTMENT
                        </h3>

                        <Dropdown
                            placeholder="Select a Department"
                            value={department}
                            onChange={setDepartment}
                            options={departments}
                            width="100%"
                        />
                    </div>
                </div>

                {/* Statistics Cards */}
                {stats.map((stat) => (
                    <div
                        key={stat.title}
                        style={{
                            backgroundColor: "white",
                            padding: "var(--card-padding)",
                            borderRadius: "var(--radius-md)",
                            boxShadow: "var(--shadow-md)",
                            width: "var(--card-sm-width)",
                            minHeight: "var(--card-sm-height)",
                            flex: "1 1 var(--card-sm-width)",
                            boxSizing: "border-box",
                            display: "flex",
                            flexDirection: "column",
                            justifyContent: "space-between",
                        }}
                    >
                        <h3
                            style={{
                                margin: 0,
                            }}
                        >
                            {stat.title}
                        </h3>

                        <h5
                            style={{
                                margin: 0,
                                color: stat.color,
                            }}
                        >
                            {stat.value}
                        </h5>
                    </div>
                ))}
            </div>

            {/* =========================================
                CHARTS SECTION
            ========================================= */}

            <div
                style={{
                    width: "100%",
                    display: "flex",
                    flexWrap: "wrap",
                    gap: "var(--content-gap)",
                }}
            >
                {/* Value by Category */}
                <Card
                    title="Value by Category"
                    dropdown
                    dropdownPlaceholder="Department"
                    dropdownValue={department}
                    dropdownOnChange={setDepartment}
                    dropdownOptions={departments}
                    dropdownWidth="60%"
                    viewAll
                    viewAllLabel="View all Categories"
                    onViewAll={() =>
                        console.log("Categories clicked")
                    }
                >
                    <div
                        style={{
                            width: "100%",
                            display: "flex",
                            flexDirection: "column",
                            gap: "var(--space-3)",
                            height: "100%",
                            justifyContent: "center",
                        }}
                    >
                        {categoryValues.map((item) => (
                            <div
                                key={item.name}
                                style={{
                                    display: "grid",
                                    gridTemplateColumns:
                                        "90px minmax(0, 1fr) 65px",
                                    alignItems: "center",
                                    gap: "var(--space-3)",
                                }}
                            >
                                <h6
                                    style={{
                                        margin: 0,
                                        whiteSpace: "nowrap",
                                    }}
                                >
                                    {item.name}
                                </h6>

                                <div
                                    style={{
                                        height: "20px",
                                        width: "100%",
                                        borderRadius:
                                            "var(--radius-sm)",
                                        backgroundColor:
                                            "var(--grey-1)",
                                        overflow: "hidden",
                                    }}
                                >
                                    <div
                                        style={{
                                            height: "100%",
                                            width: `${(item.value /
                                                maxCategoryValue) *
                                                100
                                                }%`,
                                            borderRadius:
                                                "var(--radius-sm)",
                                            backgroundColor:
                                                item.color,
                                        }}
                                    />
                                </div>

                                <h4
                                    style={{
                                        margin: 0,
                                        textAlign: "right",
                                        whiteSpace: "nowrap",
                                    }}
                                >
                                    {item.display}
                                </h4>
                            </div>
                        ))}
                    </div>
                </Card>

                {/* Warehouse Overview */}
                <Card
                    title="Warehouse Overview"
                    viewAll
                    viewAllLabel="View all Warehouses"
                    onViewAll={() =>
                        console.log("Warehouses clicked")
                    }
                >
                    <div>
                        <table
                            style={{
                                width: "100%",
                                borderCollapse: "collapse",
                            }}
                        >
                            <thead>
                                <tr>
                                    <th
                                        className="body-title"
                                        style={{
                                            textAlign: "left",
                                        }}
                                    >
                                        Warehouse
                                    </th>

                                    <th
                                        className="body-title"
                                        style={{
                                            textAlign: "center",
                                        }}
                                    >
                                        SKUs
                                    </th>

                                    <th
                                        className="body-title"
                                        style={{
                                            textAlign: "center",
                                        }}
                                    >
                                        Units
                                    </th>

                                    <th
                                        className="body-title"
                                        style={{
                                            textAlign: "center",
                                        }}
                                    >
                                        Occupancy
                                    </th>

                                    <th
                                        className="body-title"
                                        style={{
                                            textAlign: "center",
                                        }}
                                    >
                                        Stock Status
                                    </th>
                                </tr>

                                <tr>
                                    <td colSpan={5}>
                                        <div
                                            style={{
                                                width: "100%",
                                                height: "2px",
                                                backgroundColor:
                                                    "var(--light-grey)",
                                            }}
                                        />
                                    </td>
                                </tr>
                            </thead>

                            <tbody>
                                {/* =================================
                                    ALL SITES
                                ================================= */}

                                {site === "all" &&
                                    sites.map((siteItem) => (
                                        <Fragment key={siteItem.value}>
                                            <tr>
                                                <td className="body-subtitle">
                                                    <button
                                                        onClick={() =>
                                                            toggleSite(
                                                                siteItem.value
                                                            )
                                                        }
                                                        style={{
                                                            border: "none",
                                                            background:
                                                                "none",
                                                            cursor: "pointer",
                                                            display:
                                                                "flex",
                                                            alignItems:
                                                                "center",
                                                            gap: "8px",
                                                            padding: 0,
                                                        }}
                                                    >
                                                        <span
                                                            style={{
                                                                display:
                                                                    "inline-block",
                                                                transform:
                                                                    siteOpen[
                                                                        siteItem
                                                                            .value as keyof typeof siteOpen
                                                                    ]
                                                                        ? "rotate(90deg)"
                                                                        : "rotate(0deg)",
                                                                transition:
                                                                    "transform 0.2s ease",
                                                            }}
                                                        >
                                                            ›
                                                        </span>

                                                        {siteItem.label.toUpperCase()}
                                                    </button>
                                                </td>

                                                <td
                                                    className="body-subtitle"
                                                    style={{
                                                        textAlign:
                                                            "center",
                                                    }}
                                                >
                                                    18K
                                                </td>

                                                <td
                                                    className="body-subtitle"
                                                    style={{
                                                        textAlign:
                                                            "center",
                                                    }}
                                                >
                                                    2.1m
                                                </td>

                                                <td
                                                    className="body-subtitle"
                                                    style={{
                                                        textAlign:
                                                            "center",
                                                    }}
                                                >
                                                    <div
                                                        style={{
                                                            width: "100%",
                                                            backgroundColor:
                                                                "var(--light-grey)",
                                                            height: "10px",
                                                            borderRadius:
                                                                "var(--radius-md)",
                                                        }}
                                                    >
                                                        <div
                                                            style={{
                                                                height: "100%",
                                                                width: "80%",
                                                                backgroundColor:
                                                                    "var(--dark-green)",
                                                                borderRadius:
                                                                    "var(--radius-md)",
                                                            }}
                                                        />
                                                    </div>
                                                </td>

                                                <td
                                                    className="body-subtitle"
                                                    style={{
                                                        textAlign:
                                                            "center",
                                                    }}
                                                >
                                                    Good
                                                </td>
                                            </tr>

                                            {/* WAREHOUSES */}
                                            {siteOpen[
                                                siteItem
                                                    .value as keyof typeof siteOpen
                                            ] &&
                                                siteItem.warehouses.map(
                                                    (warehouse) => (
                                                        <tr
                                                            key={
                                                                warehouse.value
                                                            }
                                                        >
                                                            <td
                                                                className="body"
                                                                style={{
                                                                    paddingLeft:
                                                                        "32px",
                                                                }}
                                                            >
                                                                {
                                                                    warehouse.label
                                                                }
                                                            </td>

                                                            <td
                                                                className="body"
                                                                style={{
                                                                    textAlign:
                                                                        "center",
                                                                }}
                                                            >
                                                                {
                                                                    warehouse.skus
                                                                }
                                                            </td>

                                                            <td
                                                                className="body"
                                                                style={{
                                                                    textAlign:
                                                                        "center",
                                                                }}
                                                            >
                                                                {
                                                                    warehouse.units
                                                                }
                                                            </td>

                                                            <td
                                                                className="body"
                                                                style={{
                                                                    textAlign:
                                                                        "center",
                                                                }}
                                                            >
                                                                <div
                                                                    style={{
                                                                        width: "100%",
                                                                        backgroundColor:
                                                                            "var(--light-grey)",
                                                                        height: "10px",
                                                                        borderRadius:
                                                                            "var(--radius-md)",
                                                                    }}
                                                                >
                                                                    <div
                                                                        style={{
                                                                            height: "100%",
                                                                            width: `${warehouse.occupancy}%`,
                                                                            backgroundColor:
                                                                                "var(--dark-green)",
                                                                            borderRadius:
                                                                                "var(--radius-md)",
                                                                        }}
                                                                    />
                                                                </div>
                                                            </td>

                                                            <td
                                                                className="body"
                                                                style={{
                                                                    textAlign:
                                                                        "center",
                                                                }}
                                                            >
                                                                {
                                                                    warehouse.status
                                                                }
                                                            </td>
                                                        </tr>
                                                    )
                                                )}
                                        </Fragment>
                                    ))}

                                {/* =================================
                                    SINGLE SITE
                                ================================= */}

                                {site !== "all" &&
                                    selectedSite && (
                                        <>
                                            {/* SITE ROW */}
                                            <tr>
                                                <td className="body-subtitle">
                                                    <button
                                                        onClick={() =>
                                                            toggleSite(
                                                                selectedSite.value
                                                            )
                                                        }
                                                        style={{
                                                            border: "none",
                                                            background:
                                                                "none",
                                                            cursor: "pointer",
                                                            display:
                                                                "flex",
                                                            alignItems:
                                                                "center",
                                                            gap: "8px",
                                                            padding: 0,
                                                        }}
                                                    >
                                                        <span
                                                            style={{
                                                                display:
                                                                    "inline-block",
                                                                transform:
                                                                    siteOpen[
                                                                        selectedSite
                                                                            .value as keyof typeof siteOpen
                                                                    ]
                                                                        ? "rotate(90deg)"
                                                                        : "rotate(0deg)",
                                                                transition:
                                                                    "transform 0.2s ease",
                                                            }}
                                                        >
                                                            ›
                                                        </span>

                                                        {selectedSite.label.toUpperCase()}
                                                    </button>
                                                </td>

                                                <td
                                                    className="body-subtitle"
                                                    style={{
                                                        textAlign:
                                                            "center",
                                                    }}
                                                >
                                                    18K
                                                </td>

                                                <td
                                                    className="body-subtitle"
                                                    style={{
                                                        textAlign:
                                                            "center",
                                                    }}
                                                >
                                                    2.1m
                                                </td>

                                                <td
                                                    className="body-subtitle"
                                                    style={{
                                                        textAlign:
                                                            "center",
                                                    }}
                                                >
                                                    <div
                                                        style={{
                                                            width: "100%",
                                                            backgroundColor:
                                                                "var(--light-grey)",
                                                            height: "10px",
                                                            borderRadius:
                                                                "var(--radius-md)",
                                                        }}
                                                    >
                                                        <div
                                                            style={{
                                                                height: "100%",
                                                                width: "80%",
                                                                backgroundColor:
                                                                    "var(--dark-green)",
                                                                borderRadius:
                                                                    "var(--radius-md)",
                                                            }}
                                                        />
                                                    </div>
                                                </td>

                                                <td
                                                    className="body-subtitle"
                                                    style={{
                                                        textAlign:
                                                            "center",
                                                    }}
                                                >
                                                    Good
                                                </td>
                                            </tr>

                                            {/* WAREHOUSES */}
                                            {siteOpen[
                                                selectedSite
                                                    .value as keyof typeof siteOpen
                                            ] &&
                                                selectedSite.warehouses.map(
                                                    (warehouse) => (
                                                        <tr
                                                            key={
                                                                warehouse.value
                                                            }
                                                        >
                                                            <td
                                                                className="body"
                                                                style={{
                                                                    paddingLeft:
                                                                        "32px",
                                                                }}
                                                            >
                                                                {
                                                                    warehouse.label
                                                                }
                                                            </td>

                                                            <td
                                                                className="body"
                                                                style={{
                                                                    textAlign:
                                                                        "center",
                                                                }}
                                                            >
                                                                {
                                                                    warehouse.skus
                                                                }
                                                            </td>

                                                            <td
                                                                className="body"
                                                                style={{
                                                                    textAlign:
                                                                        "center",
                                                                }}
                                                            >
                                                                {
                                                                    warehouse.units
                                                                }
                                                            </td>

                                                            <td
                                                                className="body"
                                                                style={{
                                                                    textAlign:
                                                                        "center",
                                                                }}
                                                            >
                                                                <div
                                                                    style={{
                                                                        width: "100%",
                                                                        backgroundColor:
                                                                            "var(--light-grey)",
                                                                        height: "10px",
                                                                        borderRadius:
                                                                            "var(--radius-md)",
                                                                    }}
                                                                >
                                                                    <div
                                                                        style={{
                                                                            height: "100%",
                                                                            width: `${warehouse.occupancy}%`,
                                                                            backgroundColor:
                                                                                "var(--dark-green)",
                                                                            borderRadius:
                                                                                "var(--radius-md)",
                                                                        }}
                                                                    />
                                                                </div>
                                                            </td>

                                                            <td
                                                                className="body"
                                                                style={{
                                                                    textAlign:
                                                                        "center",
                                                                }}
                                                            >
                                                                {
                                                                    warehouse.status
                                                                }
                                                            </td>
                                                        </tr>
                                                    )
                                                )}
                                        </>
                                    )}

                                {/* NO DATA */}
                                {!selectedSite &&
                                    site !== "all" && (
                                        <tr>
                                            <td
                                                colSpan={5}
                                                className="body"
                                                style={{
                                                    textAlign:
                                                        "center",
                                                    padding:
                                                        "var(--space-5)",
                                                    color: "var(--grey)",
                                                }}
                                            >
                                                No site selected
                                            </td>
                                        </tr>
                                    )}
                            </tbody>
                        </table>
                    </div>
                </Card>

                {/* Stock Status */}
                <Card
                    title="Stock Status"
                    viewAll
                    viewAllLabel="View all Status"
                    onViewAll={() =>
                        console.log("Status clicked")
                    }
                >
                    <StockStatusChart stockStatus={stockStatus} />

                </Card>

                {/* Incoming Stock */}
                <Card
                    title="Incoming Stock"
                    viewAll
                    viewAllPosition="header"
                    viewAllLabel="View all Incoming"
                    onViewAll={() =>
                        console.log("Stock clicked")
                    }
                    flex={2}
                >
                    <div
                        style={{
                            width: "100%",
                            display: "flex",
                            flexWrap: "wrap",
                            gap: "var(--space-3)",
                        }}
                    >
                        {incomingStats.map((stat) => (
                            <div
                                key={stat.label}
                                style={{
                                    minWidth: "var(--card-xs-width)",
                                    flex: 1,
                                    height: "var(--card-xs-height)",
                                    border: "1px solid var(--light-grey)",
                                    borderRadius: "var(--radius-sm)",
                                    display: "flex",
                                    flexDirection: "column",
                                    alignItems: "center",
                                    justifyContent: "space-evenly",
                                }}
                            >
                                <h6 style={{ margin: 0 }}>
                                    {stat.label}
                                </h6>

                                <h4 style={{ margin: 0 }}>
                                    {stat.value}
                                </h4>
                            </div>
                        ))}

                    </div>
                    <div>
                        <table
                            style={{
                                width: "100%",
                                borderCollapse: "collapse",
                                marginTop: "var(--space-3)",
                            }}
                        >
                            <thead>
                                <tr>
                                    <th
                                        className="body-title"
                                        style={{ textAlign: "left" }}
                                    >
                                        PO/Ref
                                    </th>

                                    <th
                                        className="body-title"
                                        style={{ textAlign: "center" }}
                                    >
                                        Supplier
                                    </th>

                                    <th
                                        className="body-title"
                                        style={{ textAlign: "center" }}
                                    >
                                        Units
                                    </th>

                                    <th
                                        className="body-title"
                                        style={{ textAlign: "center" }}
                                    >
                                        Expected Date
                                    </th>

                                    <th
                                        className="body-title"
                                        style={{ textAlign: "center" }}
                                    >
                                        Status
                                    </th>
                                </tr>

                                <tr>
                                    <td colSpan={5}>
                                        <div
                                            style={{
                                                width: "100%",
                                                height: "2px",
                                                backgroundColor: "var(--light-grey)",
                                            }}
                                        />
                                    </td>
                                </tr>
                            </thead>

                            <tbody>
                                {incoming.map((item) => (
                                    <tr key={item.ref}>
                                        <td
                                            className="body"
                                            style={{ textAlign: "left" }}
                                        >
                                            {item.ref}
                                        </td>

                                        <td
                                            className="body"
                                            style={{ textAlign: "center" }}
                                        >
                                            {item.supplier}
                                        </td>

                                        <td
                                            className="body"
                                            style={{ textAlign: "center" }}
                                        >
                                            {item.units}
                                        </td>

                                        <td
                                            className="body"
                                            style={{ textAlign: "center" }}
                                        >
                                            {item.expectedDate}
                                        </td>

                                        <td
                                            className="body-subtitle"
                                            style={{
                                                textAlign: "center",
                                                color: item.statusColor,
                                            }}
                                        >
                                            {item.status}
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>

                </Card>

                {/* Recent Activity */}
                <Card
                    title="Recent Activity"
                    viewAll
                    viewAllPosition="footer"
                    viewAllLabel="View all Activities"
                    onViewAll={() =>
                        console.log("Activity clicked")
                    }
                    overflow
                >


                    <div
                        style={{
                            display: "flex",
                            flexDirection: "column",
                            gap: "var(--space-3)",
                            justifyContent:"flex-start"
                        }}
                    >
                        {activities.map((activity, index) => (
                            <div
                                key={index}
                                style={{
                                    display: "grid",
                                    gridTemplateColumns: "70px 20px 1fr",
                                    columnGap: "var(--space-3)",
                                }}
                            >
                                {/* Time */}
                                <div
                                    className="body"
                                    style={{
                                        textAlign: "left",
                                        paddingTop: "2px",
                                    }}
                                >
                                    {activity.time}
                                </div>

                                {/* Timeline */}
                                <div
                                    style={{
                                        position: "relative",
                                        display: "flex",
                                        justifyContent: "center",
                                    }}
                                >
                                    <div
                                        style={{
                                            width: "20px",
                                            height: "20px",
                                            borderRadius: "50%",
                                            backgroundColor: activity.color,
                                            position: "relative",
                                            zIndex: 2,
                                        }}
                                    />

                                    {index < activities.length - 1 && (
                                        <div
                                            style={{
                                                position: "absolute",
                                                top: "20px",
                                                bottom: "-var(--space-4)",
                                                width: "3px",
                                                backgroundColor: "var(--light-grey)",
                                            }}
                                        />
                                    )}
                                </div>

                                {/* Activity */}
                                <div
                                    className="body"
                                    style={{
                                        lineHeight: "1.6",
                                    }}
                                >
                                    {activity.user && (
                                        <strong>{activity.user}</strong>
                                    )}{" "}
                                    {activity.action}{" "}
                                    {activity.quantity && (
                                        <span>{activity.quantity} </span>
                                    )}
                                    {activity.item}{" "}
                                    <strong>{activity.location}</strong>
                                </div>
                            </div>
                        ))}
                    </div>

                </Card>
            </div>
        </div>
    );
}