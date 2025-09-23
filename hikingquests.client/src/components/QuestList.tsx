import React from "react";
import { useState, useEffect } from "react";

interface QuestItem {
    id: string;
    title: string;
    description: string;
    status: 0 | 1 | 2 //Planned = 0, InProgress = 1, Completed = 2
}

const getStatusInfo = (status: 0 | 1 | 2) => {
    switch (status) {
        case 0: return { label: "(Planned)", className: "quest-status--planned" };
        case 1: return { label: "(InProgress)", className: "quest-status--inprogress" };
        case 2: return { label: "(Completed)", className: "quest-status--completed" };
    }
};

const serverUrl = "/api/quests";

const QuestList: React.FC = () => {

    const [quests, setQuests] = useState<QuestItem[]>([]);

    useEffect(() => {
        const fetchQuests = async () => {

            const maxRetries = 5;
            let attempt = 0;
            let success = false;

            while (attempt < maxRetries && !success) {
                try {
                    const res = await fetch(serverUrl);
                    if (!res.ok) throw new Error(`Failed to fetch quests ${res.status}`);
                    const data: QuestItem[] = await res.json();
                    setQuests(data ?? []);
                    success = true;
                }
                catch {
                    attempt++;
                    console.warn(`Trying to fetch data from backend. Attempt ${attempt} failed, retrying...`);
                    await new Promise(res => setTimeout(res, 500));
                }
            }
            if (!success) console.error(`Unable to fetch data from backend after ${attempt} attempts.`);
        };
        fetchQuests();
    }, []);

    return (
        <div>
            <h2>Quests</h2>
            <ul className="quest-list">
                {quests.map((quest) => (
                    <li key={quest.id} className="quest-item">
                        <div className="quest-card">
                            <div className="quest-header">
                                <span className="quest-title"><strong>{quest.title}</strong></span>
                                <span className={`quest-status ${getStatusInfo(quest.status).className}`}>{getStatusInfo(quest.status).label}</span>
                            </div>
                            <p>
                                <span className="quest-description">{quest.description}</span>
                            </p>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default QuestList;
