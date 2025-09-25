import type { QuestItem } from "../types/QuestItem";
import { getQuests, startQuest } from "../services";
import { useRetryableFetch } from "./useRetryableFetch";

export function useQuests() {

    const { data, loading, error, refetch } = useRetryableFetch<QuestItem[]>(getQuests, {
        retries: 5,
        delay: 500,
    });

    const startQuestHandler = (id: string) => startQuest(id).then(refetch);

    return {
        quests: data ?? [],
        loading,
        error,
        startQuestHandler
    };
}