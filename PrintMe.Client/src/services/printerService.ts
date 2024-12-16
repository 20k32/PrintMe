import { Material } from '../constants';
import { baseApiService } from './baseApiService';
import { SimplePrinterDto } from '../types/api';
import { API_BASE_URL } from '../constants';
import axios from 'axios';

export const printerService = {
    getPrinter: (id: string) => baseApiService.get<SimplePrinterDto>(`/printers/${id}`, true, false),
    async getPrinterMinimalInfo(id: number): Promise<SimplePrinterDto> {
        return baseApiService.get<SimplePrinterDto>(`/Printers/${id}?detailed=false`);
    },
    async getMaterials(): Promise<Material[]> {
        const response = await axios.get(`${API_BASE_URL}/Printers/materials`);
        return response.data;
    }
}