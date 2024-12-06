import axios from 'axios';
import { API_BASE_URL } from "../constants";

interface LoginRequest {
  email: string;
  password: string;
}

export const authService = {
  async login(credentials: LoginRequest): Promise<string> {
    const response = await axios.post(`${API_BASE_URL}/auth/login`, credentials);
    const token = response.data.value;
    if (!token) {
      throw new Error("Invalid token");
    }
    localStorage.setItem("token", token);
    return token;
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
