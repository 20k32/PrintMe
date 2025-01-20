import { baseApiService } from './baseApiService';
import { RoleDto } from '../types/api';

export const roleService = {
    async getMyRole(): Promise<RoleDto> {
        return baseApiService.get<RoleDto>('/roles/my', true);
    },
};