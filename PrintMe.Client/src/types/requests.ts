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

export interface JwtResult {
    accessToken: string;
    refreshToken: string;
}

export interface ChatResult {
    id: string;
    user1Id: number;
    user2Id: number;
    isArchived: boolean;
}

export interface Message {
    chatId: string;
    senderId: number;
    sendedDateTime: Date;
    payload: string;
}