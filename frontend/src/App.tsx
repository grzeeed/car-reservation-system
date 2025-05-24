import { Routes, Route, Navigate } from 'react-router-dom'
import { AuthProvider } from '@/contexts/AuthContext'
import Layout from '@/components/Layout'
import ProtectedRoute from '@/components/auth/ProtectedRoute'
import Login from '@/pages/auth/Login'
import Register from '@/pages/auth/Register'
import CompleteProfile from '@/pages/auth/CompleteProfile'
import Dashboard from '@/pages/Dashboard'
import CarList from '@/pages/cars/CarList'
import CarDetails from '@/pages/cars/CarDetails'
import CreateCar from '@/pages/cars/CreateCar'
import ReservationList from '@/pages/reservations/ReservationList'
import CreateReservation from '@/pages/reservations/CreateReservation'
import TestAPI from '@/pages/TestAPI'

function App() {
  return (
    <AuthProvider>
      <Layout>
        <Routes>
          {/* Public routes */}
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          
          {/* Semi-protected routes (for authenticated users with incomplete profiles) */}
          <Route path="/complete-profile" element={
            <ProtectedRoute allowIncompleteProfile>
              <CompleteProfile />
            </ProtectedRoute>
          } />
          
          {/* Protected routes */}
          <Route path="/" element={
            <ProtectedRoute>
              <Dashboard />
            </ProtectedRoute>
          } />
          
          <Route path="/cars" element={
            <ProtectedRoute>
              <CarList />
            </ProtectedRoute>
          } />
          
          <Route path="/cars/:id" element={
            <ProtectedRoute>
              <CarDetails />
            </ProtectedRoute>
          } />
          
          <Route path="/cars/create" element={
            <ProtectedRoute requiredRole="Manager">
              <CreateCar />
            </ProtectedRoute>
          } />
          
          <Route path="/reservations" element={
            <ProtectedRoute>
              <ReservationList />
            </ProtectedRoute>
          } />
          
          <Route path="/reservations/create" element={
            <ProtectedRoute>
              <CreateReservation />
            </ProtectedRoute>
          } />
          
          <Route path="/test-api" element={
            <ProtectedRoute requiredRole="Admin">
              <TestAPI />
            </ProtectedRoute>
          } />
          
          {/* Catch-all route - redirect to dashboard */}
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </Layout>
    </AuthProvider>
  )
}

export default App
