import { baseApiService } from './baseApiService';

export const userService = {
    async getUserFullNameById(userId: number): Promise<{ firstName: string; lastName: string }> {
        return baseApiService.get<{ firstName: string; lastName: string }>(`/users/${userId}`, true);
    },
    async getUserPrintersIds(): Promise<number[]> {
        const response = await baseApiService.get<{ id: number }[]>(`/printers/my`, true);
        return response.map((printer) => printer.id);
    }
};