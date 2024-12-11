import axios from 'axios';
import {authService} from './authService';
import {API_BASE_URL} from "../constants";

export interface RequestType {
    id: number,
    type: string
}

export interface RequestStatus {
    id: number,
    status: string
}
export interface RequestDto {
    requestId: number;
    title: string | null;
    description: string | null;
    userTextData: string | null;
    requestType: RequestType;
    requestStatus: RequestStatus;
    userId: number;
    userSenderId: number;
    
    isReadonly: boolean
    
    // printer request data
    minModelHeight: number;
    minModelWidth: number;
    maxModelHeight: number;
    maxModelWidht: number;
    locationX: number;
    locationY: number;
    materials: {
        printMaterialId: number,
        name: string
    }
    //
}

interface PlainResponse {
    message: string;
    statusCode: number;
}
interface ApiResponse<T> extends PlainResponse{
    value: T;
}

export const requestsService = {
    async getMyRequests(): Promise<RequestDto[]> {
        const token = authService.getToken();
        const response = await axios.get<ApiResponse<RequestDto[]>>(`${API_BASE_URL}/request/my/`, {
            headers: {
                'Authorization': `Bearer ${token}`
            },
            withCredentials: true
        });
        
        if (response.data.statusCode === 200 && response.data.value) {
            return response.data.value;
        }
        throw new Error(response.data.message || 'Failed to load requests');
    },
    
    async getAllRequests(): Promise<RequestDto[]> {
        const token = authService.getToken();
        const response = await axios.get<ApiResponse<RequestDto[]>>(`${API_BASE_URL}/request/all/`, {
            headers: {
                'Authorization': `Bearer ${token}`
            },
            withCredentials: true
        });

        if (response.data.statusCode === 200 && response.data.value) {
            return response.data.value;
        }
        throw new Error(response.data.message || 'Failed to load requests');
    },
    
    async approveRequest(requestId: number): Promise<ApiResponse<PlainResponse>> {
        const response =  await axios.post<ApiResponse<PlainResponse>>(`${API_BASE_URL}/request/${requestId}/approve`, null,  {
            headers: {
                'Authorization': `Bearer ${authService.getToken()}`
            },
            withCredentials: true
        });
        
        return response.data;
    },

    async declineRequest(requestId: number): Promise<ApiResponse<PlainResponse>> {
        const response =  await axios.post<ApiResponse<PlainResponse>>(`${API_BASE_URL}/request/${requestId}/decline`, null,  {
            headers: {
                'Authorization': `Bearer ${authService.getToken()}`
            },
            withCredentials: true
        });

        return response.data;
    }
};