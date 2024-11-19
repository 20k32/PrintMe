import { API_BASE_URL } from "../constants";

interface LoginRequest {
  name: string;
  role: string;
}

export const authService = {
  async login(credentials: LoginRequest): Promise<string> {
    const response = await fetch(`${API_BASE_URL}/Authorization/login`, {
      method: "POST",
      headers: {
        Accept: "*/*",
        "Content-Type": "application/json",
      },
      body: JSON.stringify(credentials),
    });

    if (!response.ok) {
      throw new Error("Login failed");
    }

    const data = await response.json();
    return data;
  },
};
