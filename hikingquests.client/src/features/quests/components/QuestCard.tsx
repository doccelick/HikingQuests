import React from "react";
import type { QuestItem } from "../hooks/useQuests";
import { getStatusInfo } from "../types/QuestStatus";
import clsx from "clsx";
import styles from "./QuestCard.module.css"

export const QuestCard: React.FC<{ quest: QuestItem }> = ({ quest }) => {
    const statusInfo = getStatusInfo(quest.status);

    return (
        <li className={styles["quest-card"]}>
            <div className={styles["quest-header"]}>
                <span className={styles["quest-title"]}><strong>{quest.title}</strong></span>
                <span className={clsx(
                    styles["quest-status"],
                    styles[statusInfo.className]
                )}>
                    {statusInfo.label}
                </span>
            </div>
            <p className={styles["quest-description"]}>{quest.description}</p>
        </li>
    );
};
