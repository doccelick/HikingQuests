import React, { useEffect, useRef, useState } from "react";
import { getStatusInfo } from "../types/QuestStatus";
import clsx from "clsx";
import styles from "./QuestCard.module.css"
import type { QuestItem } from "../types/QuestItem";

interface QuestCardProperties {
    quest: QuestItem;
    expanded: boolean;
    onExpandToggle: () => void;
}

export const QuestCard: React.FC<QuestCardProperties> = ({
    quest, expanded, onExpandToggle
}) => {
    const statusInfo = getStatusInfo(quest.status);
    const descriptionRef = useRef<HTMLDivElement>(null);
    const [height, setHeight] = useState("0px");

    useEffect(() => {
        if (expanded && descriptionRef.current) {
            setHeight(`${descriptionRef.current.scrollHeight}px`);
        } else {
            setHeight("0px");
        }
    }, [expanded]);

    return (
        <li
            className={clsx(
                styles["quest-card"],
                expanded && styles["quest-card-expanded"]
            )}
            tabIndex={0}
            onClick={onExpandToggle}
        >
            <div className={styles["quest-header"]}>
                <span
                    className={styles["quest-title"]}>
                    <strong>{quest.title}</strong></span>
                <span
                    className={clsx(
                        styles["quest-status"],
                        styles[statusInfo.className]
                    )}
                >
                    {statusInfo.label}
                </span>
            </div>
            <div
                ref={descriptionRef}
                className={styles["quest-description-container"]}
                style={{ maxHeight: height }}
            >

                <p
                    className={styles["quest-description"]}
                >
                    {quest.description}
                </p>

            </div>
        </li>
    );
};
