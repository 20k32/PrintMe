import axios from 'axios';

interface Material {
    name: string;
    printerId: number;
}

interface SimplePrinterDto {
    id: number;
    modelName: string;
    materials: Material[];
}

interface ApiResponseMin {
    value: SimplePrinterDto;
    message: string;
    statusCode: number;
}

export const printerService = {
    async getPrinterMinimalInfo(id: number): Promise<SimplePrinterDto> {
        const response = await axios.get<ApiResponseMin>(`http://localhost:5193/${id}?detailed=false`);
        return response.data.value;
    }
}