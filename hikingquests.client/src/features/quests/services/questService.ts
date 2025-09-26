import type { ErrorResponseDto, QuestItem, QuestResponseDto, UpdateQuestDto } from "../types";

type NewQuestPayload = {
    title: string;
    description: string;
}

const serverUrl = "/api/quests";
const headers = { "Content-Type": "application/json; charset=utf-8" };
const unexpectedError = "An unexpected Error occured";

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
        headers: headers,
        body: JSON.stringify(newQuest),
    });

    const data: QuestItem | ErrorResponseDto = await response.json();

    if (!response.ok) {
        const message = (data as ErrorResponseDto).message || unexpectedError;
        throw new Error(message);
    }
    return data as QuestItem;
}

export async function startQuest(questId: string): Promise<QuestItem> {
    const url = `${serverUrl}/${questId}/start`;

    const response = await fetch(url, {
        method: "PATCH",
        headers: headers,
    });

    const data: QuestItem | ErrorResponseDto = await response.json();

    if (!response.ok) {
        const message = (data as ErrorResponseDto).message || unexpectedError;
        throw new Error(message);
    }

    if (!response.ok) {
        const message = (data as ErrorResponseDto).message || unexpectedError;
        throw new Error(message);
    }

    return data as QuestItem;
}

export async function completeQuest(questId: string): Promise<QuestItem> {
    const url = `${serverUrl}/${questId}/complete`;

    const response = await fetch(url, {
        method: "PATCH",
        headers: headers,
    });

    const data: QuestItem | ErrorResponseDto = await response.json();

    if (!response.ok) {
        const message = (data as ErrorResponseDto).message || unexpectedError;
        throw new Error(message);
    }

    if (!response.ok) {
        const message = (data as ErrorResponseDto)?.message || unexpectedError;
        throw new Error(message);
    }

    return data as QuestItem;
}

export async function updateQuest(questId: string, updateQuestDto: UpdateQuestDto): Promise<QuestResponseDto> {
    const url = `${serverUrl}/${questId}`;

    const response = await fetch(url, {
        method: "PATCH",
        headers: headers,
        body: JSON.stringify(updateQuestDto),
    });

    const data: QuestItem | ErrorResponseDto = await response.json();

    if (!response.ok) {
        const message = (data as ErrorResponseDto).message || unexpectedError;
        throw new Error(message);
    }

    return data as QuestItem;
}

export async function deleteQuest(questId: string): Promise<void> {
    const url = `${serverUrl}/${questId}/delete`;

    const response = await fetch(url, {
        method: "DELETE",
        headers: headers,
    });

    if (!response.ok) {
        let message = unexpectedError;
        try {
            const data = (await response.json()) as Partial<ErrorResponseDto>;
            if (data?.message) {
                message = data.message;
            }
        } catch {

        }
        throw new Error(message);
    }
}

