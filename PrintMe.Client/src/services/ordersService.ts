import { baseApiService } from './baseApiService';
import { PrintOrderDto, UpdatePartialOrderRequest, CreateOrderRequest } from '../types/api';

export const ordersService = {
    async getMyOrders(): Promise<PrintOrderDto[]> {
        return baseApiService.get<PrintOrderDto[]>('/orders/my', true);
    },

    async getOrdersAsExecutor(): Promise<PrintOrderDto[]> {
        return baseApiService.get<PrintOrderDto[]>('/orders/forme', true);
    },

    abortOrder: async (orderId: number) => {
        return baseApiService.post(`/orders/Abort/${orderId}`, {}, true);
    },

    getOrderById: async (orderId: number) => {
        return baseApiService.get(`/orders/${orderId}`, true);
    },

    updateOrder: async (request: UpdatePartialOrderRequest): Promise<PrintOrderDto> => {
        return baseApiService.put('/orders/PartialUpdate', request, true);
    },

    createOrder: async (orderData: CreateOrderRequest) => {
        return baseApiService.post<CreateOrderRequest>('/orders', orderData, true);
    },

    acceptOrder: async (orderId: number) => {
        return baseApiService.post(`/orders/accept/${orderId}`, {}, true);
    },

    declineOrder: async (orderId: number) => {
        return baseApiService.post(`/orders/decline/${orderId}`, {}, true);
    },
    
    completeOrder: async (orderId: number) => {
        return baseApiService.post(`/orders/complete/${orderId}`, {}, true);
    },

};