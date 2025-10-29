import { QuestStatus, type QuestItem } from "../types";
import { completeQuest, deleteQuest, getQuests, startQuest, updateQuest } from "../services";
import { useRetryableFetch } from "./useRetryableFetch";
import { useMemo } from "react";
import type { UpdateQuestDto } from "../types";

export function useQuests() {

    const { data, setData, loading, error, refetch } = useRetryableFetch<QuestItem[]>(getQuests, {
        retries: 5,
        delay: 1000,
    });

    const updateLocalQuest = (id: string, updates: Partial<QuestItem>) => {
        setData(prevQuests => {
            if (!prevQuests)
            {
                return null;
            }
            return prevQuests.map(quest =>
                quest.id === id ? {...quest, ...updates }
                : quest
            );
        });
    };

    const startQuestHandler = (id: string) => startQuest(id).then(() => updateLocalQuest(id, {status: QuestStatus.InProgress}));

    const completeQuestHandler = (id: string) => completeQuest(id).then(() => updateLocalQuest(id, {status: QuestStatus.Completed}));

    const updateQuestHandler = (id: string, updateQuestDto: UpdateQuestDto) => updateQuest(id, updateQuestDto).then(() => updateLocalQuest(id, updateQuestDto));

    const deleteQuestHandler = (id: string) => deleteQuest(id).then(() => {
        setData(prevQuests => (prevQuests ?? []).filter(quest => quest.id !== id))
    });

    const quests = useMemo(() => data ?? [], [data]);

    return {
        quests,
        loading,
        error,
        startQuestHandler,
        completeQuestHandler,
        updateQuestHandler,
        deleteQuestHandler,
        refetch
    };
}