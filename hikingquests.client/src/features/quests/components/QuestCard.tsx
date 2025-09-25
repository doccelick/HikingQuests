import React, { useEffect, useRef, useState } from "react";
import { getStatusInfo, QuestStatus } from "../types/QuestStatus";
import clsx from "clsx";
import styles from "./QuestCard.module.css"
import type { QuestCardProperties } from "../types";
import { useTimedMessages } from "../hooks";

export const QuestCard: React.FC<QuestCardProperties> = ({
    quest, expanded, onExpandToggle, onStartQuest, onCompleteQuest
}) => {
    const statusInfo = getStatusInfo(quest.status);
    const descriptionRef = useRef<HTMLDivElement>(null);
    const [height, setHeight] = useState("0px");
    const { messages, showMessage } = useTimedMessages(0);

    useEffect(() => {
        if (expanded && descriptionRef.current) {
            setHeight(`${descriptionRef.current.scrollHeight}px`);
        } else {
            setHeight("0px");
        }
    }, [expanded]);

    const handleStartQuest = async () => {
        showMessage("Starting quest...", "info", 1000);
        try {
            await onStartQuest();
            showMessage("Success", "success", 1500);
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : String(err);
            showMessage(errorMessage, "error", 3000);
        }
    };

    const handleCompleteQuest = async () => {
        showMessage("Completing quest...", "info", 1000);
        try {
            await onCompleteQuest();
            showMessage("Success", "success", 1500);
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : String(err);
            showMessage(errorMessage, "error", 3000);
        }
    };

    const firstErrorMessage = messages.find(
        m => m.type === "error" && m.text.trim() !== ""
    );

    const firstInfoMessage = messages.find(
        m => m.type === "info" && m.text.trim() !== ""
    );

    const firstSuccessMessage = messages.find(
        m => m.type === "success" && m.text.trim() !== ""
    );

    return (
        <li
            className={clsx(
                styles["quest-card"],
                styles[statusInfo.cardClass],
                expanded && styles["quest-card-expanded"]
            )}
        >

            <div className={styles["quest-header"]}>
                <button
                    className={styles["quest-card-expand-button"]}
                    tabIndex={0}
                    onClick={onExpandToggle}
                >
                    {
                        expanded ? "▲" : "▼"
                    }
                </button>
                <span
                    className={styles["quest-title"]}>
                    <strong>{quest.title}</strong></span>
                <span
                    className={clsx(
                        styles["quest-status"],
                        styles[statusInfo.statusClass]
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

                <p className="info-message">{firstInfoMessage?.text}</p>
                <p className="success-message">{firstSuccessMessage?.text}</p>
                <p className="error-message">{firstErrorMessage?.text}</p>

                {!firstErrorMessage?.text && <div className={styles["quest-format-button-container"]}>
                    {quest.status === QuestStatus.Planned &&
                        messages.length == 0 &&
                        <button
                            className="primary"
                            onClick={handleStartQuest}
                        >
                            Begin Quest
                        </button>}
                    {quest.status === QuestStatus.InProgress &&
                        messages.length == 0 &&

                        <button
                            className="primary"
                            onClick={handleCompleteQuest}
                        >
                            Complete Quest
                        </button>}

                </div>}

            </div>
        </li >
    );
};
