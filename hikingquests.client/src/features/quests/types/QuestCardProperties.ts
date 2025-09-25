import type { QuestItem } from "./QuestItem";

export interface QuestCardProperties {
    quest: QuestItem;
    expanded: boolean;
    onExpandToggle: () => void;
    onStartQuest: () => void;
    onCompleteQuest: () => void;
}