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

export const profileService = {
  async updateProfile(credentials: UpdateProfileRequest, token: string) {
    try {
      const response = await axios.put(
        `${API_BASE_URL}/users/user`, 
        credentials, 
        {
          headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': 'application/json-patch+json',
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
      const response = await axios.get(`${API_BASE_URL}/users/my`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.status === 200) {
        const user = response.data.value;
        return {
          userId: user.userId,
          firstName: user.firstName,
          lastName: user.lastName,
          email: user.email,
          phoneNumber: user.phoneNumber,
          shouldHidePhoneNumber: user.shouldHidePhoneNumber,
          description: user.description || "",
        };
      } else {
        throw new Error("Failed to fetch user data");
      }
    } catch (error) {
      console.error("Error fetching user data:", error);
      throw new Error("Error fetching user data");
    }
  },
};
