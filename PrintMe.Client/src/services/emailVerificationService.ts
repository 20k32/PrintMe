import { baseApiService } from "./baseApiService"

export const verificationService = {
    async verifyEmail(token: string): Promise<void> {
        return baseApiService.patch(`/users/verifyemail?token=${encodeURIComponent(token)}`, {}, false)
    },
}

