import React, { useEffect, useState } from "react";
import signalRService from "../../services/signalrService.ts";
import { chatService } from "../../services/chatService.ts";
import { ChatResult, Message } from "../../types/requests.ts";
import * as signalR from "@microsoft/signalr";
import "./assets/chat.css";
import { userService } from "../../services/userService.ts";
import {UserInfo} from "../../services/profileService.ts";

const ChatComponent: React.FC = () => {
    const [chats, setChats] = useState<ChatResult[]>([]);
    const [userNames, setUserNames] = useState<{ [key: number]: UserInfo }>({});
    const [messages, setMessages] = useState<Message[]>([]);
    const [input, setInput] = useState("");
    const [selectedChatId, setSelectedChatId] = useState<string | null>(null);
    const [selectedChatChatMateId, setSelectedChatChatMateId] = useState<number | null>(null);
    const [selectedUser, setSelectedUser] = useState<UserInfo | null>(null);
    const currentUserId = 1;

    useEffect(() => {
        const fetchChats = async () => {
            try {
                const data: ChatResult[] = await chatService.getMineChats();
                setChats(data.filter(chat => !chat.isArchived));

                const userIds = new Set<number>();
                data.forEach(chat => {
                    const otherUserId = chat.user1Id === currentUserId ? chat.user2Id : chat.user1Id;
                    userIds.add(otherUserId);
                });

                const userNamesMap: { [key: number]: UserInfo } = {};
                await Promise.all([...userIds].map(async (userId) => {
                    userNamesMap[userId] = await userService.getUserById(userId);
                }));

                setUserNames(userNamesMap);
            } catch (error) {
                console.error("Error fetching chats or users:", error);
            }
        };

        fetchChats();
    }, []);

    useEffect(() => {
        if (!selectedChatId || !selectedChatChatMateId) return;

        const fetchMessages = async () => {
            try {
                const data = await chatService.getMessages(selectedChatId, selectedChatChatMateId);
                setMessages(data);
            } catch (error) {
                console.error("Error fetching messages:", error);
            }
        };
        fetchMessages();
    }, [selectedChatId, selectedChatChatMateId]);

    useEffect(() => {
        signalRService.onMessageReceived((message: Message) => {
            if (message.chatId === selectedChatId) {
                setMessages((prev) => [...prev, message]);
            }
        });

        return () => {
            signalRService.onMessageReceived(() => {});
        };
    }, [selectedChatId]);

    const sendMessage = async () => {
        if (signalRService.connection.state !== signalR.HubConnectionState.Connected) {
            console.warn("Connection not started - starting.");
            await signalRService.startConnection();
        }

        if (!input.trim() || !selectedChatId) return;
        const newMessage: Message = {
            chatId: selectedChatId,
            senderId: currentUserId,
            payload: input,
            sendedDateTime: new Date().toISOString(),
        };

        await signalRService.sendMessage(selectedChatId, input);
        setMessages((prev) => [...prev, newMessage]);
        setInput("");
    };

    return (
        <div className="chatpage-container">
            <div className="chatpage-content text-white">
                <div className="chat-container">
                    <h2 className="chat-title">Chats</h2>
                    {/* Left Panel - List */}
                    <div className="chat-list">
                        <ul>
                            {chats.map((chat) => {
                                const otherUserId = chat.user1Id === currentUserId ? chat.user2Id : chat.user1Id;
                                const isSelected = selectedChatId === chat.id;

                                return (
                                    <li
                                        key={chat.id}
                                        className={isSelected ? "selected" : ""}
                                        onClick={() => {
                                            setSelectedChatId(chat.id);
                                            setSelectedChatChatMateId(otherUserId);
                                            setSelectedUser(userNames[otherUserId]);
                                        }}
                                    >
                                        <strong>
                                            {userNames[otherUserId]
                                                ? `${userNames[otherUserId].firstName} ${userNames[otherUserId].lastName}`
                                                : "..."}
                                        </strong>
                                    </li>
                                );
                            })}
                        </ul>
                    </div>
                </div>

                {/* Right Panel - Messages */}
                <div className="messages-container">
                    {selectedChatId && selectedChatChatMateId && (
                        <div className="messages-container">
                            <h3>Messages {selectedUser ? `with ${selectedUser.firstName}` : ""}</h3>
                            <ul className="messages-list">
                                {messages.map((msg, index) => (
                                    <li
                                        key={index}
                                        className={msg.senderId === currentUserId ? "my-message" : "other-message"}
                                    >
                                        <span>{msg.payload}</span>
                                        <span className="message-time">
                                            {new Date(msg.sendedDateTime).toLocaleTimeString()}
                                        </span>
                                    </li>
                                ))}
                            </ul>


                            <div className="message-input">
                                <input
                                    value={input}
                                    onChange={(e) => setInput(e.target.value)}
                                    placeholder="Type a message"
                                />
                                <button onClick={sendMessage}>Send</button>
                            </div>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default ChatComponent;
