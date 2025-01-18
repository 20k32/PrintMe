import axios, { AxiosRequestConfig } from 'axios';
import { API_BASE_URL } from '../constants';
import { ApiResponse, RequestData } from '../types/api';

const getToken = () => localStorage.getItem('token');

const createAuthHeader = (): AxiosRequestConfig => ({
    headers: {
        'Authorization': `Bearer ${getToken()}`
    },
    withCredentials: true
});

export const baseApiService = {
    async get<T>(endpoint: string, requiresAuth: boolean = false, isArray: boolean = false): Promise<T> {
        const config = requiresAuth ? createAuthHeader() : {};
        const response = await axios.get<ApiResponse<T>>(`${API_BASE_URL}${endpoint}`, config);
        if (isArray || Array.isArray(response.data)) {
            return response.data as unknown as T;
        }
        return response.data.value;
    },

    async post<T>(endpoint: string, data: RequestData | FormData, requiresAuth: boolean = false, isArray: boolean = false): Promise<T> {
        const config = requiresAuth ? createAuthHeader() : {};
        const response = await axios.post<ApiResponse<T>>(`${API_BASE_URL}${endpoint}`, data, config);
        if (isArray || Array.isArray(response.data)) {
            return response.data as unknown as T;
        }
        return response.data.value;
    },

    async put<T>(endpoint: string, data: RequestData, requiresAuth: boolean = false, isArray: boolean = false): Promise<T> {
        const config = requiresAuth ? createAuthHeader() : {};
        const response = await axios.put<ApiResponse<T>>(`${API_BASE_URL}${endpoint}`, data, config);
        if (isArray || Array.isArray(response.data)) {
            return response.data as unknown as T;
        }
        return response.data.value;
    },

    async delete<T>(endpoint: string, requiresAuth: boolean = false, isArray: boolean = false): Promise<T> {
        const config = requiresAuth ? createAuthHeader() : {};
        const response = await axios.delete<ApiResponse<T>>(`${API_BASE_URL}${endpoint}`, config);
        if (isArray || Array.isArray(response.data)) {
            return response.data as unknown as T;
        }
        return response.data.value;
    }
};
