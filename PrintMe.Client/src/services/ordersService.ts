import { baseApiService } from './baseApiService';
import { PrintOrderDto } from '../types/api';

export const ordersService = {
    async getMyOrders(): Promise<PrintOrderDto[]> {
        return baseApiService.get<PrintOrderDto[]>('/orders/my', true);
    },
    async getOrdersAsExecutor(): Promise<PrintOrderDto[]> {
        return baseApiService.get<PrintOrderDto[]>('/orders/forme', true);
    }
};