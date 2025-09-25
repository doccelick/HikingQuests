import type { QuestItem } from "../types/QuestItem";
import { completeQuest, getQuests, startQuest, updateQuest } from "../services";
import { useRetryableFetch } from "./useRetryableFetch";
import { useMemo } from "react";
import type { UpdateQuestDto } from "../types";

export function useQuests() {

    const { data, loading, error, refetch } = useRetryableFetch<QuestItem[]>(getQuests, {
        retries: 5,
        delay: 1000,
    });

    const startQuestHandler = (id: string) => startQuest(id).then(refetch);

    const completeQuestHandler = (id: string) => completeQuest(id).then(refetch);

    const updateQuestHandler = (id: string, updateQuestDto: UpdateQuestDto) => updateQuest(id, updateQuestDto);

    const quests = useMemo(() => data ?? [], [data]);

    return {
        quests,
        loading,
        error,
        startQuestHandler,
        completeQuestHandler,
        updateQuestHandler
    };
}