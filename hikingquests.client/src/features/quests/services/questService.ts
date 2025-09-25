import type { ErrorResponseDto, QuestItem, QuestResponseDto, UpdateQuestDto } from "../types";

type NewQuestPayload = {
    title: string;
    description: string;
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
        headers: { "Content-Type": "application/json; charset=utf-8" },
        body: JSON.stringify(newQuest),
    });

    const data: QuestItem | ErrorResponseDto = await response.json();

    if (!response.ok) {
        throw new Error((data as ErrorResponseDto).message || "An unexpected Error occured");
    }
    return data as QuestItem;
}

export async function startQuest(questId: string): Promise<QuestItem> {
    const url = `/api/quests/${questId}/start`;

    const response = await fetch(url, {
        method: "PATCH",
        headers: { "Content-Type": "application/json; charset=utf-8" },
    });

    let data: unknown;
    try {
        data = await response.json();
    } catch {
        data = {};
    }

    if (!response.ok) {
        const message = (data as ErrorResponseDto)?.message || "An unexpected error occurred";
        throw new Error(message);
    }

    return data as QuestItem;
}

export async function completeQuest(questId: string): Promise<QuestItem> {
    const url = `/api/quests/${questId}/complete`;

    const response = await fetch(url, {
        method: "PATCH",
        headers: { "Content-Type": "application/json; charset=utf-8" },
    });

    let data: unknown;
    try {
        data = await response.json();
    } catch {
        data = {};
    }

    if (!response.ok) {
        const message = (data as ErrorResponseDto)?.message || "An unexpected error occurred";
        throw new Error(message);
    }

    return data as QuestItem;
}


export async function updateQuest(questId: string, updateQuestDto: UpdateQuestDto): Promise<QuestResponseDto> {
    const url = `/api/quests/${questId}`;

    const response = await fetch(url, {
        method: "PATCH",
        headers: { "Content-Type": "application/json; charset=utf-8" },
        body: JSON.stringify(updateQuestDto),
    });

    const data = await response.json();

    if (!response.ok) {
        const message = (data as ErrorResponseDto)?.message || "An unexpected error occurred";
        throw new Error(message);
    }

    return data as QuestItem;
}
