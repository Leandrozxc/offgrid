/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./Components/**/*.{razor,html}"],
  theme: {
    extend: {
      colors: {
        ink: "#111111",
        cream: "#F5F1E8",
        concrete: "#858585",
        fog: "#E8E6E1",
      },
      fontFamily: {
        sans: ['"Helvetica Neue"', "Helvetica", "Arial", "ui-sans-serif", "system-ui", "sans-serif"],
      },
    },
  },
  plugins: [],
};