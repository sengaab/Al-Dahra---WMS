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
        name: "Reports",
        href: "/reports",
        icon: "/nav/reports.svg",
    },
    {
        name: "Suppliers",
        href: "/suppliers",
        icon: "/nav/suppliers.svg",
    },
    {
        name: "Orders",
        href: "/orders",
        icon: "/nav/orders.svg",
    },
    {
        name: "Scan Product",
        href: "/scan",
        icon: "/nav/scan.svg",
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

            <h4
                style={{
                    color: active?"black":"var(--white)",
                }}
            >
                {name}
            </h4>
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

                    <h4
                        style={{
                            color: "var(--white)",
                        }}
                    >
                        Log Out
                    </h4>
                </button>
            </nav>
        </aside>
    );
}