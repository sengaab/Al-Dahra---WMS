interface StockDistributionItemProps {
    location: string;
    bin: string;
    units: number;
    available: number;
    reserved: number;
    batch: string;
    totalQuantity?: number;
}

export default function StockDistributionItem({
    location,
    bin,
    units,
    available,
    reserved,
    batch,
    totalQuantity = 350,
}: StockDistributionItemProps) {
    const percentage = (units / totalQuantity) * 100;

    return (
        <div
            className="card column-container"
            style={{
                backgroundColor: "var(--beige)",
                width: "100%",
                padding: "var(--space-2)",
                boxSizing: "border-box",
            }}
        >
            <div
                className="row-container"
                style={{
                    width: "100%",
                    justifyContent: "space-between",
                }}
            >
                <p>
                    {location} /{" "}
                    <span className="body-title">{bin}</span>
                </p>

                <p className="body-title">
                    {units} Units
                </p>
            </div>

            <div
                style={{
                    width: "100%",
                    height: "10px",
                    borderRadius: "var(--radius-lg)",
                    backgroundColor: "var(--light-grey)",
                    overflow: "hidden",
                }}
            >
                <div
                    style={{
                        width: `${percentage}%`,
                        height: "100%",
                        borderRadius: "var(--radius-lg)",
                        backgroundColor: "var(--dark-green)",
                    }}
                />
            </div>

            <div className="row-container">
                <p>
                    Avail:{" "}
                    <span
                        className="body-title"
                        style={{
                            color: "var(--dark-green)",
                        }}
                    >
                        {available}
                    </span>
                </p>

                <p>
                    Resv:{" "}
                    <span
                        className="body-title"
                        style={{
                            color: "var(--orange)",
                        }}
                    >
                        {reserved}
                    </span>
                </p>

                <p>
                    Batch:{" "}
                    <span className="body-title">
                        {batch}
                    </span>
                </p>
            </div>
        </div>
    );
}