import { PrintMaterial } from '../constants';

export type RequestData = Record<string, unknown>;

export interface ApiResponse<T> {
    value: T;
    message: string;
    statusCode: number;
}

export interface CreateOrderRequest extends RequestData {
    printerId: number;
    price: number;
    itemLink: string;
    dueDate: string;
    itemQuantity: number;
    itemDescription: string;
    itemMaterialId: number;
}

export interface SimplePrinterDto {
    id: number;
    modelName: string;
    materials: PrintMaterial[];
}

export interface MarkerDto {
    id: number;
    locationX: number;
    locationY: number;
}

export interface RoleDto {
    userRole: string;
    message: string;
}