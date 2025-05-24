import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '@/contexts/AuthContext'
import { User, Building, Phone, Calendar } from 'lucide-react'

export default function CompleteProfile() {
  const { state, updateProfile } = useAuth()
  const navigate = useNavigate()
  
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    phone: '',
    department: '',
  })
  
  const [isSubmitting, setIsSubmitting] = useState(false)

  useEffect(() => {
    if (!state.isAuthenticated) {
      navigate('/login')
      return
    }

    if (state.user?.isProfileComplete) {
      navigate('/')
      return
    }

    // Pre-fill with existing data if any
    if (state.user) {
      setFormData({
        firstName: state.user.firstName || '',
        lastName: state.user.lastName || '',
        phone: state.user.phone || '',
        department: state.user.department || '',
      })
    }
  }, [state.isAuthenticated, state.user, navigate])

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    })
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setIsSubmitting(true)

    try {
      await updateProfile({
        ...formData,
        isProfileComplete: true,
      })
      navigate('/')
    } catch (error) {
      console.error('Failed to update profile:', error)
    } finally {
      setIsSubmitting(false)
    }
  }

  const handleSkip = async () => {
    setIsSubmitting(true)
    try {
      await updateProfile({
        firstName: 'User',
        lastName: 'Name',
        isProfileComplete: true,
      })
      navigate('/')
    } catch (error) {
      console.error('Failed to skip profile setup:', error)
    } finally {
      setIsSubmitting(false)
    }
  }

  if (!state.user) {
    return null
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-md w-full space-y-8">
        <div>
          <div className="flex justify-center">
            <div className="w-16 h-16 bg-primary-600 text-white rounded-full flex items-center justify-center text-xl font-bold">
              <User className="h-8 w-8" />
            </div>
          </div>
          <h2 className="mt-6 text-center text-3xl font-bold text-gray-900">
            Complete your profile
          </h2>
          <p className="mt-2 text-center text-sm text-gray-600">
            Welcome to CarReserve! Let's set up your profile to get started.
          </p>
        </div>
        
        <form className="mt-8 space-y-6" onSubmit={handleSubmit}>
          <div className="space-y-4">
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label htmlFor="firstName" className="block text-sm font-medium text-gray-700">
                  First Name *
                </label>
                <input
                  id="firstName"
                  name="firstName"
                  type="text"
                  required
                  value={formData.firstName}
                  onChange={handleChange}
                  className="input mt-1"
                  placeholder="John"
                />
              </div>
              
              <div>
                <label htmlFor="lastName" className="block text-sm font-medium text-gray-700">
                  Last Name *
                </label>
                <input
                  id="lastName"
                  name="lastName"
                  type="text"
                  required
                  value={formData.lastName}
                  onChange={handleChange}
                  className="input mt-1"
                  placeholder="Doe"
                />
              </div>
            </div>

            <div>
              <label htmlFor="phone" className="block text-sm font-medium text-gray-700">
                <Phone className="inline h-4 w-4 mr-1" />
                Phone Number
              </label>
              <input
                id="phone"
                name="phone"
                type="tel"
                value={formData.phone}
                onChange={handleChange}
                className="input mt-1"
                placeholder="+1 (555) 123-4567"
              />
            </div>

            <div>
              <label htmlFor="department" className="block text-sm font-medium text-gray-700">
                <Building className="inline h-4 w-4 mr-1" />
                Department
              </label>
              <select
                id="department"
                name="department"
                value={formData.department}
                onChange={handleChange}
                className="input mt-1"
              >
                <option value="">Select a department</option>
                <option value="IT">IT</option>
                <option value="Operations">Operations</option>
                <option value="Sales">Sales</option>
                <option value="Marketing">Marketing</option>
                <option value="Finance">Finance</option>
                <option value="HR">Human Resources</option>
                <option value="Customer Service">Customer Service</option>
                <option value="Other">Other</option>
              </select>
            </div>
          </div>

          <div className="flex items-center space-x-2 text-sm text-gray-600">
            <Calendar className="h-4 w-4" />
            <span>Account created: {state.user.joinDate}</span>
          </div>

          <div className="space-y-3">
            <button
              type="submit"
              disabled={isSubmitting}
              className="btn btn-primary w-full"
            >
              {isSubmitting ? (
                <div className="flex items-center justify-center">
                  <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-white mr-2"></div>
                  Saving profile...
                </div>
              ) : (
                'Complete Profile'
              )}
            </button>

            <button
              type="button"
              onClick={handleSkip}
              disabled={isSubmitting}
              className="btn btn-outline w-full"
            >
              Skip for now
            </button>
          </div>
        </form>

        <div className="mt-6 text-center">
          <p className="text-xs text-gray-500">
            You can always update your profile later in the settings.
          </p>
        </div>
      </div>
    </div>
  )
}
