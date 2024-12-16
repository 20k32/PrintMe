import { Material } from '../constants';

export type RequestData = Record<string, unknown>;

export interface ApiResponse<T> {
    value: T;
    message: string;
    statusCode: number;
}

export interface SimplePrinterDto {
    id: number;
    modelName: string;
    materials: Material[];
}

export interface MarkerDto {
    id: number;
    locationX: number;
    locationY: number;
}
