import { apiFetch } from "@/lib/api";
import type { UnitDto } from "@/types";

export async function getUnits(): Promise<UnitDto[]> {
    return apiFetch("/api/Units", {
        method: "GET",
    });
}