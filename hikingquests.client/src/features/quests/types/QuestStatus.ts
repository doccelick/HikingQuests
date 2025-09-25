export const QuestStatus = {
    Planned: 0,
    InProgress: 1,
    Completed: 2,
} as const;

export type QuestStatus = (typeof QuestStatus)[keyof typeof QuestStatus];

export function getStatusInfo(status: QuestStatus) {
    switch (status) {
        case QuestStatus.Planned:
            return {
                label: "(Planned)",
                statusClass: "quest-status--planned",
                cardClass: "quest-card--planned",
            };
        case QuestStatus.InProgress:
            return {
                label: "(InProgress)",
                statusClass: "quest-status--inprogress",
                cardClass: "quest-card--inprogress"
            };
        case QuestStatus.Completed:
            return {
                label: "(Completed)",
                statusClass: "quest-status--completed",
                cardClass: "quest-card--completed"
            };
    };
};
