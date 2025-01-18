import { authService } from "./authService";
import { baseApiService } from './baseApiService';
import { RequestData } from '../types/api';

interface RegistrationRequest extends RequestData {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
}

export const registrationService = {
  async register(credentials: RegistrationRequest) {
    await baseApiService.post('/auth/register', credentials);
    await authService.login({ email: credentials.email, password: credentials.password });
  }
};
