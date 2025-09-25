import type { QuestItem } from "./QuestItem";

export interface AddQuestFormProperties {
    onQuestAdded: (quest: QuestItem) => void;
    onCancel: () => void;
}