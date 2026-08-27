"use client";

import { useEffect, useState } from "react";
import { usePathname } from "next/navigation";
import SearchBar from "@/components/searchbar";
import Sidebar from "@/components/sidebar";

const pageTitles: Record<string, string> = {
    "/dashboard": "Operations Dashboard",
    "/inventory": "Inventory",
    "/stock-counts": "Stock Counts",
    "/transfers": "Transfers",
    "/stock-issues": "Stock Issues",
    "/returns": "Returns",
    "/procurement": "Procurement",
    "/purchase-orders": "Purchase Orders",
    "/suppliers": "Suppliers",
    "/receiving": "Receiving",
    "/putaway": "Putaway",
    "/requests": "Requests",
    "/picking": "Picking",
    "/scan": "Barcode Scanner",
    "/reports": "Reports",
    "/audit-logs": "Audit Logs",
    "/settings": "Settings",
};

export default function DashboardLayout({
    children,
}: {
    children: React.ReactNode;
}) {
    const pathname = usePathname();
    const [showHeader, setShowHeader] = useState(true);

    const PageTitle =
        pageTitles[pathname] ||
        "Operations Dashboard";

    return (
        <div
            style={{
                display: "flex",
                minHeight: "100vh",
                width: "100%",
                backgroundColor: "var(--beige)",
            }}
        >
            {/* Sidebar */}
            <Sidebar />

            {/* Main Content */}
            <main
                style={{
                    flex: 1,
                    minWidth: 0,
                    marginLeft: "var(--sidebar-width)",
                    minHeight: "100vh",
                    backgroundColor: "var(--beige)",
                    paddingTop: "var(--header-height)",
                    boxSizing: "border-box",
                }}
            >
                {/* Header */}
                <header
                    style={{
                        position: "fixed",
                        top: 0,
                        left: "var(--sidebar-width)",
                        right: 0,
                        backgroundColor: "var(--white)",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "space-between",
                        gap: "var(--space-6)",
                        padding:
                            "var(--space-3) var(--content-padding)",
                        boxSizing: "border-box",
                        zIndex: 1000,
                        height: "var(--header-height)",
                    }}
                >
                    {/* Left Side */}
                    <div
                        style={{
                            display: "flex",
                            alignItems: "center",
                            gap: "var(--space-5)",
                            minWidth: 0,
                            flex: 1,
                        }}
                    >
                        <p
                            className="page-title"
                            style={{
                                whiteSpace: "nowrap",
                                flexShrink: 0,
                            }}
                        >
                            {PageTitle}
                        </p>

                        <SearchBar
                            placeholder="Search products..."
                            onSearch={(value) => {
                                console.log(
                                    "Searching for:",
                                    value
                                );
                            }}
                        />
                    </div>

                    {/* Right Side */}
                    <div
                        style={{
                            display: "flex",
                            alignItems: "center",
                            gap: "var(--space-5)",
                            flexShrink: 0,
                        }}
                    >
                        {/* Notifications */}
                        <button
                            type="button"
                            style={{
                                display: "flex",
                                alignItems: "center",
                                justifyContent: "center",
                                width: "var(--icon-xl)",
                                height: "var(--icon-xl)",
                                padding: 0,
                                border: "none",
                                backgroundColor: "transparent",
                                cursor: "pointer",
                            }}
                        >
                            <img
                                src="/notification.svg"
                                alt="Notifications"
                                style={{
                                    width: "var(--icon-xl)",
                                    height: "var(--icon-xl)",
                                }}
                            />
                        </button>

                        {/* Profile */}
                        <button
                            type="button"
                            style={{
                                display: "flex",
                                alignItems: "center",
                                justifyContent: "center",
                                width: "var(--icon-xl)",
                                height: "var(--icon-xl)",
                                padding: 0,
                                border: "none",
                                backgroundColor: "transparent",
                                cursor: "pointer",
                            }}
                        >
                            <img
                                src="/profile.svg"
                                alt="Profile"
                                style={{
                                    width: "var(--icon-xl)",
                                    height: "var(--icon-xl)",
                                }}
                            />
                        </button>
                    </div>
                </header>

                {/* Page Content */}
                {children}
            </main>
        </div>
    );
}