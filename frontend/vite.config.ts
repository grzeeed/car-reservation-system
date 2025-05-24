import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import path from 'path'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },
  server: {
    host: true, // Allow external connections (needed for Docker)
    port: 5173,
    proxy: {
      '/api': {
        target: process.env.NODE_ENV === 'development' && process.env.DOCKER 
          ? 'http://api:80' 
          : 'http://localhost:5170',
        changeOrigin: true,
        secure: false,
      },
      '/health': {
        target: process.env.NODE_ENV === 'development' && process.env.DOCKER 
          ? 'http://api:80' 
          : 'http://localhost:5170',
        changeOrigin: true,
        secure: false,
      },
      '/swagger': {
        target: process.env.NODE_ENV === 'development' && process.env.DOCKER 
          ? 'http://api:80' 
          : 'http://localhost:5170',
        changeOrigin: true,
        secure: false,
      }
    }
  },
  preview: {
    host: true,
    port: 4173
  }
})
