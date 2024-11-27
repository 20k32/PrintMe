import axios from "axios";
import { API_BASE_URL } from "../constants";
import { authService } from "./authService";

interface RegistrationRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export const register = async (credentials: RegistrationRequest): Promise<string> => {
  const response = await axios.post(`${API_BASE_URL}/auth/register`, credentials);
  if (response.status !== 200) {
    throw new Error("Registration failed");
  }
  return await authService.login({ email: credentials.email, password: credentials.password });
};

export const registerService = {
  register
};