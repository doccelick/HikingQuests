import { useState } from "react";
import { addQuest as addQuestService } from "../services";
import styles from "./AddQuestForm.module.css";
import clsx from "clsx";
import { useTimedMessages } from "../hooks";
import type { AddQuestFormProperties } from "../types";

export const AddQuestForm: React.FC<AddQuestFormProperties> = ({
    onQuestAdded, onCancel
}) => {
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [loading, setLoading] = useState(false);
    const { messages, showMessage } = useTimedMessages(0);

    const handleAdd = async () => {
        setLoading(true);

        try {
            const newQuest = await addQuestService({ title, description });
            onQuestAdded(newQuest);
            setTitle("");
            setDescription("");
        } catch (err) {
            showMessage((err as Error).message, "error", 2000)
        } finally {
            setLoading(false);
        }
    };

    const firstError = messages.find(
        m => m.type === "error" && m.text.trim() !== ""
    );

    return (
        <div className={clsx(styles["add-quest-form"])}>
            {<p style={{ color: "red" }}>{firstError?.text}</p>}

            <input type="text"
                placeholder="Title"
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                disabled={loading} />

            <textarea placeholder="Description"
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                disabled={loading} />

            <div className={clsx(styles["form-buttons"])}>

                <button
                    type="button"
                    className="primary"
                    onClick={handleAdd}
                    disabled={loading}>
                    Add Quest
                </button>

                <button
                    type="button"
                    className="tertiary"
                    onClick={onCancel}
                    disabled={loading}>
                    Cancel
                </button>
            </div>
        </div>
    )
};