import type { QuestItem } from "../types/QuestItem";
import { getQuests } from "../services";
import { useRetryableFetch } from "./useRetryableFetch";

export function useQuests() {

    const { data, loading, error } = useRetryableFetch<QuestItem[]>(getQuests, {
        retries: 5,
        delay: 500,
    });

    return { quests: data ?? [], loading, error };
}