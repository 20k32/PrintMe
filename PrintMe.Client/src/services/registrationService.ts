import axios from "axios";
import { API_BASE_URL } from "../constants";
import { authService } from "./authService";

interface RegistrationRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export const registrationService = {
  async register(credentials: RegistrationRequest) {
    await axios.post(`${API_BASE_URL}/auth/register`, credentials);
    await authService.login({ email: credentials.email, password: credentials.password });
  }
};
