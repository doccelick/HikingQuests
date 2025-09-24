import { useState, useEffect } from "react";
import type { QuestItem } from "../types/QuestItem";


const serverUrl = "/api/quests";

export function useQuests() {
    const [quests, setQuests] = useState<QuestItem[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchQuests = async () => {
            const maxRetries = 5;
            let attempt = 0;

            while (attempt < maxRetries) {
                try {
                    const res = await fetch(serverUrl);
                    if (!res.ok) throw new Error(`Failed to fetch quests ${res.status}`);
                    const data: QuestItem[] = await res.json();
                    setQuests(data ?? []);
                    setError(null);
                    break;
                } catch (err) {
                    attempt++;
                    if (attempt >= maxRetries) {
                        setError("Unable to fetch quests after several attempts.");
                    } else {
                        await new Promise((res) => setTimeout(res, 500));
                    }
                }
            }
            setLoading(false);
        };

        fetchQuests();
    }, []);

    return { quests, loading, error };
}