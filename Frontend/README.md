# Invoyz - Invoice Management System Frontend

A modern web application built with Vue.js 3 and Vuetify for managing customers, products, and invoices.

## Features

- Customer Management (CRUD operations)
- Product Management (CRUD operations)
- Invoice Management with line items
- Responsive design with Vuetify components
- Clean and intuitive UX
- Real-time calculations for invoice totals

## Prerequisites

- Node.js (v18 or higher)
- npm or yarn

## Installation

```bash
# Install dependencies
npm install

# Run development server
npm run dev

# Build for production
npm run build

# Preview production build
npm run preview
```

## API Configuration

The application connects to the backend API at `https://localhost:7139`. The proxy is configured in `vite.config.js` to handle CORS issues during development.

## Project Structure

```
src/
├── components/      # Reusable components
├── views/          # Page components
├── router/         # Vue Router configuration
├── services/       # API services
├── stores/         # Pinia stores
├── plugins/        # Plugin configurations (Vuetify)
└── main.js         # Application entry point
```

## Technologies Used

- Vue.js 3 - Progressive JavaScript Framework
- Vuetify 3 - Material Design Component Framework
- Vue Router - Official router for Vue.js
- Pinia - State management
- Axios - HTTP client
- Vite - Build tool
