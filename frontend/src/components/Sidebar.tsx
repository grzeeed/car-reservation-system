import { Link, useLocation } from 'react-router-dom'
import { 
  LayoutDashboard, 
  Car, 
  Calendar,
  Plus,
  Users,
  Zap
} from 'lucide-react'
import { cn } from '@/utils/helpers'

const navigation = [
  { name: 'Dashboard', href: '/', icon: LayoutDashboard },
  { name: 'Cars', href: '/cars', icon: Car },
  { name: 'Reservations', href: '/reservations', icon: Calendar },
  { name: 'Customers', href: '/customers', icon: Users },
  { name: 'Test API', href: '/test-api', icon: Zap },
]

const quickActions = [
  { name: 'Add Car', href: '/cars/create', icon: Plus },
  { name: 'New Reservation', href: '/reservations/create', icon: Plus },
]

export default function Sidebar() {
  const location = useLocation()

  return (
    <div className="hidden md:flex md:w-64 md:flex-col">
      <div className="flex flex-col flex-grow pt-5 bg-white border-r border-gray-200 overflow-y-auto">
        <div className="flex flex-col flex-grow">
          {/* Main Navigation */}
          <nav className="flex-1 px-4 pb-4 space-y-1">
            <div className="space-y-1">
              {navigation.map((item) => {
                const isActive = location.pathname === item.href || 
                  (item.href !== '/' && location.pathname.startsWith(item.href))
                
                return (
                  <Link
                    key={item.name}
                    to={item.href}
                    className={cn(
                      'group flex items-center px-2 py-2 text-sm font-medium rounded-md transition-colors',
                      isActive
                        ? 'bg-primary-50 text-primary-600 border-r-2 border-primary-600'
                        : 'text-gray-600 hover:bg-gray-50 hover:text-gray-900'
                    )}
                  >
                    <item.icon
                      className={cn(
                        'mr-3 h-5 w-5 flex-shrink-0',
                        isActive ? 'text-primary-600' : 'text-gray-400 group-hover:text-gray-500'
                      )}
                    />
                    {item.name}
                  </Link>
                )
              })}
            </div>
            
            {/* Quick Actions */}
            <div className="pt-6">
              <div className="px-2 text-xs font-semibold text-gray-500 uppercase tracking-wider">
                Quick Actions
              </div>
              <div className="mt-2 space-y-1">
                {quickActions.map((item) => (
                  <Link
                    key={item.name}
                    to={item.href}
                    className="group flex items-center px-2 py-2 text-sm font-medium text-gray-600 rounded-md hover:bg-gray-50 hover:text-gray-900 transition-colors"
                  >
                    <item.icon className="mr-3 h-4 w-4 text-gray-400 group-hover:text-gray-500" />
                    {item.name}
                  </Link>
                ))}
              </div>
            </div>
          </nav>
        </div>
      </div>
    </div>
  )
}
