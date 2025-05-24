import { ReactNode } from 'react'
import { Navigate, useLocation } from 'react-router-dom'
import { useAuth } from '@/contexts/AuthContext'

interface ProtectedRouteProps {
  children: ReactNode
  requiredRole?: 'Admin' | 'Manager' | 'Employee'
  allowIncompleteProfile?: boolean
}

export default function ProtectedRoute({ children, requiredRole, allowIncompleteProfile = false }: ProtectedRouteProps) {
  const { state } = useAuth()
  const location = useLocation()

  if (!state.isAuthenticated) {
    // Redirect to login page with return url
    return <Navigate to="/login" state={{ from: location }} replace />
  }

  // Check if user needs to complete profile
  if (!allowIncompleteProfile && state.user && !state.user.isProfileComplete) {
    return <Navigate to="/complete-profile" replace />
  }

  if (requiredRole && state.user?.role !== requiredRole) {
    // Check role hierarchy: Admin > Manager > Employee
    const roleHierarchy = { Admin: 3, Manager: 2, Employee: 1 }
    const userLevel = roleHierarchy[state.user?.role || 'Employee']
    const requiredLevel = roleHierarchy[requiredRole]

    if (userLevel < requiredLevel) {
      return (
        <div className="min-h-screen flex items-center justify-center bg-gray-50">
          <div className="text-center">
            <h2 className="text-2xl font-bold text-gray-900 mb-4">Access Denied</h2>
            <p className="text-gray-600 mb-4">
              You don't have permission to access this page.
            </p>
            <p className="text-sm text-gray-500">
              Required role: {requiredRole} | Your role: {state.user?.role}
            </p>
          </div>
        </div>
      )
    }
  }

  return <>{children}</>
}
