// app/(dashboard)/layout.tsx
"use client";

import { useEffect, useState } from "react";
import SearchBar from "@/components/searchbar";
import Sidebar from "@/components/sidebar";

export default function DashboardLayout({
    children,
}: {
    children: React.ReactNode;
}) {
    const [showHeader, setShowHeader] = useState(true);

    useEffect(() => {
        let lastScrollY = window.scrollY;

        const handleScroll = () => {
            const currentScrollY = window.scrollY;

            if (currentScrollY <= 0) {
                setShowHeader(true);
            } else if (currentScrollY > lastScrollY) {
                // Scrolling down
                setShowHeader(false);
            } else {
                // Scrolling up
                setShowHeader(true);
            }

            lastScrollY = currentScrollY;
        };

        window.addEventListener("scroll", handleScroll);

        return () => {
            window.removeEventListener("scroll", handleScroll);
        };
    }, []);

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
                        height: "var(--header-height)",
                        backgroundColor: "var(--white)",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "space-between",
                        gap: "var(--space-6)",
                        padding: "0 var(--content-padding)",
                        boxSizing: "border-box",
                        zIndex: 1000,

                        transform: showHeader
                            ? "translateY(0)"
                            : "translateY(-100%)",

                        transition:
                            "transform 0.25s ease",
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
                        <h2
                            style={{
                                whiteSpace: "nowrap",
                                flexShrink: 0,
                            }}
                        >
                            Operations Dashboard
                        </h2>

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