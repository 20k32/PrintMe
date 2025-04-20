import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { authService } from '../services/authService';

interface AuthState {
  isLogined: boolean;
  loading: boolean;
  error: string | null;
}

const initialState: AuthState = {
  isLogined: authService.isLoggedIn(),
  loading: false,
  error: null,
};

interface LoginError {
  message: string;
  statusCode?: number;
}

// Login action
export const login = createAsyncThunk<
  boolean, 
  { email: string; password: string }, 
  { rejectValue: LoginError }
>(
  'auth/login',
  async (credentials, { rejectWithValue }) => {
    try {
      await authService.login(credentials); // Call the login service
      return true;
    } catch (err: any) {
      let errorMessage = err.message || 'An unexpected error occurred.';
      console.error('Login error:', errorMessage);
      return rejectWithValue({
        message: errorMessage,
        statusCode: err?.response?.status,
      });
    }    
  }
);

// Logout action
export const logout = createAsyncThunk('auth/logout', async () => {
  await authService.logout(); // Call the logout service
});

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    clearError(state) {
      state.error = null; // Clear the error state
    },
  },
  extraReducers: (builder) => {
    builder
      // Handle login
      .addCase(login.pending, (state) => {
        state.loading = true;
        state.error = null; // Clear previous errors
      })
      .addCase(login.fulfilled, (state) => {
        state.isLogined = true;
        state.loading = false;
      })
      .addCase(login.rejected, (state, action) => {
        state.loading = false;

        // Handle different types of error responses more specifically
        if (action.payload?.statusCode === 403) {
          state.error = action.payload.message || "Incorrect password. Please try again."; // Handle wrong password specifically
        } else {
          state.error = action.payload?.message || "An unexpected error occurred.";
        }
      })

      // Handle logout
      .addCase(logout.fulfilled, (state) => {
        state.isLogined = false;
        state.error = null; // Clear errors on logout
      });
  },
});

export const { clearError } = authSlice.actions; // Export the clearError action
export default authSlice.reducer; // Export the reducer