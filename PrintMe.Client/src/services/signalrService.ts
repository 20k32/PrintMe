import * as signalR from "@microsoft/signalr";
import "bootstrap/dist/css/bootstrap.min.css";
import {API_MESSAGE_URL} from "../constants.ts";
import {profileService} from "./profileService.ts";

class SignalRService {
    private connection: signalR.HubConnection;

    constructor() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(API_MESSAGE_URL, {
                accessTokenFactory: () => localStorage.getItem("token") || "",
            })
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .build();

        this.startConnection();
    }

    private async startConnection(): Promise<void> {
        try {
            await this.connection.start();
            console.log("SignalR Connected.");
        } catch (err) {
            console.error("Error starting SignalR connection:", err);
            setTimeout(() => this.startConnection(), 5000);
        }
    }

    onMessageReceived(callback: (message: any) => void): void {
        this.connection.on("MessageReceived", callback);
    }

    async sendMessage(receiverId: string, content: string): Promise<void> {
        try {
            //const profileData = await profileService.fetchUserData();
            await this.connection.invoke("MessageReceived", {
                receiverId,
                payload: content,
                sentDate: new Date().toISOString()
                //senderId: profileData.userId,
            });
        } catch (err) {
            console.error("Error sending message:", err);
        }
    }
}

const signalRService = new SignalRService();
export default signalRService;