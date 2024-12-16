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
    }
};