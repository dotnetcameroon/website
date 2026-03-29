import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],
  base: '/admin/',
  build: {
    outDir: '../app/wwwroot/admin',
    emptyOutDir: true,
  },
  server: {
    proxy: {
      '/api': 'https://localhost:5001',
      '/account': 'https://localhost:5001',
    },
  },
})
