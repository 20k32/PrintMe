import axios from 'axios';
import { API_BASE_URL } from "../constants";

interface LoginRequest {
  email: string;
  password: string;
}

export const authService = {
  async login(credentials: LoginRequest) {
    const response = await axios.post(`${API_BASE_URL}/auth/login`, credentials);
    const token = response.data.value;
    
    if (!token) {
      throw new Error("No token received from server");
    }
    
    localStorage.setItem("token", token);
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
