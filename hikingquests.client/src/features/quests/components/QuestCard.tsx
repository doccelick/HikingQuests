import React, { useEffect, useRef, useState } from "react";
import { getStatusInfo, QuestStatus } from "../types";
import clsx from "clsx";
import styles from "./QuestCard.module.css"
import type { QuestCardProperties, QuestEditingState } from "../types";
import { useTimedMessages } from "../hooks";

export const QuestCard: React.FC<QuestCardProperties> = ({
    quest,
    expanded,
    onExpandToggle,
    onStartQuest,
    onCompleteQuest,
    onUpdateQuest
}) => {
    const statusInfo = getStatusInfo(quest.status);
    const descriptionRef = useRef<HTMLDivElement>(null);
    const [height, setHeight] = useState("0px");
    const { messages, showMessage } = useTimedMessages(0);
    const [saving, setSaving] = useState(false);
    const [questEditing, setEditingQuest] = useState<QuestEditingState>({
        isEditing: false,
        title: quest.title,
        description: quest.description
    });

    useEffect(() => {

        if (!descriptionRef.current) {
            return;
        }

        const updateHeight = () => {
            if (expanded && descriptionRef.current) {
                setHeight(`${descriptionRef.current.scrollHeight}px`);
            } else {
                setHeight("0px");
            }
        };
        updateHeight();

        const observer = new ResizeObserver(updateHeight);
        observer.observe(descriptionRef.current);

        return () => observer.disconnect();
    }, [expanded, questEditing.isEditing]);


    const handleEditQuest = () => setEditingQuest({
        ...questEditing, isEditing: true
    });

    const handleCancelQuest = () => setEditingQuest({
        isEditing: false,
        title: quest.title,
        description: quest.description
    });

    const handleConfirmEditQuest = async () => {
        setSaving(true);
        try {
            await onUpdateQuest(quest.id, {
                title: questEditing.title,
                description: questEditing.description
            })
            setEditingQuest({
                ...questEditing,
                isEditing: false
            });
        } finally {
            setSaving(false)
        }
    };

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

    const errorMessage = messages.find(
        m => m.type === "error" && m.text.trim() !== ""
    );

    const infoMessage = messages.find(
        m => m.type === "info" && m.text.trim() !== ""
    );

    const successMessage = messages.find(
        m => m.type === "success" && m.text.trim() !== ""
    );

    return (
        <li className={clsx(styles["quest-card"],
            styles[statusInfo.cardClass],
            expanded && styles["quest-card-expanded"])}>

            <div className={styles["quest-header"]}>

                <button className={styles["quest-card-expand-button"]}
                    tabIndex={0}
                    onClick={onExpandToggle}>{expanded ? "▲" : "▼"}</button>

                {!questEditing.isEditing &&
                    <span className={styles["quest-title"]}>
                        <strong>{quest.title}</strong></span>}

                {questEditing.isEditing &&
                    <input value={questEditing.title}
                        onChange={
                            e => setEditingQuest({
                                ...questEditing, title: e.target.value
                            })} />}

                <span className={clsx(styles["quest-status"],
                    styles[statusInfo.statusClass])}>
                    {statusInfo.label}</span>
            </div>

            <div ref={descriptionRef}
                className={styles["quest-description-container"]}
                style={{ maxHeight: height }}>

                {/*Quest description display field*/}
                {!questEditing.isEditing &&
                    <p className={styles["quest-description"]}>{quest.description}</p>}

                {/*Quest description edit field*/}
                {questEditing.isEditing &&
                    <textarea value={questEditing.description}
                        onChange={
                            e => setEditingQuest({
                                ...questEditing, description: e.target.value
                            })} />}

                <p className="info-message">{infoMessage?.text}</p>
                <p className="success-message">{successMessage?.text}</p>
                <p className="error-message">{errorMessage?.text}</p>

                <div className={styles["quest-format-button-container"]}>

                    {!questEditing.isEditing &&
                        quest.status === QuestStatus.Planned && (
                            <button className="primary"
                                onClick={handleStartQuest}>
                                Start Quest
                            </button>)}

                    {!questEditing.isEditing &&
                        quest.status === QuestStatus.InProgress && (
                            <button className="primary"
                                onClick={handleCompleteQuest}>
                                Complete Quest
                            </button>)}

                    {!questEditing.isEditing &&
                        quest.status === QuestStatus.Planned &&
                        messages.length === 0 && (
                            <button className="secondary"
                                onClick={handleEditQuest}>
                                Edit Quest
                            </button>)}


                    {questEditing.isEditing &&
                        <button className="primary"
                            onClick={handleConfirmEditQuest}
                            disabled={saving}>
                            Confirm
                        </button>}

                    {questEditing.isEditing &&
                        <button className="tertiary"
                            onClick={handleCancelQuest}
                            disabled={saving}>
                            Cancel
                        </button>}

                </div>

            </div>
        </li >
    );
};
