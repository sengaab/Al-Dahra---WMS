"use client";

import Link from "next/link";
import { usePathname, useRouter } from "next/navigation";
import { createClient } from "@/lib/supabase/client";

const navItems = [
    {
        name: "Dashboard",
        href: "/dashboard",
        icon: "/nav/dashboard.svg",
    },
    {
        name: "Inventory",
        href: "/inventory",
        icon: "/nav/inventory.svg",
    },
    {
        name: "Stock Counts",
        href: "/stock-counts",
        icon: "/nav/stock-counts.svg",
    },
    {
        name: "Transfers",
        href: "/transfers",
        icon: "/nav/transfers.svg",
    },
    {
        name: "Stock Issues",
        href: "/stock-issues",
        icon: "/nav/stock-issues.svg",
    },
    {
        name: "Returns",
        href: "/returns",
        icon: "/nav/returns.svg",
    },
    {
        name: "Procurement",
        href: "/procurement",
        icon: "/nav/procurement.svg",
    },
    {
        name: "Purchase Orders",
        href: "/purchase-orders",
        icon: "/nav/purchase-orders.svg",
    },
    {
        name: "Suppliers",
        href: "/suppliers",
        icon: "/nav/suppliers.svg",
    },
    {
        name: "Receiving",
        href: "/receiving",
        icon: "/nav/receiving.svg",
    },
    {
        name: "Putaway",
        href: "/putaway",
        icon: "/nav/putaway.svg",
    },
    {
        name: "Requests",
        href: "/requests",
        icon: "/nav/requests.svg",
    },
    {
        name: "Picking",
        href: "/picking",
        icon: "/nav/picking.svg",
    },
    {
        name: "Barcode Scanner",
        href: "/scan",
        icon: "/nav/barcode-scanner.svg",
    },
    {
        name: "Reports",
        href: "/reports",
        icon: "/nav/reports.svg",
    },
    {
        name: "Audit Logs",
        href: "/audit-logs",
        icon: "/nav/audit-logs.svg",
    },
];

const bottomNavItems = [
    {
        name: "Settings",
        href: "/settings",
        icon: "/nav/settings.svg",
    },
];

function NavItem({
    name,
    href,
    icon,
    active,
}: {
    name: string;
    href: string;
    icon: string;
    active: boolean;
}) {
    return (
        <Link
            href={href}
            style={{
                width: "100%",
                display: "flex",
                alignItems: "center",
                gap: "var(--space-3)",
                padding: "var(--space-3) var(--space-4)",
                boxSizing: "border-box",
                textDecoration: "none",
                borderRadius: "var(--radius-md)",
                backgroundColor: active
                    ? "var(--lime-green)"
                    : "transparent",
                transition: "background-color var(--transition-fast)",
            }}
        >
            <img
                src={icon}
                alt=""
                style={{
                    width: "24px",
                    height: "24px",
                    flexShrink: 0,
                    filter:active?"invert(1)":"none",
                }}
            />

            <p
                className="nav-item"
                style={{
                    color: active?"black":"var(--white)",
                }}
            >
                {name}
            </p>
        </Link>
    );
}

export default function Sidebar() {
    const pathname = usePathname();
    const router = useRouter();
    const supabase = createClient();

    const handleLogout = async () => {
        const { error } = await supabase.auth.signOut();

        if (error) {
            console.error("Logout failed:", error);
            return;
        }

        router.push("/");
        router.refresh();
    };

    return (
        <aside
            style={{
                width: "var(--sidebar-width)",
                height: "100vh",
                position: "fixed",
                left: 0,
                top: 0,
                backgroundColor: "var(--midnight-blue)",
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
                padding: "var(--space-5)",
                boxSizing: "border-box",
                gap: "var(--space-10)",
                flexShrink: 0,
                overflowY:"auto",
                scrollbarWidth:"none",
            }}
        >
            {/* Logo */}
            <div
                style={{
                    width: "100%",
                    display: "flex",
                    justifyContent: "center",
                }}
            >
                <img
                    src="/logo.svg"
                    alt="WMS Logo"
                    style={{
                        maxWidth: "100%",
                        height: "auto",
                    }}
                />
            </div>

            {/* Main Navigation */}
            <nav
                style={{
                    width: "100%",
                    display: "flex",
                    flexDirection: "column",
                    gap: "var(--space-5)",
                }}
            >
                {navItems.map((item) => {
                    const isActive =
                        pathname === item.href ||
                        pathname.startsWith(`${item.href}/`);

                    return (
                        <NavItem
                            key={item.href}
                            name={item.name}
                            href={item.href}
                            icon={item.icon}
                            active={isActive}
                        />
                    );
                })}
            </nav>

            {/* Bottom Navigation */}
            <nav
                style={{
                    width: "100%",
                    marginTop: "auto",
                    display: "flex",
                    flexDirection: "column",
                    gap: "var(--space-4)",
                }}
            >
                {bottomNavItems.map((item) => {
                    const isActive =
                        pathname === item.href ||
                        pathname.startsWith(`${item.href}/`);

                    return (
                        <NavItem
                            key={item.href}
                            name={item.name}
                            href={item.href}
                            icon={item.icon}
                            active={isActive}
                        />
                    );
                })}

                {/* Logout */}
                <button
                    type="button"
                    onClick={handleLogout}
                    style={{
                        width: "100%",
                        display: "flex",
                        alignItems: "center",
                        gap: "var(--space-3)",
                        padding: "var(--space-3) var(--space-4)",
                        boxSizing: "border-box",
                        border: "none",
                        borderRadius: "var(--radius-md)",
                        backgroundColor: "transparent",
                        cursor: "pointer",
                        textAlign: "left",
                        fontFamily: "var(--font-roboto)",
                    }}
                >
                    <img
                        src="/nav/log out.svg"
                        alt=""
                        style={{
                            width: "24px",
                            height: "24px",
                            flexShrink: 0,
                        }}
                    />

                    <p
                        className="nav-item"
                        style={{
                            color: "var(--white)",
                        }}
                    >
                        Log Out
                    </p>
                </button>
            </nav>
        </aside>
    );
}