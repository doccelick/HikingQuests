export const QuestStatus = {
    Planned: 0,
    InProgress: 1,
    Completed: 2,
} as const;

export type QuestStatus = (typeof QuestStatus)[keyof typeof QuestStatus];

export function getStatusInfo(status: QuestStatus) {
    switch (status) {
        case QuestStatus.Planned:
            return { label: "(Planned)", className: "quest-status--planned" };
        case QuestStatus.InProgress:
            return { label: "(InProgress)", className: "quest-status--inprogress" };
        case QuestStatus.Completed:
            return { label: "(Completed)", className: "quest-status--completed" };
    };
};
