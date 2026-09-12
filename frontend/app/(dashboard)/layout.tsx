"use client";

import { useEffect, useState } from "react";
import { usePathname, useRouter } from "next/navigation";

import SearchBar from "@/components/searchbar";
import Sidebar from "@/components/sidebar";
import { searchProducts } from "@/lib/api/products";
import type { ProductDto } from "@/types/product";

const pageTitles: Record<string, string> = {
    "/dashboard": "Operations Dashboard",
    "/inventory": "Inventory",
    "/locations": "Loactions",
    "/stock-counts": "Stock Counts",
    "/transfers": "Transfers",
    "/stock-issues": "Stock Issues",
    "/returns": "Returns",
    "/procurement": "Procurement",
    "/purchase-orders": "Purchase Orders",
    "/suppliers": "Suppliers",
    "/receiving": "Receiving",
    "/receiving/select-po": "Receiving / Select PO",
    "/receiving/po-lines": "Receiving / PO Lines",
    "/receiving/confirm-receipt": "Receiving / Confirm Receipt",
    "/putaway": "Putaway",
    "/requests": "Requests",
    "/picking": "Picking",
    "/scan": "Barcode Scanner",
    "/reports": "Reports",
    "/audit-logs": "Audit Logs",
    "/settings": "Settings",
    "/inventory/product": "Inventory / Products",
};

export default function DashboardLayout({
    children,
}: {
    children: React.ReactNode;
}) {
    const pathname = usePathname();
    const router = useRouter();

    const [searchValue, setSearchValue] = useState("");
    const [searchResults, setSearchResults] = useState<ProductDto[]>(
        []
    );
    const [isSearching, setIsSearching] = useState(false);
    const [searchError, setSearchError] = useState(false);

    const PageTitle =
        pageTitles[pathname] || "Operations Dashboard";

    useEffect(() => {
        const query = searchValue.trim();

        // Don't search when empty
        if (!query) {
            setSearchResults([]);
            setIsSearching(false);
            setSearchError(false);
            return;
        }

        const timeoutId = setTimeout(async () => {
            try {
                setIsSearching(true);
                setSearchError(false);

                const results = await searchProducts(query);

                setSearchResults(results);
            } catch (error) {
                console.error(
                    "Failed to search products:",
                    error
                );

                setSearchResults([]);
                setSearchError(true);
            } finally {
                setIsSearching(false);
            }
        }, 300);

        return () => {
            clearTimeout(timeoutId);
        };
    }, [searchValue]);

    const handleProductClick = (product: ProductDto) => {
        setSearchValue("");
        setSearchResults([]);

        router.push(
            `/inventory/product?productId=${product.productId}`
        );
    };

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

                        {/* Search */}
                        <div
                            style={{
                                position: "relative",
                                flex: 1,
                                minWidth: 0,
                            }}
                        >
                            <SearchBar
                                placeholder="Search products, SKU, barcode or QR code..."
                                value={searchValue}
                                onChange={setSearchValue}
                            />

                            {/* Search dropdown */}
                            {searchValue.trim() && (
                                <div
                                    style={{
                                        position: "absolute",
                                        top: "calc(100% + var(--space-2))",
                                        left: 0,
                                        right: 0,
                                        backgroundColor:
                                            "var(--white)",
                                        border:
                                            "var(--border-default)",
                                        borderRadius:
                                            "var(--radius-md)",
                                        boxShadow:
                                            "var(--shadow-md)",
                                        maxHeight: "400px",
                                        overflowY: "auto",
                                        zIndex: 1100,
                                    }}
                                >
                                    {/* Loading */}
                                    {isSearching && (
                                        <div
                                            style={{
                                                padding:
                                                    "var(--space-4)",
                                                color:
                                                    "var(--grey)",
                                            }}
                                        >
                                            Searching...
                                        </div>
                                    )}

                                    {/* Error */}
                                    {!isSearching &&
                                        searchError && (
                                            <div
                                                style={{
                                                    padding:
                                                        "var(--space-4)",
                                                    color:
                                                        "var(--blood-red)",
                                                }}
                                            >
                                                Failed to search
                                                products.
                                            </div>
                                        )}

                                    {/* No results */}
                                    {!isSearching &&
                                        !searchError &&
                                        searchResults.length ===
                                            0 && (
                                            <div
                                                style={{
                                                    padding:
                                                        "var(--space-4)",
                                                    color:
                                                        "var(--grey)",
                                                }}
                                            >
                                                No products found.
                                            </div>
                                        )}

                                    {/* Results */}
                                    {!isSearching &&
                                        !searchError &&
                                        searchResults.map(
                                            (product) => (
                                                <button
                                                    key={
                                                        product.productId
                                                    }
                                                    type="button"
                                                    onClick={() =>
                                                        handleProductClick(
                                                            product
                                                        )
                                                    }
                                                    style={{
                                                        width: "100%",
                                                        display:
                                                            "flex",
                                                        flexDirection:
                                                            "column",
                                                        alignItems:
                                                            "flex-start",
                                                        gap: "4px",
                                                        padding:
                                                            "var(--space-3) var(--space-4)",
                                                        border: "none",
                                                        borderBottom:
                                                            "var(--border-default)",
                                                        backgroundColor:
                                                            "transparent",
                                                        cursor: "pointer",
                                                        textAlign:
                                                            "left",
                                                    }}
                                                >
                                                    {/* Product Name */}
                                                    <span
                                                        style={{
                                                            fontWeight:
                                                                500,
                                                        }}
                                                    >
                                                        {
                                                            product.name
                                                        }
                                                    </span>

                                                    {/* SKU */}
                                                    <span
                                                        style={{
                                                            fontSize:
                                                                "var(--text-sm)",
                                                            color:
                                                                "var(--grey)",
                                                        }}
                                                    >
                                                        SKU:{" "}
                                                        {
                                                            product.sku
                                                        }
                                                    </span>

                                                    {/* Barcode */}
                                                    {product.barcode && (
                                                        <span
                                                            style={{
                                                                fontSize:
                                                                    "var(--text-sm)",
                                                                color:
                                                                    "var(--grey)",
                                                            }}
                                                        >
                                                            Barcode:{" "}
                                                            {
                                                                product.barcode
                                                            }
                                                        </span>
                                                    )}

                                                    {/* QR Code */}
                                                    {product.qrValue && (
                                                        <span
                                                            style={{
                                                                fontSize:
                                                                    "var(--text-sm)",
                                                                color:
                                                                    "var(--grey)",
                                                            }}
                                                        >
                                                            QR:{" "}
                                                            {
                                                                product.qrValue
                                                            }
                                                        </span>
                                                    )}
                                                </button>
                                            )
                                        )}
                                </div>
                            )}
                        </div>
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