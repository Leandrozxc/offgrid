/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./Components/**/*.{razor,html}"],
  theme: {
    extend: {
      colors: {
        ink: "#000000",       // Pure black
        cream: "#FFFFFF",     // Mapped to pure white globally
        fog: "#F5F5F5",       // Neutral studio light gray for media cards & section contrast
        concrete: "#737373",  // Neutral industrial mid-gray
      },
      fontFamily: {
        sans: ['"Helvetica Neue"', "Helvetica", "Arial", "ui-sans-serif", "system-ui", "sans-serif"],
      },
    },
  },
  plugins: [],
};