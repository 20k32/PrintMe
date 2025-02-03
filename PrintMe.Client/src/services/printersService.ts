import { baseApiService } from './baseApiService';
import { PrinterDto, SimplePrinterDto } from '../types/api';
import { PrinterModel, PrintMaterial } from '../constants';
import { API_BASE_URL } from '../constants';
import axios from 'axios';

export const printersService = {
    getMyPrinters: () => {
        return baseApiService.get<PrinterDto[]>('/printers/my', true);
    },

    deactivatePrinter: (printerId: number) => {
        return baseApiService.post<void>(`/printers/deactivate/${printerId}`, {}, true);
    },

    activatePrinter: (printerId: number) => {
        return baseApiService.post<void>(`/printers/activate/${printerId}`, {}, true);
    },
    getPrinterById(id: number): Promise<PrinterDto> {
        return baseApiService.get<PrinterDto>(`/Printers/${id}?detailed=true`);
    }, 
    getPrinter: (id: string) => baseApiService.get<SimplePrinterDto>(`/printers/${id}`, true, false),

    async getPrinterMinimalInfo(id: number): Promise<SimplePrinterDto> {
        return baseApiService.get<SimplePrinterDto>(`/Printers/${id}?detailed=false`);
    },

    async getMaterials(): Promise<PrintMaterial[]> {
        const response = await axios.get(`${API_BASE_URL}/Printers/materials`);
        return response.data;
    },

    async getModels(): Promise<PrinterModel[]> {
        const response = await axios.get(`${API_BASE_URL}/Printers/models`);
        return response.data;
    }
};
