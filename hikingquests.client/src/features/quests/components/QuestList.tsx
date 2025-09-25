import React, { useEffect, useState } from "react";
import { useQuests } from "../hooks/useQuests";
import { QuestCard } from "./QuestCard";
import clsx from "clsx";
import styles from "./QuestList.module.css";
import type { QuestItem } from "../types/QuestItem";
import { AddQuestForm } from "./AddQuestForm";

export const QuestList: React.FC = () => {
    const { quests: fetchedQuests, loading, error, startQuestHandler, completeQuestHandler } = useQuests();
    const [expandedId, setExpandedId] = useState<string | null>(null);
    const [quests, setQuests] = useState<QuestItem[]>([]);
    const [showCreationForm, setShowCreationForm] = useState(false);

    useEffect(() => {
        setQuests(fetchedQuests);
    },
        [fetchedQuests]
    );

    const handleQuestAdded = (newQuest: QuestItem) => {
        setQuests((prev) => [...prev, newQuest]);
        setShowCreationForm(false);
    };

    return (
        <div className={clsx(styles["quest-container"])}>
            <h2>Quest List</h2>
            {loading && <p>Loading data...</p>}
            {error && <p>Error: {error}</p>}
            <div className={clsx(styles["add-quest-button-container"])}>
                {!showCreationForm && (
                    <button onClick={() => setShowCreationForm(true)}>Add Quest</button>
                )}
            </div>
            {showCreationForm && (
                <AddQuestForm
                    onQuestAdded={handleQuestAdded}
                    onCancel={() => setShowCreationForm(false)}
                />
            )}

            <ul className={clsx(styles["quest-list"])}>
                {quests.map((quest) => (
                    <QuestCard
                        key={quest.id}
                        quest={quest}
                        expanded={expandedId === quest.id}
                        onExpandToggle={() =>
                            setExpandedId(expandedId === quest.id ? null : quest.id)
                        }
                        onStartQuest={() => startQuestHandler(quest.id)}
                        onCompleteQuest={() => completeQuestHandler(quest.id)}
                    />
                ))}
            </ul>
        </div>
    );
};
