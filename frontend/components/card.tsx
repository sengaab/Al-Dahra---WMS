"use client";

import { ReactNode } from "react";

interface CardProps {
    title: string;
    header?: ReactNode;
    children: ReactNode;
    maxWidth?: string;
}

export default function Card({
    title,
    header,
    children,
    maxWidth = "none",
}: CardProps) {
    return (
        <div
            style={{
                width: "100%",
                minHeight: "100%",
                boxSizing: "border-box",
                backgroundColor:"var(--white)",
                borderRadius:"var(--radius-md)",
                border:"solid 1px var(--light-grey)",
                maxWidth: maxWidth,
            }}
        >
            {/* Header */}
            <header
                style={{
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "flex-start",
                    gap: "var(--space-3)",
                    height:"fit-content",
                    padding:"var(--space-3)",
                }}
            >
                <h1 className="body-title">
                    {title}
                </h1>

                {header && (
                    <div
                        style={{
                            display: "flex",
                            alignItems: "center",
                            gap: "var(--space-3)",
                            minWidth:0,
                            flex:1,
                        }}
                    >
                        {header}
                    </div>
                )}
            </header>

            {/* Main Content */}
            <main style={{ width: "100%" }}>
                {children}
            </main>
        </div>
    );
}