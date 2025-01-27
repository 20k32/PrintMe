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
