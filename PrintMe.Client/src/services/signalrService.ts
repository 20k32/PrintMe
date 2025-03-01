import * as signalR from "@microsoft/signalr";
import "bootstrap/dist/css/bootstrap.min.css";
import {API_MESSAGE_URL} from "../constants.ts";

class SignalRService {
    public connection: signalR.HubConnection;

    constructor() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(API_MESSAGE_URL, {
                accessTokenFactory: () => localStorage.getItem("token") || "",
            })
            .withAutomaticReconnect() // Enables automatic reconnection
            .build();

        this.startConnection();
    }

    public async startConnection() {
        try {
            await this.connection.start();
            console.log("SignalR Connected");
        } catch (error) {
            console.error("SignalR Connection Error:", error);
            setTimeout(() => this.startConnection(), 5000); // Retry connection
        }
    }

    public async sendMessage(chatId: string, message: string) {
        if (this.connection.state !== signalR.HubConnectionState.Connected) {
            console.error("SignalR is not connected. Retrying...");
            await this.startConnection(); // Ensure connection is restarted
        }

        try {
            await this.connection.invoke("MessageReceived", chatId, message);
        } catch (error) {
            console.error("Error sending message:", error);
        }
    }

    public onMessageReceived(callback: (message: any) => void) {
        this.connection.on("ReceiveMessage", callback);
    }
}


const signalRService = new SignalRService();
export default signalRService;