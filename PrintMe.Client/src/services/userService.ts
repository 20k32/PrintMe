import { baseApiService } from './baseApiService';
import {UserInfo} from "./profileService.ts";


export const userService = {
    async getUserById(userId: number): Promise<UserInfo> {
        return baseApiService.get<UserInfo>(`/users/${userId}`, true);
    }
};