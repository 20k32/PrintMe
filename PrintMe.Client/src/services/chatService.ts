import { baseApiService } from './baseApiService';
import {ChatResult, Message, SendMessageToChatRequest} from '../types/requests';
export const chatService = {
    async getMineChats() {
        
        let result: ChatResult[] = [];
        
        try {
            result = await baseApiService.get<ChatResult[]>('/chat/getMineChats');
        } catch (err) {
            console.error('chat receiving failed:', err);
        }
        
        return result || [];
    },
    
    async getMessages(chatId: string, chatMateId: number) {
        let result: Message[] = [];
        
        try{
            result = await baseApiService.get<Message[]>(`/chat/getChatMessagesForMe/${chatId}/${chatMateId}`);
        } catch(err){
            console.error('getting chat messages failed:', err);
        }
        
        return result || [];
    },
    
    async sendMessage(request: SendMessageToChatRequest){
        try{
            const result = await baseApiService.put<SendMessageToChatRequest>('/chat/sendMessage', request);
            
            if(result === null || result === undefined)
            {
                throw new Error();
            }
        }
        catch(err){
            console.error('error adding message to db:', err);
        }
    }
}