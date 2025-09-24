import { useState } from "react";
import { addQuest as addQuestService } from "../services/questService";
import type { QuestItem } from "../types/QuestItem";
import styles from "./AddQuestForm.module.css";
import clsx from "clsx";

interface AddQuestFormProperties {
    onQuestAdded: (quest: QuestItem) => void;
    onCancel: () => void;
}

export const AddQuestForm: React.FC<AddQuestFormProperties> = ({
    onQuestAdded, onCancel
}) => {
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleAdd = async () => {
        setLoading(true);

        try {
            const newQuest = await addQuestService({ title, description });
            onQuestAdded(newQuest);


            setTitle("");
            setDescription("");
        } catch (err) {
            setError((err as Error).message)
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className={clsx(styles["add-quest-form"])}>
            {error && <p style={{ color: "red" }}>{error}</p>}

            <input
                type="text"
                placeholder="Title"
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                disabled={loading}
            />

            <textarea
                placeholder="Description"
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                disabled={loading}
            />

            <div className={clsx(styles["form-buttons"])}>
                <button
                    type="button"
                    className="primary"
                    onClick={handleAdd}
                    disabled={loading}
                >
                    Add Quest
                </button>
                <button
                    type="button"
                    className="secondary"
                    onClick={onCancel}
                    disabled={loading}
                >
                    Cancel
                </button>
            </div>
        </div>
    )
};