import { createClient } from "@/lib/supabase/client";

const API_URL = "http://localhost:5171";

export async function apiFetch(
    endpoint: string,
    options: RequestInit = {}
) {
    const supabase = createClient();

    const {
        data: { session },
    } = await supabase.auth.getSession();

    const headers = new Headers(options.headers);

    headers.set("Content-Type", "application/json");

    if (session?.access_token) {
        headers.set(
            "Authorization",
            `Bearer ${session.access_token}`
        );
    }

    const response = await fetch(
        `${API_URL}${endpoint}`,
        {
            ...options,
            headers,
        }
    );

    if (!response.ok) {
        throw new Error(
            `API Error: ${response.status} ${response.statusText}`
        );
    }

    return response.json();
}