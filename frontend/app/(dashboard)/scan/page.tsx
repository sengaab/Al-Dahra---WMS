"use client";

import { useEffect, useRef, useState } from "react";

export default function BarcodeScannerPage() {
    const inputRef = useRef<HTMLInputElement>(null);

    const [barcode, setBarcode] = useState("");
    const [lastScanned, setLastScanned] = useState("");
    const [status, setStatus] = useState("Ready to scan");

    useEffect(() => {
        inputRef.current?.focus();

        const keepFocus = () => {
            if (document.activeElement !== inputRef.current) {
                inputRef.current?.focus();
            }
        };

        window.addEventListener("click", keepFocus);

        return () => {
            window.removeEventListener("click", keepFocus);
        };
    }, []);

    const handleKeyDown = (
        e: React.KeyboardEvent<HTMLInputElement>
    ) => {
        if (e.key === "Enter") {
            e.preventDefault();

            const scannedBarcode = barcode.trim();

            if (!scannedBarcode) return;

            console.log("Barcode:", scannedBarcode);

            setLastScanned(scannedBarcode);
            setStatus("Barcode scanned successfully");
            setBarcode("");

            setTimeout(() => {
                inputRef.current?.focus();
            }, 50);
        }
    };

    return (
        <div>
            <input
                ref={inputRef}
                value={barcode}
                onChange={(e) => setBarcode(e.target.value)}
                onKeyDown={handleKeyDown}
                autoFocus
            />

            <p>{status}</p>

            {lastScanned && (
                <p>Last scanned: {lastScanned}</p>
            )}
        </div>
    );
}