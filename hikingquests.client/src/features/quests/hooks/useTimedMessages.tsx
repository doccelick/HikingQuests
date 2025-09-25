import { useState } from "react";

type MessageType = "error" | "success" | "info";

interface TimedMessage {
    id: string;
    text: string;
    type: MessageType;
}

export function useTimedMessages(defaultDuration = 4000) {
    const [messages, setMessages] = useState<TimedMessage[]>([]);

    const showMessage = (text: string, type: MessageType = "info", duration = defaultDuration) => {
        const id = crypto.randomUUID();
        setMessages(prev => [...prev, { id, text, type }]);

        setTimeout(() => {
            setMessages(prev => prev.filter(m => m.id !== id));
        }, duration);
    };

    return { messages, showMessage };
}
