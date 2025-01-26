import { baseApiService } from './baseApiService';
import { RequestDto, PrinterApplicationDto } from '../types/requests';

export enum RequestType {
    None = 0,
    PrinterApplication = 2,
}

export const requestsService = {
    async getMyRequests(): Promise<RequestDto[]> {
        return baseApiService.get<RequestDto[]>('/request/my', true);
    },
    
    async submitPrinterApplication(printer: PrinterApplicationDto) {
        return baseApiService.post('/request/add', printer, true);
    },

    async approveRequest(requestId: number): Promise<void> {
        await baseApiService.post(`/request/${requestId}/approve`, {}, true);
    },

    async declineRequest(requestId: number, reason: string): Promise<void> {
        await baseApiService.post(`/request/${requestId}/decline?reason=${encodeURIComponent(reason)}`, {}, true);
    },

    async getAllRequests(): Promise<RequestDto[]> {
        return baseApiService.get<RequestDto[]>('/request/all/', true);
    }

};