import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5193',
        changeOrigin: true,
        secure: false,
      },
      '/message': {
        target: 'http://localhost:5193',
        changeOrigin: true,
        secure: false,
        ws: true,  // Important for WebSocket/SignalR support
      },
    },
    port: 5173
  },
});