import { baseApiService } from './baseApiService';
import { CreateOrderRequest } from '../types/api';

const createOrder = async (orderData: CreateOrderRequest) => {
  return baseApiService.post<CreateOrderRequest>('/orders', orderData, true);
};

export const orderService = {
  createOrder,
};
