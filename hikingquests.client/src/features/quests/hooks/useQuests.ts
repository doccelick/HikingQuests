import type { QuestItem } from "../types/QuestItem";
import { getQuests, startQuest } from "../services";
import { useRetryableFetch } from "./useRetryableFetch";
import { useMemo } from "react";

export function useQuests() {

    const { data, loading, error, refetch } = useRetryableFetch<QuestItem[]>(getQuests, {
        retries: 5,
        delay: 500,
    });

    const startQuestHandler = (id: string) => startQuest(id).then(refetch);

    const quests = useMemo(() => data ?? [], [data]);

    return {
        quests,
        loading,
        error,
        startQuestHandler
    };
}