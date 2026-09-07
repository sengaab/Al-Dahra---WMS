"use client";

import { useEffect, useMemo, useState } from "react";
import { useRouter } from "next/navigation";

import Button from "@/components/button";
import StatsCard from "@/components/stats-card";
import Status from "@/components/status";

import { getSites } from "@/lib/api/sites";
import { getWarehouses } from "@/lib/api/warehouses";
import { getPartitions } from "@/lib/api/partitions";
import { getBins } from "@/lib/api/bins";
import { getStock } from "@/lib/api/stock";

import type { SiteDto } from "@/types/site";
import type { WarehouseDto } from "@/types/warehouse";
import type { PartitionDto } from "@/types/partition";
import type { BinDto } from "@/types/bin";
import type { StockDto } from "@/types/stock";

type LocationType =
    | "Site"
    | "Warehouse"
    | "Partition"
    | "Bin";

interface LocationNode {
    id: string;
    numericId: number;

    name: string;
    code?: string;

    type: LocationType;

    skus: number;
    units: number;

    occupancy?: string;

    skuCodes?: string[];

    children?: LocationNode[];
}

/* -------------------------------------------------------------------------- */
/* Helpers                                                                    */
/* -------------------------------------------------------------------------- */

function getStatusVariant(
    type: string
): "green" | "orange" | "yellow" | "blue" | "grey" {
    switch (type) {
        case "Site":
            return "green";

        case "Warehouse":
            return "orange";

        case "Partition":
            return "yellow";

        case "Bin":
            return "blue";

        case "Occupied":
            return "green";    

        case "Partially Occupied":
            return "yellow";    

        case "Empty":
            return "grey";    

        default:
            return "green";
    }
}

function formatNumber(value: number) {
    return value.toLocaleString();
}

function getUniqueSkus(stock: StockDto[]) {
    return new Set(
        stock
            .map((item) => item.sku)
            .filter(Boolean)
    );
}

function getTotalQuantity(stock: StockDto[]) {
    return stock.reduce(
        (total, item) =>
            total + item.quantity,
        0
    );
}

function getBinOccupancy(units: number) {
    return units > 0
        ? "Occupied"
        : "Empty";
}

function getParentOccupancy(
    children: LocationNode[]
) {
    if (children.length === 0) {
        return "Empty";
    }

    const occupiedChildren =
        children.filter(
            (child) =>
                child.occupancy ===
                "Occupied" ||
                child.occupancy ===
                "Partially Occupied"
        ).length;

    if (occupiedChildren === 0) {
        return "Empty";
    }

    if (
        occupiedChildren ===
        children.length
    ) {
        return "Occupied";
    }

    return "Partially Occupied";
}

/* -------------------------------------------------------------------------- */
/* Component                                                                  */
/* -------------------------------------------------------------------------- */

export default function Locations() {
    const router = useRouter();

    const [sites, setSites] = useState<SiteDto[]>(
        []
    );

    const [warehouses, setWarehouses] =
        useState<WarehouseDto[]>([]);

    const [partitions, setPartitions] =
        useState<PartitionDto[]>([]);

    const [bins, setBins] = useState<BinDto[]>(
        []
    );

    const [stock, setStock] = useState<StockDto[]>(
        []
    );

    const [expanded, setExpanded] =
        useState<Record<string, boolean>>({});

    const [loading, setLoading] =
        useState(true);

    const [error, setError] = useState<
        string | null
    >(null);

    /* ---------------------------------------------------------------------- */
    /* Load data                                                               */
    /* ---------------------------------------------------------------------- */

    useEffect(() => {
        let cancelled = false;

        async function loadData() {
            try {
                setLoading(true);
                setError(null);

                const [
                    sitesData,
                    warehousesData,
                    partitionsData,
                    binsData,
                    stockData,
                ] = await Promise.all([
                    getSites(),
                    getWarehouses(),
                    getPartitions(),
                    getBins(),
                    getStock(),
                ]);

                if (cancelled) {
                    return;
                }

                setSites(sitesData);
                setWarehouses(
                    warehousesData
                );
                setPartitions(
                    partitionsData
                );
                setBins(binsData);
                setStock(stockData);

                /*
                 * Keep the original behavior:
                 * Sites are expanded by default.
                 */
                const initialExpanded: Record<
                    string,
                    boolean
                > = {};

                sitesData.forEach((site) => {
                    initialExpanded[
                        `site-${site.siteId}`
                    ] = true;
                });

                setExpanded(
                    initialExpanded
                );
            } catch (err) {
                if (cancelled) {
                    return;
                }

                console.error(
                    "Failed to load locations:",
                    err
                );

                setError(
                    err instanceof Error
                        ? err.message
                        : "Failed to load locations."
                );
            } finally {
                if (!cancelled) {
                    setLoading(false);
                }
            }
        }

        loadData();

        return () => {
            cancelled = true;
        };
    }, []);


    /* ---------------------------------------------------------------------- */
    /* Stock grouped by bin                                                    */
    /* ---------------------------------------------------------------------- */

    const stockByBin = useMemo(() => {
        const map = new Map<
            number,
            StockDto[]
        >();

        stock.forEach((item) => {
            if (item.binId === null) {
                return;
            }

            const existing =
                map.get(item.binId) ?? [];

            existing.push(item);

            map.set(
                item.binId,
                existing
            );
        });

        return map;
    }, [stock]);

    /* ---------------------------------------------------------------------- */
    /* Build hierarchy                                                         */
    /*                                                                          */
    /* Site                                                                     */
    /*   └── Warehouse                                                          */
    /*       └── Partition                                                      */
    /*           └── Bin                                                        */
    /* ---------------------------------------------------------------------- */

    const locations = useMemo<LocationNode[]>(() => {
        return sites.map((site) => {
            /* ------------------------------------------------------------------ */
            /* Warehouses belonging to this site                                  */
            /* ------------------------------------------------------------------ */

            const siteWarehouses = warehouses.filter(
                (warehouse) =>
                    warehouse.siteId === site.siteId
            );

            /* ------------------------------------------------------------------ */
            /* Build warehouses                                                    */
            /* ------------------------------------------------------------------ */

            const warehouseNodes = siteWarehouses.map(
                (warehouse) => {
                    /*
                     * Partitions belonging to this warehouse.
                     */
                    const warehousePartitions =
                        partitions.filter(
                            (partition) =>
                                partition.warehouseId ===
                                warehouse.warehouseId
                        );

                    /* ---------------------------------------------------------- */
                    /* Build partitions                                            */
                    /* ---------------------------------------------------------- */

                    const partitionNodes =
                        warehousePartitions.map(
                            (partition) => {
                                /*
                                 * Bins belonging to this partition.
                                 */
                                const partitionBins =
                                    bins.filter(
                                        (bin) =>
                                            bin.partitionId ===
                                            partition.partitionId
                                    );

                                /* ------------------------------------------------ */
                                /* Build bins                                         */
                                /* ------------------------------------------------ */

                                const binNodes =
                                    partitionBins.map(
                                        (bin) => {
                                            /*
                                             * Stock directly assigned
                                             * to this bin.
                                             */
                                            const binStock =
                                                stockByBin.get(
                                                    bin.binId
                                                ) ?? [];

                                            const binSkus =
                                                getUniqueSkus(
                                                    binStock
                                                );

                                            const binUnits =
                                                getTotalQuantity(
                                                    binStock
                                                );

                                            /*
                                             * BIN IS THE LOWEST LEVEL.
                                             *
                                             * No children.
                                             */
                                            return {
                                                id: `bin-${bin.binId}`,
                                                numericId:
                                                    bin.binId,
                                                name: bin.name,
                                                code: bin.code,
                                                type: "Bin" as const,

                                                skus:
                                                    binSkus.size,

                                                units:
                                                    binUnits,

                                                occupancy:
                                                    getBinOccupancy(
                                                        binUnits
                                                    ),

                                                skuCodes:
                                                    Array.from(
                                                        binSkus
                                                    ),

                                                children:
                                                    undefined,
                                            };
                                        }
                                    );

                                /* ------------------------------------------------ */
                                /* Partition totals                                  */
                                /* ------------------------------------------------ */

                                /*
                                 * IMPORTANT:
                                 *
                                 * Partition totals come ONLY from
                                 * the bins displayed underneath it.
                                 */
                                const partitionStock =
                                    partitionBins.flatMap(
                                        (bin) =>
                                            stockByBin.get(
                                                bin.binId
                                            ) ?? []
                                    );

                                const partitionSkus =
                                    getUniqueSkus(
                                        partitionStock
                                    );

                                const partitionUnits =
                                    getTotalQuantity(
                                        partitionStock
                                    );

                                return {
                                    id: `partition-${partition.partitionId}`,
                                    numericId:
                                        partition.partitionId,
                                    name: partition.name,
                                    code: partition.code,
                                    type: "Partition" as const,

                                    skus:
                                        partitionSkus.size,

                                    units:
                                        partitionUnits,

                                    occupancy:
                                        getParentOccupancy(
                                            binNodes
                                        ),

                                    skuCodes:
                                        Array.from(
                                            partitionSkus
                                        ),

                                    children:
                                        binNodes,
                                };
                            }
                        );

                    /* ---------------------------------------------------------- */
                    /* Warehouse totals                                             */
                    /* ---------------------------------------------------------- */

                    /*
                     * IMPORTANT:
                     *
                     * Warehouse totals come ONLY from the
                     * partitions/bins displayed underneath it.
                     */
                    const warehouseStock =
                        warehousePartitions.flatMap(
                            (partition) => {
                                const partitionBins =
                                    bins.filter(
                                        (bin) =>
                                            bin.partitionId ===
                                            partition.partitionId
                                    );

                                return partitionBins.flatMap(
                                    (bin) =>
                                        stockByBin.get(
                                            bin.binId
                                        ) ?? []
                                );
                            }
                        );

                    const warehouseSkus =
                        getUniqueSkus(
                            warehouseStock
                        );

                    const warehouseUnits =
                        getTotalQuantity(
                            warehouseStock
                        );

                    return {
                        id: `warehouse-${warehouse.warehouseId}`,
                        numericId:
                            warehouse.warehouseId,
                        name: warehouse.name,
                        code: warehouse.code,
                        type: "Warehouse" as const,

                        skus:
                            warehouseSkus.size,

                        units:
                            warehouseUnits,

                        occupancy:
                            getParentOccupancy(
                                partitionNodes
                            ),

                        skuCodes:
                            Array.from(
                                warehouseSkus
                            ),

                        children:
                            partitionNodes,
                    };
                }
            );

            /* ------------------------------------------------------------------ */
            /* Site totals                                                         */
            /* ------------------------------------------------------------------ */

            /*
             * Site totals come ONLY from the warehouses
             * and therefore their partitions/bins.
             */
            const siteStock =
                siteWarehouses.flatMap(
                    (warehouse) => {
                        const warehousePartitions =
                            partitions.filter(
                                (partition) =>
                                    partition.warehouseId ===
                                    warehouse.warehouseId
                            );

                        return warehousePartitions.flatMap(
                            (partition) => {
                                const partitionBins =
                                    bins.filter(
                                        (bin) =>
                                            bin.partitionId ===
                                            partition.partitionId
                                    );

                                return partitionBins.flatMap(
                                    (bin) =>
                                        stockByBin.get(
                                            bin.binId
                                        ) ?? []
                                );
                            }
                        );
                    }
                );

            const siteSkus =
                getUniqueSkus(siteStock);

            const siteUnits =
                getTotalQuantity(siteStock);

            /* ------------------------------------------------------------------ */
            /* Site node                                                            */
            /* ------------------------------------------------------------------ */

            return {
                id: `site-${site.siteId}`,
                numericId: site.siteId,
                name: site.name,
                code: site.code,
                type: "Site" as const,

                skus: siteSkus.size,

                units: siteUnits,

                occupancy:
                    getParentOccupancy(
                        warehouseNodes
                    ),

                skuCodes:
                    Array.from(siteSkus),

                children:
                    warehouseNodes,
            };
        });
    }, [
        sites,
        warehouses,
        partitions,
        bins,
        stock,
        stockByBin,
    ]);

    /* ---------------------------------------------------------------------- */
    /* Statistics                                                              */
    /* ---------------------------------------------------------------------- */

    const stats = useMemo(() => {
        const uniqueSkus =
            getUniqueSkus(stock);

        const totalUnits =
            getTotalQuantity(stock);

        const occupiedBins =
            bins.filter((bin) => {
                const binStock =
                    stockByBin.get(
                        bin.binId
                    ) ?? [];

                return (
                    getTotalQuantity(
                        binStock
                    ) > 0
                );
            }).length;

        const avgBinOccupancy =
            bins.length > 0
                ? Math.round(
                    (occupiedBins /
                        bins.length) *
                    100
                )
                : 0;

        return {
            sites: sites.length,
            warehouses:
                warehouses.length,
            bins: bins.length,
            skus: uniqueSkus.size,
            units: totalUnits,
            avgOccupancy:
                avgBinOccupancy,
        };
    }, [
        sites,
        warehouses,
        bins,
        stock,
        stockByBin,
    ]);

    /* ---------------------------------------------------------------------- */
    /* Expand / collapse                                                       */
    /* ---------------------------------------------------------------------- */

    const toggleLocation = (
        id: string
    ) => {
        setExpanded((prev) => ({
            ...prev,
            [id]: !prev[id],
        }));
    };

    /* ---------------------------------------------------------------------- */
    /* View Stock                                                              */
    /* ---------------------------------------------------------------------- */

    const viewStock = (location: LocationNode) => {
        const binStock =
            stockByBin.get(location.numericId) ?? [];

        if (binStock.length === 0) {
            return;
        }

        const productId = binStock[0].productId;

        router.push(
            `/inventory/product?productId=${productId}`
        );
    };

    /* ---------------------------------------------------------------------- */
    /* Render rows                                                             */
    /* ---------------------------------------------------------------------- */

    const renderRows = (
        rows: LocationNode[],
        level = 0
    ): React.ReactNode[] => {
        return rows.flatMap(
            (location) => {
                const hasChildren =
                    !!location.children &&
                    location.children.length >
                    0;

                const isExpanded =
                    expanded[
                    location.id
                    ] ?? false;

                const row = (
                    <tr
                        key={location.id}
                        style={{
                            height: "44px",
                        }}
                    >
                        {/* ---------------------------------------------------- */}
                        {/* LOCATION                                             */}
                        {/* ---------------------------------------------------- */}

                        <td
                            style={{
                                padding:
                                    "0 24px",
                                whiteSpace:
                                    "nowrap",
                                borderBottom:
                                    "var(--border-default)",
                            }}
                        >
                            <div
                                style={{
                                    display:
                                        "flex",
                                    alignItems:
                                        "center",
                                    paddingLeft: `${level * 24}px`,
                                }}
                            >
                                {hasChildren ? (
                                    <button
                                        type="button"
                                        onClick={() =>
                                            toggleLocation(
                                                location.id
                                            )
                                        }
                                        aria-label={
                                            isExpanded
                                                ? `Collapse ${location.name}`
                                                : `Expand ${location.name}`
                                        }
                                        style={{
                                            width: "16px",
                                            height: "16px",
                                            padding: 0,
                                            marginRight:
                                                "8px",
                                            border: "none",
                                            background:
                                                "transparent",
                                            cursor: "pointer",
                                            display:
                                                "flex",
                                            alignItems:
                                                "center",
                                            justifyContent:
                                                "center",
                                            color:
                                                "#6b7280",
                                        }}
                                    >
                                        <span
                                            style={{
                                                fontSize:
                                                    "9px",
                                                lineHeight:
                                                    1,
                                                display:
                                                    "inline-block",
                                                transform:
                                                    isExpanded
                                                        ? "rotate(0deg)"
                                                        : "rotate(-90deg)",
                                                transition:
                                                    "transform 0.15s ease",
                                            }}
                                        >
                                            ▼
                                        </span>
                                    </button>
                                ) : (
                                    <span
                                        style={{
                                            width:
                                                "16px",
                                            marginRight:
                                                "8px",
                                        }}
                                    />
                                )}

                                <span
                                    style={{
                                        marginRight:
                                            "8px",
                                    }}
                                >
                                    {
                                        location.name
                                    }
                                </span>

                                <Status
                                    variant={getStatusVariant(
                                        location.type
                                    )}
                                    text={
                                        location.type
                                    }
                                />
                            </div>
                        </td>

                        {/* ---------------------------------------------------- */}
                        {/* SKUS                                                 */}
                        {/* ---------------------------------------------------- */}

                        <td
                            style={{
                                padding:
                                    "0 24px",
                                textAlign:
                                    "left",
                                verticalAlign:
                                    "middle",
                                borderBottom:
                                    "var(--border-default)",
                            }}
                        >
                            {location.type ===
                                "Bin" ? (
                                location.skuCodes &&
                                    location.skuCodes
                                        .length >
                                    0 ? (
                                    <div
                                        style={{
                                            display:
                                                "flex",
                                            justifyContent:
                                                "flex-start",
                                            alignItems:
                                                "center",
                                            gap: "4px",
                                            flexWrap:
                                                "wrap",
                                        }}
                                    >
                                        {location.skuCodes
                                            .slice(
                                                0,
                                                2
                                            )
                                            .map(
                                                (
                                                    sku
                                                ) => (
                                                    <Status
                                                        key={
                                                            sku
                                                        }
                                                        text={
                                                            sku
                                                        }
                                                        variant="grey"
                                                    />
                                                )
                                            )}

                                        {location
                                            .skuCodes
                                            .length >
                                            2 && (
                                                <span
                                                    style={{
                                                        fontSize:
                                                            "12px",
                                                    }}
                                                >
                                                    +
                                                    {location
                                                        .skuCodes
                                                        .length -
                                                        2}
                                                </span>
                                            )}
                                    </div>
                                ) : (
                                    "-"
                                )
                            ) : (
                                location.skus
                            )}
                        </td>

                        {/* ---------------------------------------------------- */}
                        {/* UNITS                                                */}
                        {/* ---------------------------------------------------- */}

                        <td
                            style={{
                                padding:
                                    "0 24px",
                                textAlign:
                                    "center",
                                borderBottom:
                                    "var(--border-default)",
                            }}
                        >
                            {formatNumber(
                                location.units
                            )}
                        </td>

                        {/* ---------------------------------------------------- */}
                        {/* OCCUPANCY                                            */}
                        {/* ---------------------------------------------------- */}

                        <td
                            style={{
                                padding:
                                    "0 24px",
                                textAlign:
                                    "center",
                                borderBottom:
                                    "var(--border-default)",
                            }}
                        >
                            <Status 
                            text={location.occupancy ?? "—"}
                            variant={getStatusVariant(location.occupancy || "")}
                                
                                />
                            
                        </td>

                        {/* ---------------------------------------------------- */}
                        {/* ACTIONS                                               */}
                        {/* ---------------------------------------------------- */}

                        <td
                            style={{
                                padding:
                                    "0 24px",
                                whiteSpace:
                                    "nowrap",
                                verticalAlign:
                                    "middle",
                                textAlign:
                                    "center",
                                borderBottom:
                                    "var(--border-default)",
                            }}
                        >
                            {location.type ===
                                "Bin" &&
                                location.units >
                                0 && (
                                    <div
                                        style={{
                                            width:
                                                "100%",
                                            display:
                                                "flex",
                                            justifyContent:
                                                "center",
                                            alignItems:
                                                "center",
                                        }}
                                    >
                                        <Button
                                            variant="outline"
                                            size="sm"
                                            onClick={() =>
                                                viewStock(
                                                    location
                                                )
                                            }
                                        >
                                            View Stock
                                        </Button>
                                    </div>
                                )}
                        </td>
                    </tr>
                );

                /*
                 * Only Site, Warehouse and Partition
                 * can have children.
                 *
                 * Bin is always the final level.
                 */
                if (
                    hasChildren &&
                    isExpanded
                ) {
                    return [
                        row,
                        ...renderRows(
                            location.children!,
                            level + 1
                        ),
                    ];
                }

                return [row];
            }
        );
    };

    /* ---------------------------------------------------------------------- */
    /* UI                                                                      */
    /* ---------------------------------------------------------------------- */

    return (
        <div className="page-column">
            {/* ---------------------------------------------------------------- */}
            {/* STATS                                                             */}
            {/* ---------------------------------------------------------------- */}

            <div
                className="row-container"
                style={{
                    width: "100%",
                    flexWrap: "wrap",
                }}
            >
                <StatsCard
                    title="Sites"
                    valueColor={
                        "var(--midnight-blue)"
                    }
                    value={stats.sites}
                />

                <StatsCard
                    title="Warehouses"
                    valueColor={
                        "var(--blue)"
                    }
                    value={
                        stats.warehouses
                    }
                />

                <StatsCard
                    title="Total Bins"
                    valueColor={
                        "var(--dark-green)"
                    }
                    value={stats.bins}
                />

                <StatsCard
                    title="Avg Occupancy"
                    valueColor={
                        "var(--orange)"
                    }
                    value={`${stats.avgOccupancy}%`}
                />
            </div>

            {/* ---------------------------------------------------------------- */}
            {/* LOCATION HIERARCHY                                                */}
            {/* ---------------------------------------------------------------- */}

            <div
                className="card column-container"
                style={{
                    width: "100%",
                    gap: 0,
                    paddingInline: 0,
                }}
            >
                {/* HEADER */}

                <div
                    className="row-container"
                    style={{
                        width: "100%",
                        justifyContent:
                            "space-between",
                        alignItems:
                            "center",
                        paddingInline:
                            "var(--space-3)",
                    }}
                >
                    <p className="body-title">
                        Location Hierarchy
                    </p>

                    <Button
                        variant="secondary"
                        size="sm"
                    >
                        Add Location
                    </Button>
                </div>

                {/* ------------------------------------------------------------ */}
                {/* LOADING                                                        */}
                {/* ------------------------------------------------------------ */}

                {loading && (
                    <div
                        style={{
                            padding: "40px",
                            textAlign:
                                "center",
                        }}
                    >
                        Loading locations...
                    </div>
                )}

                {/* ------------------------------------------------------------ */}
                {/* ERROR                                                          */}
                {/* ------------------------------------------------------------ */}

                {!loading &&
                    error && (
                        <div
                            style={{
                                padding:
                                    "40px",
                                textAlign:
                                    "center",
                                color:
                                    "var(--blood-red)",
                            }}
                        >
                            <div>
                                Failed to load
                                locations.
                            </div>

                            <small>
                                {error}
                            </small>
                        </div>
                    )}

                {/* ------------------------------------------------------------ */}
                {/* TABLE                                                          */}
                {/* ------------------------------------------------------------ */}

                {!loading &&
                    !error && (
                        <div
                            style={{
                                width:
                                    "100%",
                                overflowX:
                                    "auto",
                                overflowY:
                                    "auto",
                                marginTop:
                                    "16px",
                                maxHeight:
                                    "568px",
                            }}
                        >
                            <table
                                style={{
                                    width:
                                        "100%",
                                    borderCollapse:
                                        "separate",
                                    borderSpacing:
                                        0,
                                    tableLayout:
                                        "fixed",
                                }}
                            >
                                <thead>
                                    <tr
                                        style={{
                                            height:
                                                "40px",
                                            backgroundColor:
                                                "var(--beige)",
                                            position:
                                                "sticky",
                                            top: 0,
                                            zIndex: 2,
                                        }}
                                    >
                                        <th
                                            style={{
                                                width:
                                                    "35%",
                                                padding:
                                                    "0 24px",
                                                textAlign:
                                                    "left",
                                                verticalAlign:
                                                    "middle",
                                                borderTop:
                                                    "var(--border-default)",
                                                borderBottom:
                                                    "var(--border-default)",
                                                backgroundColor:
                                                    "var(--beige)",
                                            }}
                                        >
                                            Location
                                        </th>

                                        <th
                                            style={{
                                                width:
                                                    "15%",
                                                padding:
                                                    "0 24px",
                                                textAlign:
                                                    "left",
                                                verticalAlign:
                                                    "middle",
                                                borderTop:
                                                    "var(--border-default)",
                                                borderBottom:
                                                    "var(--border-default)",
                                                backgroundColor:
                                                    "var(--beige)",
                                            }}
                                        >
                                            SKUs
                                        </th>

                                        <th
                                            style={{
                                                width:
                                                    "15%",
                                                padding:
                                                    "0 24px",
                                                textAlign:
                                                    "center",
                                                verticalAlign:
                                                    "middle",
                                                borderTop:
                                                    "var(--border-default)",
                                                borderBottom:
                                                    "var(--border-default)",
                                                backgroundColor:
                                                    "var(--beige)",
                                            }}
                                        >
                                            Units
                                        </th>

                                        <th
                                            style={{
                                                width:
                                                    "15%",
                                                padding:
                                                    "0 24px",
                                                textAlign:
                                                    "left",
                                                verticalAlign:
                                                    "middle",
                                                borderTop:
                                                    "var(--border-default)",
                                                borderBottom:
                                                    "var(--border-default)",
                                                backgroundColor:
                                                    "var(--beige)",
                                            }}
                                        >
                                            Occupancy
                                        </th>

                                        <th
                                            style={{
                                                width:
                                                    "20%",
                                                padding:
                                                    "0 24px",
                                                textAlign:
                                                    "center",
                                                verticalAlign:
                                                    "middle",
                                                borderTop:
                                                    "var(--border-default)",
                                                borderBottom:
                                                    "var(--border-default)",
                                                backgroundColor:
                                                    "var(--beige)",
                                            }}
                                        >
                                            Actions
                                        </th>
                                    </tr>
                                </thead>

                                <tbody>
                                    {locations.length >
                                        0 ? (
                                        renderRows(
                                            locations
                                        )
                                    ) : (
                                        <tr>
                                            <td
                                                colSpan={
                                                    5
                                                }
                                                style={{
                                                    padding:
                                                        "40px",
                                                    textAlign:
                                                        "center",
                                                }}
                                            >
                                                No
                                                locations
                                                found.
                                            </td>
                                        </tr>
                                    )}
                                </tbody>
                            </table>
                        </div>
                    )}
            </div>
        </div>
    );
}