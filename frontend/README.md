# Car Reservation System - Frontend

React + TypeScript + Vite frontend for the Car Reservation System.

## 🚀 Tech Stack

- **React 18** - UI Framework
- **TypeScript** - Type Safety
- **Vite** - Build Tool & Dev Server
- **Tailwind CSS** - Styling
- **React Query** - Server State Management
- **React Router** - Navigation
- **React Hook Form + Zod** - Form Handling & Validation
- **Axios** - HTTP Client
- **Lucide React** - Icons

## 📁 Project Structure

```
src/
├── components/         # Reusable UI components
├── pages/             # Page components
├── hooks/             # Custom React hooks
├── services/          # API service functions
├── types/             # TypeScript type definitions
├── utils/             # Utility functions
└── index.css          # Global styles
```

## 🛠️ Getting Started

### Prerequisites
- Node.js 18+ 
- npm or yarn

### Installation

1. Navigate to frontend directory:
```bash
cd frontend
```

2. Install dependencies:
```bash
npm install
```

3. Start development server:
```bash
npm run dev
```

4. Open [http://localhost:3000](http://localhost:3000)

## 🔧 Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build
- `npm run lint` - Run ESLint
- `npm run type-check` - Run TypeScript checking

## 🌟 Features

- **Car Management** - Browse, view, and manage cars
- **Reservation System** - Create and manage reservations
- **Real-time Data** - Auto-refreshing data with React Query
- **Responsive Design** - Mobile-first responsive UI
- **Type Safety** - Full TypeScript coverage
- **Form Validation** - Client-side validation with Zod

## 🔗 API Integration

Frontend connects to .NET API running on `http://localhost:5170`. 
Configure API URL in `.env` file:

```
VITE_API_URL=http://localhost:5170
```
