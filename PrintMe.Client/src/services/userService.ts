import { baseApiService } from './baseApiService';

export const userService = {
    async getUserFullNameById(userId: number): Promise<{ firstName: string; lastName: string }> {
        return baseApiService.get<{ firstName: string; lastName: string }>(`/users/${userId}`, true);
    },
    async getUserPrintersIds(): Promise<number[]> {
        const response = await baseApiService.get<{ id: number }[]>(`/printers/my`, true);
        return response.map((printer) => printer.id);
    },
    async getIsUserEmailVerified(): Promise<boolean> {
        return baseApiService.get<{ isVerified: boolean }>(`/users/my`, true).then((data) => data.isVerified);
    }
};