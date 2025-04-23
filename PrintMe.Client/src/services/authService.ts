import { baseApiService } from './baseApiService';
import { RequestData } from '../types/api';
import { JwtResult } from '../types/requests';

interface LoginRequest extends RequestData {
  email: string;
  password: string;
}

class AuthenticationError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'AuthenticationError';
  }
}

export const authService = {
  async login(credentials: LoginRequest) {
    try {
      const jwtResult = await baseApiService.post<JwtResult>('/auth/login', credentials);
      if (!jwtResult.accessToken) {
        throw new AuthenticationError('No token received from server');
      }
      localStorage.setItem('token', jwtResult.accessToken);
      localStorage.setItem('refreshToken', jwtResult.refreshToken);
      window.location.reload();
    } catch (err: any) {
      let message = 'An unexpected error occurred';
      if (err?.response?.data?.message) {
        message = err.response.data.message; // Витягуємо повідомлення з відповіді сервера
      } else if (err?.message) {
        message = err.message; // Якщо помилка не з response, використовуємо її
      }
    
      throw new AuthenticationError(message);
    }
    },
  logout() {
    localStorage.removeItem("token");
    window.location.href = "/";
  },
  getToken() {
    return localStorage.getItem("token");
  },
  isLoggedIn(): boolean {
    const token = this.getToken();
    return !!token;
  }
};

