import React, { useState } from "react";
import { useQuests } from "../hooks/useQuests";
import { QuestCard } from "./QuestCard";
import clsx from "clsx";
import styles from "./QuestList.module.css";

export const QuestList: React.FC = () => {
    const { quests, loading, error } = useQuests();
    const [expandedId, setExpandedId] = useState<string | null>(null);

    if (loading) return <p className={styles.loading}>Loading quests...</p>;
    if (error) return <p className={styles.error}>{error}</p>;

    return (
        <div className={clsx(styles["quest-container"])}>
            <h2>Quest List</h2>
            <ul className={clsx(styles["quest-list"])}>
                {quests.map((quest) => (
                    <QuestCard
                        key={quest.id}
                        quest={quest}
                        expanded={expandedId === quest.id}
                        onExpandToggle={() =>
                            setExpandedId(expandedId === quest.id ? null : quest.id)
                        }
                    />
                ))}
            </ul>
        </div>
    );
};
