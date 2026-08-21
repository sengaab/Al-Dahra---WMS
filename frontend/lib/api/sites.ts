import { apiFetch } from "./api";
import {
    Site,
    CreateSiteRequest,
    UpdateSiteRequest,
} from "./types";

export async function getSites(): Promise<Site[]> {
    return apiFetch<Site[]>("/api/Site");
}

export async function getSite(
    id: number
): Promise<Site> {
    return apiFetch<Site>(
        `/api/Site/${id}`
    );
}

export async function getSiteByCode(
    code: string
): Promise<Site> {
    return apiFetch<Site>(
        `/api/Site/GetbyCode/${encodeURIComponent(code)}`
    );
}

export async function createSite(
    data: CreateSiteRequest
): Promise<Site> {
    return apiFetch<Site>(
        "/api/Site",
        {
            method: "POST",
            body: JSON.stringify(data),
        }
    );
}

export async function updateSite(
    id: number,
    data: UpdateSiteRequest
): Promise<Site> {
    return apiFetch<Site>(
        `/api/Site/${id}`,
        {
            method: "PUT",
            body: JSON.stringify(data),
        }
    );
}

export async function deleteSite(
    id: number
): Promise<void> {
    await apiFetch<void>(
        `/api/Site/${id}`,
        {
            method: "DELETE",
        }
    );
}