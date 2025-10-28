import type { ErrorResponseDto, QuestItem, QuestResponseDto, UpdateQuestDto } from "../types";

type NewQuestPayload = {
    title: string;
    description: string;
}

//TODO: Refactor into separate service scripts
const serverUrlForQuery = "/api/quest-query";
const serverUrlForManagement = "/api/quest-management";
const serverUrlForWorkflow = "/api/quest-workflow";

const headers = { "Content-Type": "application/json; charset=utf-8" };
const unexpectedError = "An unexpected Error occured";

export async function getQuests(): Promise<QuestItem[]> {

    const response = await fetch(serverUrlForQuery);

    if (!response.ok) {
        throw new Error(`Failed to fetch quests: ${response.status}`);
    }
    return response.json();
}

export async function addQuest(newQuest: NewQuestPayload): Promise<QuestItem> {

    const response = await fetch(serverUrlForManagement, {
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
    const url = `${serverUrlForWorkflow}/${questId}/start`;

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
    const url = `${serverUrlForWorkflow}/${questId}/complete`;

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
    const url = `${serverUrlForManagement}/${questId}`;

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
    const url = `${serverUrlForManagement}/${questId}/delete`;

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

