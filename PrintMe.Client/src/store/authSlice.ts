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

export const login = createAsyncThunk(
  'auth/login',
  async (credentials: { email: string; password: string }, { rejectWithValue }) => {
    try {
      await authService.login(credentials);
      return true;
    } catch (err) {
      return rejectWithValue((err as Error).message);
    }
  }
);

export const logout = createAsyncThunk('auth/logout', async () => {
  authService.logout();
  return false;
});

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(login.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(login.fulfilled, (state) => {
        state.isLogined = true;
        state.loading = false;
      })
      .addCase(login.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(logout.fulfilled, (state) => {
        state.isLogined = false;
      });
  },
});

export default authSlice.reducer;