import { RequestData } from './api';

export enum RequestType {
    None = 0,
    PrinterApplication = 2,
}

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

export interface PrinterApplicationDto extends RequestData {
    printerModelId: number;
    description: string;
    minModelHeight: number;
    minModelWidth: number;
    maxModelHeight: number;
    maxModelWidth: number;
    locationX: number;
    locationY: number;
    materials: { printMaterialId: number }[];
}
