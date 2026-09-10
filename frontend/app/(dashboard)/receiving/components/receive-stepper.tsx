"use client";

import { usePathname, useRouter } from "next/navigation";

const steps = [
    {
        number: 1,
        label: "Select PO",
        path: "/receiving/select-po",
    },
    {
        number: 2,
        label: "PO Lines",
        path: "/receiving/po-lines",
    },
    {
        number: 3,
        label: "Confirm Receipt",
        path: "/receiving/confirm-receipt",
    },
];

export default function ReceiveStepper() {
    const router = useRouter();
    const pathname = usePathname();

    const currentStep =
        pathname === "/receiving/select-po"
            ? 1
            : pathname === "/receiving/po-lines"
                ? 2
                : pathname === "/receiving/confirm-receipt"
                    ? 3
                    : 1;

    return (
        <div
            className="card row-container"
            style={{
                width: "100%",
                justifyContent: "space-between",
                alignItems: "center",
            }}
        >
            <div
                className="row-container"
                style={{
                    alignItems: "center",
                    gap: "24px",
                }}
            >
                {steps.map((step, index) => {
                    const isCurrent = step.number === currentStep;
                    const isCompleted = step.number < currentStep;

                    return (
                        <div
                            key={step.number}
                            className="row-container"
                            style={{
                                alignItems: "center",
                            }}
                        >
                            <button
                                type="button"
                                onClick={() => router.push(step.path)}
                                style={{
                                    display: "flex",
                                    alignItems: "center",
                                    gap: "10px",
                                    padding: 0,
                                    border: "none",
                                    background: "transparent",
                                    cursor: "pointer",
                                }}
                            >
                                <div
                                    className="body-title"
                                    style={{
                                        width: "24px",
                                        height: "24px",
                                        borderRadius: "50%",
                                        display: "flex",
                                        alignItems: "center",
                                        justifyContent: "center",
                                        backgroundColor: isCurrent
                                            ? "var(--dark-green)"
                                            : isCompleted
                                                ? "var(--light-green)"
                                                : "var(--light-grey)",
                                        color: isCurrent
                                            ? "var(--beige)"
                                            : "var(--midnight-blue)",
                                    }}
                                >
                                    {step.number}
                                </div>

                                <span
                                    className="body-title"
                                    style={{
                                        whiteSpace: "nowrap",
                                    }}
                                >
                                    {step.label}
                                </span>
                            </button>

                            {index < steps.length - 1 && (
                                <img
                                 src = "/next.svg"
                                />
                            )}
                        </div>
                    );
                })}
            </div>
        </div>
    );
}