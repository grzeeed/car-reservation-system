import { createContext, useContext, useReducer, ReactNode, useEffect } from 'react'

// Types
export interface User {
  id: string
  email: string
  firstName: string
  lastName: string
  role: 'Admin' | 'Manager' | 'Employee'
  avatar?: string
  phone?: string
  department?: string
  joinDate?: string
  isProfileComplete: boolean
}

export interface AuthState {
  user: User | null
  isAuthenticated: boolean
  isLoading: boolean
  error: string | null
}

export type AuthAction =
  | { type: 'LOGIN_START' }
  | { type: 'LOGIN_SUCCESS'; payload: User }
  | { type: 'LOGIN_FAILURE'; payload: string }
  | { type: 'REGISTER_START' }
  | { type: 'REGISTER_SUCCESS'; payload: User }
  | { type: 'REGISTER_FAILURE'; payload: string }
  | { type: 'UPDATE_PROFILE'; payload: Partial<User> }
  | { type: 'LOGOUT' }
  | { type: 'CLEAR_ERROR' }
  | { type: 'SET_LOADING'; payload: boolean }

// Initial state
const initialState: AuthState = {
  user: null,
  isAuthenticated: false,
  isLoading: false,
  error: null,
}

// Reducer
function authReducer(state: AuthState, action: AuthAction): AuthState {
  switch (action.type) {
    case 'LOGIN_START':
    case 'REGISTER_START':
      return {
        ...state,
        isLoading: true,
        error: null,
      }
    case 'LOGIN_SUCCESS':
    case 'REGISTER_SUCCESS':
      return {
        ...state,
        user: action.payload,
        isAuthenticated: true,
        isLoading: false,
        error: null,
      }
    case 'LOGIN_FAILURE':
    case 'REGISTER_FAILURE':
      return {
        ...state,
        user: null,
        isAuthenticated: false,
        isLoading: false,
        error: action.payload,
      }
    case 'UPDATE_PROFILE':
      return {
        ...state,
        user: state.user ? { ...state.user, ...action.payload } : null,
      }
    case 'LOGOUT':
      return {
        ...state,
        user: null,
        isAuthenticated: false,
        isLoading: false,
        error: null,
      }
    case 'CLEAR_ERROR':
      return {
        ...state,
        error: null,
      }
    case 'SET_LOADING':
      return {
        ...state,
        isLoading: action.payload,
      }
    default:
      return state
  }
}

// Context
const AuthContext = createContext<{
  state: AuthState
  dispatch: React.Dispatch<AuthAction>
  login: (email: string, password: string) => Promise<void>
  register: (email: string, password: string, confirmPassword: string) => Promise<void>
  updateProfile: (profileData: Partial<User>) => Promise<void>
  logout: () => void
  clearError: () => void
} | null>(null)

// Mock users for development
const MOCK_USERS: Array<User & { password: string }> = [
  {
    id: '1',
    email: 'admin@carreserve.com',
    password: 'admin123',
    firstName: 'John',
    lastName: 'Admin',
    role: 'Admin',
    phone: '+1 (555) 123-4567',
    department: 'IT',
    joinDate: '2023-01-15',
    isProfileComplete: true,
  },
  {
    id: '2',
    email: 'manager@carreserve.com',
    password: 'manager123',
    firstName: 'Jane',
    lastName: 'Manager',
    role: 'Manager',
    phone: '+1 (555) 234-5678',
    department: 'Operations',
    joinDate: '2023-03-10',
    isProfileComplete: true,
  },
  {
    id: '3',
    email: 'employee@carreserve.com',
    password: 'employee123',
    firstName: 'Bob',
    lastName: 'Employee',
    role: 'Employee',
    phone: '+1 (555) 345-6789',
    department: 'Sales',
    joinDate: '2023-06-20',
    isProfileComplete: true,
  },
  // Test user with incomplete profile
  {
    id: '4',
    email: 'test@carreserve.com',
    password: 'test123',
    firstName: '',
    lastName: '',
    role: 'Employee',
    joinDate: new Date().toISOString().split('T')[0],
    isProfileComplete: false,
  },
]

// Simulate user storage
let mockUserStorage = [...MOCK_USERS]

// Provider
export function AuthProvider({ children }: { children: ReactNode }) {
  const [state, dispatch] = useReducer(authReducer, initialState)

  // Check for stored auth on mount
  useEffect(() => {
    const storedUser = localStorage.getItem('auth-user')
    if (storedUser) {
      try {
        const user = JSON.parse(storedUser)
        dispatch({ type: 'LOGIN_SUCCESS', payload: user })
      } catch (error) {
        localStorage.removeItem('auth-user')
      }
    }
  }, [])

  const login = async (email: string, password: string): Promise<void> => {
    dispatch({ type: 'LOGIN_START' })

    // Simulate API delay
    await new Promise(resolve => setTimeout(resolve, 1000))

    const user = mockUserStorage.find(u => u.email === email && u.password === password)

    if (user) {
      const { password: _, ...userWithoutPassword } = user
      localStorage.setItem('auth-user', JSON.stringify(userWithoutPassword))
      dispatch({ type: 'LOGIN_SUCCESS', payload: userWithoutPassword })
    } else {
      dispatch({ type: 'LOGIN_FAILURE', payload: 'Invalid email or password' })
      throw new Error('Invalid email or password')
    }
  }

  const register = async (email: string, password: string, confirmPassword: string): Promise<void> => {
    dispatch({ type: 'REGISTER_START' })

    // Simulate API delay
    await new Promise(resolve => setTimeout(resolve, 1000))

    // Validation
    if (password !== confirmPassword) {
      dispatch({ type: 'REGISTER_FAILURE', payload: 'Passwords do not match' })
      throw new Error('Passwords do not match')
    }

    if (password.length < 6) {
      dispatch({ type: 'REGISTER_FAILURE', payload: 'Password must be at least 6 characters' })
      throw new Error('Password must be at least 6 characters')
    }

    // Check if user already exists
    if (mockUserStorage.find(u => u.email === email)) {
      dispatch({ type: 'REGISTER_FAILURE', payload: 'User with this email already exists' })
      throw new Error('User with this email already exists')
    }

    // Create new user
    const newUser: User & { password: string } = {
      id: Date.now().toString(),
      email,
      password,
      firstName: '',
      lastName: '',
      role: 'Employee', // Default role
      isProfileComplete: false,
      joinDate: new Date().toISOString().split('T')[0],
    }

    mockUserStorage.push(newUser)
    
    const { password: _, ...userWithoutPassword } = newUser
    localStorage.setItem('auth-user', JSON.stringify(userWithoutPassword))
    dispatch({ type: 'REGISTER_SUCCESS', payload: userWithoutPassword })
  }

  const updateProfile = async (profileData: Partial<User>): Promise<void> => {
    if (!state.user) throw new Error('No user logged in')

    dispatch({ type: 'SET_LOADING', payload: true })

    // Simulate API delay
    await new Promise(resolve => setTimeout(resolve, 500))

    const updatedUser = { ...state.user, ...profileData }
    
    // Update in mock storage
    const userIndex = mockUserStorage.findIndex(u => u.id === state.user!.id)
    if (userIndex !== -1) {
      mockUserStorage[userIndex] = { ...mockUserStorage[userIndex], ...profileData }
    }

    // Update localStorage
    localStorage.setItem('auth-user', JSON.stringify(updatedUser))
    
    dispatch({ type: 'UPDATE_PROFILE', payload: profileData })
    dispatch({ type: 'SET_LOADING', payload: false })
  }

  const logout = () => {
    localStorage.removeItem('auth-user')
    dispatch({ type: 'LOGOUT' })
  }

  const clearError = () => {
    dispatch({ type: 'CLEAR_ERROR' })
  }

  return (
    <AuthContext.Provider
      value={{
        state,
        dispatch,
        login,
        register,
        updateProfile,
        logout,
        clearError,
      }}
    >
      {children}
    </AuthContext.Provider>
  )
}

// Hook
export function useAuth() {
  const context = useContext(AuthContext)
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider')
  }
  return context
}
