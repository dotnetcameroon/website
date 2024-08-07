/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./**/*.{html,js,razor}"],
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: '#512BD4',
          dark: '#3B12C8'
        },
        secondary: {
          DEFAULT: '#4877CE',
          dark: '#B79918'
        }
      }
    },
  },
  plugins: [],
}
