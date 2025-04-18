import { baseApiService } from './baseApiService';
import { UserProfileDto } from '../types/api';
import {UserInfo} from "./profileService.ts";

export const userService = {

    async getUserById(userId: number): Promise<UserInfo> {
        return baseApiService.get<UserInfo>(`/users/${userId}`, true);
    },
    async getUserFullNameById(userId: number): Promise<{ firstName: string; lastName: string }> {
        return baseApiService.get<{ firstName: string; lastName: string }>(`/users/${userId}`, true);
    },
    async getUserPrintersIds(): Promise<number[]> {
        const response = await baseApiService.get<{ id: number }[]>(`/printers/my`, true);
        return response.map((printer) => printer.id);
    },
    async getIsUserEmailVerified(): Promise<boolean> {
        return baseApiService.get<{ isVerified: boolean }>(`/users/my`, true).then((data) => data.isVerified);
    },
    async getUserProfile(userId: number): Promise<UserProfileDto> {
        return baseApiService.get<UserProfileDto>(`/users/${userId}`, true);
      }
};