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
    isDeactivated?: boolean;
}

export interface PrinterDto extends SimplePrinterDto {
    description: string;
    minModelHeight: number;
    minModelWidth: number;
    maxModelHeight: number;
    maxModelWidth: number;
    locationX: number;
    locationY: number;
    userId: number;
}

export interface MarkerDto {
    id: number;
    locationX: number;
    locationY: number;
}

export interface PrintOrderDto {
    printOrderId: number;
    userId: number;
    executorId: number;
    printerId: number;
    price: number;
    startDate: string;
    dueDate: string;
    itemLink: string | undefined;
    itemQuantity: number;
    itemDescription: string | undefined;
    itemMaterialId: number;
    printOrderStatusId: number;
    printOrderStatusReasonId: number;
}

export interface UpdatePartialOrderRequest extends RequestData {
  orderId: number;
  itemDescription?: string;
  itemQuantity?: number;
  itemMaterialId?: number;
  itemLink?: string;
  dueDate?: string;
  price?: number;
  [key: string]: unknown;
  }

export interface RoleDto {
    userRole: string;
    message: string;
}