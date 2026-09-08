import { createClient } from "@/lib/supabase/client";

const API_URL = "http://localhost:5171";

export async function apiFetch<T>(
    endpoint: string,
    options: RequestInit = {}
): Promise<T> {
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
        let errorMessage = `API Error: ${response.status} ${response.statusText}`;

        try {
            const errorData = await response.json();

            if (errorData?.message) {
                errorMessage = errorData.message;
            }
        } catch {
            // Keep the default error message
        }

        const error = new Error(errorMessage) as Error & {
            status: number;
        };

        error.status = response.status;

        throw error;
    }

    return response.json();
}