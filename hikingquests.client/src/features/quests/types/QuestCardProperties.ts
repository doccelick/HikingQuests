import type { QuestItem } from "./QuestItem";
import type { UpdateQuestDto } from "./UpdateQuestDto";

export interface QuestCardProperties {
    quest: QuestItem;
    expanded: boolean;
    onExpandToggle: () => void;
    onStartQuest: () => void;
    onCompleteQuest: () => void;
    onUpdateQuest: (id: string, dto: UpdateQuestDto) => Promise<void>;
    onDeleteQuest: () => void;
}