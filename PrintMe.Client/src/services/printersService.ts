import { baseApiService } from './baseApiService';
import { PrinterDto } from '../types/api';

export const printersService = {
    getMyPrinters: () => {
        return baseApiService.get<PrinterDto[]>('/printers/my', true);
    },

    deactivatePrinter: (printerId: number) => {
        return baseApiService.post<void>(`/printers/deactivate/${printerId}`, {}, true);
    },

    activatePrinter: (printerId: number) => {
        return baseApiService.post<void>(`/printers/activate/${printerId}`, {}, true);
    }
};
