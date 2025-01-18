
import axios from 'axios';
import { authService } from '../services/authService';

export interface ErrorMessages {
  unauthorized?: string;
  notFound?: string;
  conflict?: string;
  badRequest?: string;
  default?: string;
}

export const handleApiError = (error: unknown, messages: ErrorMessages = {}): string => {
  if (axios.isAxiosError(error)) {
    const defaultMessages = {
      unauthorized: authService.getToken() 
        ? "Your session has expired, please log in again"
        : "You must be logged in to perform this action",
      notFound: "Resource not found",
      conflict: "Resource already exists",
      badRequest: "Invalid request. Please check your input",
      default: "An error occurred. Please try again later"
    };

    const finalMessages = { ...defaultMessages, ...messages };

    switch (error.response?.status) {
      case 400:
        return finalMessages.badRequest;
      case 401:
      case 403:
        return finalMessages.unauthorized;
      case 404:
        return finalMessages.notFound;
      case 409:
        return finalMessages.conflict;

      default:
        return finalMessages.default;
    }
  }
  return "An unexpected error occurred";
};