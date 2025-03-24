import React, {useEffect, useRef, useState} from "react";
import signalRService from "../../services/signalrService.ts";
import {chatService} from "../../services/chatService.ts";
import {
    ChatResult,
    Message,
    SendMessageToChatRequest,
    SendMessageToSignalR
} from "../../types/requests.ts";
import * as signalR from "@microsoft/signalr";
import "./assets/chat.css";
import {userService} from "../../services/userService.ts";
import {profileService, UserInfo} from "../../services/profileService.ts";

const ChatComponent: React.FC = () => {
    const [chats, setChats] = useState<ChatResult[]>([]);
    const [userNames, setUserNames] = useState<{ [key: number]: UserInfo }>({});
    const [messages, setMessages] = useState<Message[]>([]);
    const [input, setInput] = useState("");
    const [selectedChatId, setSelectedChatId] = useState<string | null>(null);
    const [selectedChatChatMateId, setSelectedChatChatMateId] = useState<number | null>(null);
    const [selectedUser, setSelectedUser] = useState<UserInfo | null>(null);
    const [me, setMe] = useState<UserInfo | null>(null);
    const [searchQuery, setSearchQuery] = useState("");
    const [filteredUsers, setFilteredUsers] = useState<UserInfo[]>([]);

    const messagesContainerRef = useRef<HTMLUListElement | null>(null);

    const selectedChatIdRef = useRef(selectedChatId);
    const selectedChatChatMateIdRef = useRef(selectedChatChatMateId);

    useEffect(() => {
        selectedChatIdRef.current = selectedChatId;
    }, [selectedChatId]);

    useEffect(() => {
        selectedChatChatMateIdRef.current = selectedChatChatMateId;
    }, [selectedChatChatMateId]);

    const scrollToBottom = () => {
        if (messagesContainerRef.current) {
            messagesContainerRef.current.scrollTop = messagesContainerRef.current.scrollHeight;
        }
    };

    const messageHandler = (message: SendMessageToSignalR) => {
        if (selectedChatIdRef.current && selectedChatChatMateIdRef.current === Number(message.senderId)) {
            const newMessage: Message = {
                chatId: selectedChatIdRef.current!,
                senderId: Number(selectedChatChatMateIdRef.current),
                payload: message.payload,
                sentDateTime: message.sentDate,
            };

            setMessages((prev) => [...prev, newMessage]);
        }
    };

    useEffect(() => {
        const fetchChats = async () => {
            try {
                const userMe = await profileService.fetchUserData();
                setMe(userMe);

                const data: ChatResult[] = await chatService.getMineChats();
                setChats(data.filter(chat => !chat.isArchived));

                const userIds = new Set<number>();
                data.forEach(chat => {
                    const otherUserId = chat.user1Id === userMe.userId ? chat.user2Id : chat.user1Id;
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

        signalRService.onMessageReceived(messageHandler);

        return () => {
            signalRService.offMessageReceived(messageHandler);
        };
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
        scrollToBottom();
    }, [messages]);

    const sendMessage = async () => {
        if (signalRService.connection.state !== signalR.HubConnectionState.Connected) {
            console.warn("Connection not started - starting.");
            await signalRService.startConnection();
        }

        if (!input.trim() || !selectedChatId || !selectedChatChatMateId) return;

        const sentDateTime = new Date();
        const newMessage: Message = {
            chatId: selectedChatId!,
            senderId: me!.userId,
            payload: input,
            sentDateTime: sentDateTime,
        };

        const newSignalRMessage: SendMessageToSignalR = {
            receiverId: selectedChatChatMateId!.toString(),
            senderId: me!.userId.toString(),
            payload: input,
            sentDate: sentDateTime,
        };

        await signalRService.sendMessage(newSignalRMessage);

        const sendMessageRequest: SendMessageToChatRequest = {
            chatId: selectedChatId!,
            payload: input,
            sentDateTime: sentDateTime,
        };
        await chatService.sendMessage(sendMessageRequest);

        setMessages((prev) => [...prev, newMessage]);

        setInput("");
    };

    const handleSearchChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
        const query = e.target.value;
        setSearchQuery(query);

        if (!query) {
            setFilteredUsers([]);
            return;
        }

        try {
            const users = await userService.searchUsers(query);
            setFilteredUsers(users);
        } catch (error) {
            console.error("Error searching for users:", error);
        }
    };

    const handleUserSelect = async (userId: number) => {
        const selectedUser = userNames[userId];
        if (!selectedUser) return;

        const existingChat = chats.find(chat => {
            return (chat.user1Id === selectedUser.userId || chat.user2Id === selectedUser.userId);
        });

        if (existingChat) {
            setSelectedChatId(existingChat.id);
            setSelectedChatChatMateId(selectedUser.userId);
        } else {
            const newChat = await chatService.createChat(selectedUser.userId);
            setChats((prevChats) => [...prevChats, newChat]);
            setSelectedChatId(newChat.id);
            setSelectedChatChatMateId(selectedUser.userId);
        }

        setSelectedUser(selectedUser);
    };

    return (
        <div className="chatpage-container">
            <div className="chatpage-content text-white">
                <div className="chat-container">
                    <h2 className="chat-title">Chats</h2>
                    {/* Left Panel - Chat List */}
                    <div className="chat-list">
                        <input
                            type="text"
                            value={searchQuery}
                            onChange={handleSearchChange}
                            placeholder="Search users..."
                        />
                        <ul>
                            {filteredUsers.length > 0 && searchQuery && (
                                <div>
                                    <h3>Search Results:</h3>
                                    {filteredUsers.map((user) => (
                                        <li
                                            key={user.userId}
                                            onClick={() => handleUserSelect(user.userId)}
                                        >
                                            <strong>{user.firstName} {user.lastName}</strong>
                                        </li>
                                    ))}
                                </div>
                            )}

                            {chats.map((chat) => {
                                const otherUserId = chat.user1Id === me?.userId ? chat.user2Id : chat.user1Id;
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
                                                : "Loading..."}
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
                            <ul className="messages-list" ref={messagesContainerRef}>
                                {messages.map((msg, index) => (
                                    <li
                                        key={index}
                                        className={msg.senderId === me?.userId ? "my-message" : "other-message"}
                                    >
                                        <span>{msg.payload}</span>
                                        <span className="message-time">
                                            {new Date(msg.sentDateTime).toLocaleTimeString()}
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
