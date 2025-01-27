import { baseApiService } from './baseApiService';
import { PrintOrderDto, UpdatePartialOrderRequest } from '../types/api';

const abortOrder = async (orderId: number) => {
  return baseApiService.post(`/orders/Abort/${orderId}`, {}, true);
};

const getOrderById = async (orderId: number) => {
  return baseApiService.get(`/orders/${orderId}`, true);
};

const updateOrder = async (request: UpdatePartialOrderRequest): Promise<PrintOrderDto> => {
  return baseApiService.put('/orders/PartialUpdate', request, true);
};

export const ordersService = {
    async getMyOrders(): Promise<PrintOrderDto[]> {
        return baseApiService.get<PrintOrderDto[]>('/orders/my', true);
    },
    async getOrdersAsExecutor(): Promise<PrintOrderDto[]> {
        return baseApiService.get<PrintOrderDto[]>('/orders/forme', true);
    },
    abortOrder,
    getOrderById,
    updateOrder,
};