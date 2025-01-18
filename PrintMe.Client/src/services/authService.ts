import { baseApiService } from './baseApiService';
import { RequestData } from '../types/api';

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
      const token = await baseApiService.post<string>('/auth/login', credentials);
      if (!token) {
        throw new AuthenticationError('No token received from server');
      }
      localStorage.setItem('token', token);
    } catch (err) {
      console.error('Login failed:', err);
      throw new AuthenticationError('Login failed');
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
