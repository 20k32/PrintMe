import axios, {AxiosRequestConfig} from 'axios';
import { API_BASE_URL } from '../constants';
import { ApiResponse, RequestData } from '../types/api';
import {JwtResult} from "../types/requests.ts";

const getToken = () => localStorage.getItem('token');
const getRefreshToken = () => localStorage.getItem('refreshToken');

const api = axios.create({
    baseURL: API_BASE_URL,
    withCredentials: true, 
});

const createAuthHeader = (): AxiosRequestConfig => ({
    headers: {
        'Authorization': `Bearer ${getToken()}`
    },
    withCredentials: true
});

export const baseApiService = {
    async get<T>(endpoint: string, requiresAuth: boolean = false, isArray: boolean = false): Promise<T> {
        const config = requiresAuth ? createAuthHeader() : {};
        const response = await api.get<ApiResponse<T>>(endpoint, config);
        if (isArray || Array.isArray(response.data)) {
            return response.data as unknown as T;
        }
        return response.data.value;
    },

    async post<T>(endpoint: string, data: RequestData | FormData, requiresAuth: boolean = false, isArray: boolean = false): Promise<T> {
        const config = requiresAuth ? createAuthHeader() : {};
        const response = await api.post<ApiResponse<T>>(endpoint, data, config);
        if (isArray || Array.isArray(response.data)) {
            return response.data as unknown as T;
        }
        return response.data.value;
    },

    async put<T>(endpoint: string, data: RequestData, requiresAuth: boolean = false, isArray: boolean = false): Promise<T> {
        const config = requiresAuth ? createAuthHeader() : {};
        const response = await api.put<ApiResponse<T>>(endpoint, data, config);
        if (isArray || Array.isArray(response.data)) {
            return response.data as unknown as T;
        }
        return response.data.value;
    },

    async delete<T>(endpoint: string, requiresAuth: boolean = false, isArray: boolean = false): Promise<T> {
        const config = requiresAuth ? createAuthHeader() : {};
        const response = await api.delete<ApiResponse<T>>(endpoint, config);
        if (isArray || Array.isArray(response.data)) {
            return response.data as unknown as T;
        }
        return response.data.value;
    },
    
    async patch<T>(endpoint: string, data: RequestData, requiresAuth: boolean = false, isArray: boolean = false): Promise<T> {
        const config = requiresAuth ? createAuthHeader() : {};
        const response = await api.patch<ApiResponse<T>>(endpoint, data, config);
        if (isArray || Array.isArray(response.data)) {
            return response.data as unknown as T;
        }
        return response.data.value;
    }
};

let isRefreshing = false;
let failedQueue: any[] = [];

const processQueue = (error: any, token: string | null = null) => {
    failedQueue.forEach((prom) => {
        if (token) {
            prom.resolve(token);
        } else {
            prom.reject(error);
        }
    });

    failedQueue = [];
};

const setToken = (token: string) => {
    localStorage.setItem("token", token);
};

api.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error.config;

        if (error.response?.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;

            if (isRefreshing) {
                return new Promise((resolve, reject) => {
                    failedQueue.push({ resolve, reject });
                })
                    .then((token) => {
                        originalRequest.headers["Authorization"] = `Bearer ${token}`;
                        return api(originalRequest);
                    })
                    .catch((err) => Promise.reject(err));
            }

            isRefreshing = true;

            try {
                
                const response = await api.post<ApiResponse<JwtResult>>(
                    "/auth/refreshtoken",
                    {
                        accessToken: getToken(),
                        refreshToken: getRefreshToken()
                    },
                    { withCredentials: true }
                );

                if(response.data.statusCode === 200)
                {
                    setToken(response.data.value.accessToken)
                    api.defaults.headers.common["Authorization"] = `Bearer ${response.data.value.accessToken}`;
                    processQueue(null, response.data.value.accessToken);
                }
                

                return api(originalRequest);
            } catch (refreshError) {
                processQueue(refreshError, null);
                return Promise.reject(refreshError);
            } finally {
                isRefreshing = false;
            }
        }

        return Promise.reject(error);
    }
);
