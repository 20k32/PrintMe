import React, { useEffect, useState } from "react";
import signalRService from "../../services/signalrService.ts";

const ChatComponent: React.FC = () => {
    const [messages, setMessages] = useState<{ sender: string; message: string }[]>([]);
    const [input, setInput] = useState("");
    const [receiverId, setReceiverId] = useState("");

    useEffect(() => {
        signalRService.onMessageReceived((message) => {
            setMessages((prev) => [...prev, message]);
        });

        return () => {
            signalRService.onMessageReceived(() => {});
        };
    }, []);

    const sendMessage = async () => {
        if (!input.trim() || !receiverId.trim()) return;
        await signalRService.sendMessage(receiverId, input);
        setInput("");
    };

    return (
        <div>
            <h2>Chat</h2>

            <label>Receiver ID:</label>
            <input value={receiverId} onChange={(e) => setReceiverId(e.target.value)} placeholder="Enter user ID" />

            <ul>
                {messages.map((msg, index) => (
                    <li key={index}>
                        <strong>{msg.sender}:</strong> {msg.message}
                    </li>
                ))}
            </ul>

            <input value={input} onChange={(e) => setInput(e.target.value)} placeholder="Type a message" />
            <button onClick={sendMessage}>Send</button>
        </div>
    );
};

export default ChatComponent;
