import type { QuestItem } from "../types/QuestItem";

type NewQuestPayload = {
    title: string;
    description: string;
}

type ErrorResponseDto = {
    message: string;
}

const serverUrl = "/api/quests";

export async function getQuests(): Promise<QuestItem[]> {

    const response = await fetch(serverUrl);

    if (!response.ok) {
        throw new Error(`Failed to fetch quests: ${response.status}`);
    }
    return response.json();
}

export async function addQuest(newQuest: NewQuestPayload): Promise<QuestItem> {

    const response = await fetch(serverUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(newQuest),
    });

    const data: QuestItem | ErrorResponseDto = await response.json();

    if (!response.ok) {
        throw new Error((data as ErrorResponseDto).message || "An unexpected Error occured");
    }
    return data as QuestItem;
}