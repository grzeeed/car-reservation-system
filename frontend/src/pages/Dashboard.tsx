import { useGetAllCars } from '@/hooks/useCars'
import { Car, Calendar, TrendingUp, Users } from 'lucide-react'
import { formatCurrency } from '@/utils/helpers'

export default function Dashboard() {
  const { data: cars = [], isLoading } = useGetAllCars()

  const stats = {
    totalCars: cars.length,
    availableCars: cars.filter(car => car.status === 'Available').length,
    reservedCars: cars.filter(car => car.status === 'Reserved').length,
    totalRevenue: cars.reduce((sum, car) => sum + car.pricePerDay, 0)
  }

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-primary-600"></div>
      </div>
    )
  }

  return (
    <div>
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900">Dashboard</h1>
        <p className="text-gray-600 mt-2">
          Welcome back! Here's what's happening with your car reservation system.
        </p>
      </div>

      {/* Stats Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
        <StatCard
          title="Total Cars"
          value={stats.totalCars}
          icon={Car}
          color="blue"
        />
        <StatCard
          title="Available Cars"
          value={stats.availableCars}
          icon={TrendingUp}
          color="green"
        />
        <StatCard
          title="Reserved Cars"
          value={stats.reservedCars}
          icon={Calendar}
          color="yellow"
        />
        <StatCard
          title="Daily Revenue Potential"
          value={formatCurrency(stats.totalRevenue)}
          icon={Users}
          color="purple"
        />
      </div>

      {/* Recent Activity */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div className="card">
          <div className="card-header">
            <h3 className="text-lg font-semibold">Recent Cars</h3>
          </div>
          <div className="card-content">
            <div className="space-y-4">
              {cars.slice(0, 5).map((car) => (
                <div key={car.id} className="flex items-center space-x-4">
                  <div className="w-12 h-12 bg-primary-100 rounded-lg flex items-center justify-center">
                    <Car className="h-6 w-6 text-primary-600" />
                  </div>
                  <div className="flex-1">
                    <p className="text-sm font-medium text-gray-900">
                      {car.brand} {car.model}
                    </p>
                    <p className="text-sm text-gray-500">{car.licensePlate}</p>
                  </div>
                  <div className="text-sm text-gray-900">
                    {formatCurrency(car.pricePerDay, car.currency)}
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>

        <div className="card">
          <div className="card-header">
            <h3 className="text-lg font-semibold">Car Status Distribution</h3>
          </div>
          <div className="card-content">
            <div className="space-y-4">
              {Object.entries({
                Available: cars.filter(c => c.status === 'Available').length,
                Reserved: cars.filter(c => c.status === 'Reserved').length,
                InMaintenance: cars.filter(c => c.status === 'InMaintenance').length,
                OutOfService: cars.filter(c => c.status === 'OutOfService').length,
              }).map(([status, count]) => (
                <div key={status} className="flex items-center justify-between">
                  <span className="text-sm font-medium text-gray-900">{status}</span>
                  <div className="flex items-center space-x-2">
                    <div className="w-20 bg-gray-200 rounded-full h-2">
                      <div 
                        className="bg-primary-600 h-2 rounded-full"
                        style={{ width: `${(count / stats.totalCars) * 100}%` }}
                      ></div>
                    </div>
                    <span className="text-sm text-gray-500 w-8">{count}</span>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

interface StatCardProps {
  title: string
  value: string | number
  icon: React.ComponentType<{ className?: string }>
  color: 'blue' | 'green' | 'yellow' | 'purple'
}

function StatCard({ title, value, icon: Icon, color }: StatCardProps) {
  const colorClasses = {
    blue: 'bg-blue-500 text-white',
    green: 'bg-green-500 text-white',
    yellow: 'bg-yellow-500 text-white',
    purple: 'bg-purple-500 text-white',
  }

  return (
    <div className="card">
      <div className="card-content">
        <div className="flex items-center">
          <div className={`p-2 rounded-lg ${colorClasses[color]}`}>
            <Icon className="h-6 w-6" />
          </div>
          <div className="ml-4">
            <p className="text-sm font-medium text-gray-500">{title}</p>
            <p className="text-2xl font-bold text-gray-900">{value}</p>
          </div>
        </div>
      </div>
    </div>
  )
}
