import { useState, useEffect } from "react";

interface RetryOptions {
    retries?: number;
    delay?: number;
}

export function useRetryableFetch<T>(
    fetchFn: () => Promise<T>,
    { retries = 3, delay = 500 }: RetryOptions = {}
) {
    const [data, setData] = useState<T | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        let cancelled = false;

        const attemptFetch = async () => {
            setLoading(true);
            setError(null);

            for (let attempt = 0; attempt <= retries; attempt++) {
                try {
                    const result = await fetchFn();
                    if (!cancelled) {
                        setData(result);
                        setError(null);
                        setLoading(false);
                    }
                    return;
                } catch (err) {
                    if (attempt === retries) {
                        if (!cancelled) {
                            setError((err as Error).message || "Failed after retries");
                            setLoading(false);
                        }
                    } else {
                        await new Promise((res) => setTimeout(res, delay));
                    }
                }
            }
        };

        attemptFetch();

        return () => {
            cancelled = true;
        };
    }, [fetchFn, retries, delay]);

    return { data, loading, error };
}
