import { baseApiService } from './baseApiService';
import { RequestData } from '../types/api';
import { handleApiError } from '../utils/apiErrorHandler';

interface UpdateProfileRequest extends RequestData {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  userStatusId: number;
  shouldHidePhoneNumber: boolean;
  description: string;
  userRole: string;
  [key: string]: unknown;
}

export interface UserInfo {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string | null;
  shouldHidePhoneNumber: boolean;
  description: string;
}

export const profileService = {
  async updateProfile(credentials: UpdateProfileRequest) {
    try {
      return await baseApiService.put<void>('/users/user', credentials, true);
    } catch (error) {
      throw new Error(handleApiError(error, {
        badRequest: "Invalid profile data",
        default: "Failed to update profile"
      }));
    }
  },

  async fetchUserData(): Promise<UserInfo> {
    try {
      return await baseApiService.get<UserInfo>('/users/my', true);
    } catch (error) {
      throw new Error(handleApiError(error, {
        default: "Failed to load profile data"
      }));
    }
  }
};
