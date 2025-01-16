import { baseApiService } from './baseApiService';
import { PrintOrderDto } from '../types/api';

export const ordersService = {
    async getMyOrders(): Promise<PrintOrderDto[]> {
        return baseApiService.get<PrintOrderDto[]>('/orders/my', true);
    },
    async getUserById(userId: number): Promise<{ firstName: string; lastName: string }> {
        return baseApiService.get<{ firstName: string; lastName: string }>(`/users/${userId}`, true);
    },
};