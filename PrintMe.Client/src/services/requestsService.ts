
import axios from 'axios';
import { authService } from './authService';
import { API_BASE_URL } from "../constants";

export interface RequestDto {
    requestId: number;
    title: string | null;
    description: string | null;
    userTextData: string | null;
    requestTypeId: number;
    requestStatusId: number;
    userId: number;
    userSenderId: number;
}

interface ApiResponse {
    value: RequestDto[];
    message: string;
    statusCode: number;
}

export const requestsService = {
    async getMyRequests(): Promise<RequestDto[]> {
        const token = authService.getToken();
        const response = await axios.get<ApiResponse>(`${API_BASE_URL}/request/my`, {
            headers: {
                'Authorization': `Bearer ${token}`
            },
            withCredentials: true
        });

        if (response.data.statusCode === 200 && response.data.value) {
            return response.data.value;
        }
        throw new Error(response.data.message || 'Failed to load requests');
    }
};