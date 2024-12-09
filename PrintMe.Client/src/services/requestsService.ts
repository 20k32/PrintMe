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

export enum RequestType {
    PrinterApplication = 1
}

export interface PrinterApplicationDto {
    description: string;
    minModelHeight: number;
    minModelWidth: number;
    maxModelHeight: number;
    maxModelWidth: number;
    locationX: number;
    locationY: number;
    materials: { materialId: number }[];
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

        return response.data.value;
   
    },
    async submitPrinterApplication(printer: PrinterApplicationDto) {
        const token = authService.getToken();
        await axios.post(`${API_BASE_URL}/request/add`, printer, {
            headers: {
                'Authorization': `Bearer ${token}`
            },
            withCredentials: true
        });
    }
};