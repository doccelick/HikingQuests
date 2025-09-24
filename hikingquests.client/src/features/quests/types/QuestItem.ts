import type { QuestStatus } from "./QuestStatus";

export interface QuestItem {
    id: string;
    title: string;
    description: string;
    status: QuestStatus;
}
