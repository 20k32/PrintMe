import axios from "axios";
import { API_BASE_URL } from "../constants";

interface UpdateProfileRequest {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  userStatusId: number;
  shouldHidePhoneNumber: boolean;
  description: string;
  userRole: string;
}

interface UserInfo {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string | null;
  shouldHidePhoneNumber: boolean;
  description: string;
}

interface PlainResponse {
  message: string;
  statusCode: number;
}
interface ApiResponse<T> extends PlainResponse{
  value: T;
}

export const profileService = {
  async updateProfile(credentials: UpdateProfileRequest, token: string) {
    try {
      const response = await axios.put(`${API_BASE_URL}/users/user`,credentials,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json-patch+json",
          },
        }
      );
      console.log("Response data:", response.data);
      return response.data;
    } catch (error) {
      if (axios.isAxiosError(error)) {
        console.error("API error:", error.response?.data || error.message);
      } else {
        console.error("Unexpected error:", error);
      }
      throw new Error("Profile update failed");
    }
  },

  async fetchUserData(token: string): Promise<UserInfo> {
    try {
      const response = await axios.get<ApiResponse<UserInfo>>(`${API_BASE_URL}/users/my`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      return response.data.value;
    } catch (error) {
      if (axios.isAxiosError(error)) {
        console.error("API error:", error.response?.data || error.message);
      } else {
        console.error("Unexpected error:", error);
      }
      throw new Error("Error fetching user data");
    }
  },
};
