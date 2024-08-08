/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./app/**/*.{html,js,razor}"],
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: '#512BD4',
          dark: '#3B12C8',
          accentuation: '#7352E7',
        },
        secondary: {
          DEFAULT: '#EBC006',
          dark: '#B79918',
          accentuation: '#F9D32C',
        },
        tertiary: {
          DEFAULT: '#0AA072',
          dark: '#0A855F',
          accentuation: '#0A855F',
        },
      },
      fontFamily: {
        special: ['Space Grotesk'],
        heading: ['Lexend'],
      },
      fontSize: {
        'heading-1': '2.25rem', // 36px
        'heading-2': '1.875rem', // 30px
        'heading-3': '1.5rem', // 24px
        'heading-4': '1.25rem', // 20px
        'heading-5': '1rem', // 16px
        'heading-6': '0.875rem', // 14px
      },
      lineHeight: {
        'heading-1': '1.2',
        'heading-2': '1.25',
        'heading-3': '1.3',
        'heading-4': '1.35',
        'heading-5': '1.4',
        'heading-6': '1.5',
      },
      margin: {
        'heading-1': '1rem', // 16px
        'heading-2': '0.875rem', // 14px
        'heading-3': '0.75rem', // 12px
        'heading-4': '0.625rem', // 10px
        'heading-5': '0.5rem', // 8px
        'heading-6': '0.375rem', // 6px
      },
    },
  },
  plugins: [],
}
