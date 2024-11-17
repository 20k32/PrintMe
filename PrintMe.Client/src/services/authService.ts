interface LoginRequest {
  name: string;
  role: string;
}

const API_BASE_URL = "http://localhost:5193/api";

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
