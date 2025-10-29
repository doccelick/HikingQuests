import { useState, useEffect, useCallback } from "react";

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

    const fetchData = useCallback(async () => {
        setLoading(true);
        setError(null);

        for (let attempt = 0; attempt <= retries; attempt++) {
            try {
                const result = await fetchFn();
                setData(result);
                setLoading(false);
                setError(null);
                return;
            } catch (err) {
                if (attempt === retries) {
                    setError((err as Error).message || "Failed after retries");
                    setLoading(false);
                } else {
                    await new Promise((res) => setTimeout(res, delay));
                }
            }
        }
    }, [fetchFn, retries, delay]);

    useEffect(() => {
        let cancelled = false;

        fetchData().then(() => {
            if (cancelled) return;
        });

        return () => {
            cancelled = true;
        };
    }, [fetchData]);

    return { data, setData, loading, error, refetch: fetchData };
}
