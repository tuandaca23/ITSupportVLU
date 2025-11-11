/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{vue,js,ts,jsx,tsx}'],
  theme: {
    extend: {
      fontFamily: {
        sans: [
          'Inter',
          'ui-sans-serif',
          'system-ui',
          'Segoe UI',
          'Roboto',
          'Helvetica Neue',
          'Arial',
          'Noto Sans',
          'Apple Color Emoji',
          'Segoe UI Emoji',
          'Segoe UI Symbol',
        ],
      },
      colors: {
        brand: {
          50: '#eef5ff',
          100: '#d9e9ff',
          200: '#b9d6ff',
          300: '#90bcff',
          400: '#5e98ff',
          500: '#3b82f6',
          600: '#2f6ee6',
          700: '#2859bf',
          800: '#234c9d',
          900: '#1f3f80',
        },
      },
      boxShadow: {
        'elev-1': '0 2px 8px rgba(0,0,0,0.06)',
        'elev-2': '0 10px 30px rgba(2, 6, 23, 0.15)',
      },
    },
  },
  plugins: [],
}
