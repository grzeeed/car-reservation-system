import { useParams, Link } from 'react-router-dom'
import { useGetCarById } from '@/hooks/useCars'
import { ArrowLeft, Car, MapPin, Calendar } from 'lucide-react'
import { formatCurrency, getStatusColor, getCarTypeDisplayName } from '@/utils/helpers'

export default function CarDetails() {
  const { id } = useParams<{ id: string }>()
  const { data: car, isLoading, error } = useGetCarById(id!)

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-primary-600"></div>
      </div>
    )
  }

  if (error || !car) {
    return (
      <div className="text-center py-12">
        <p className="text-red-600">Car not found or error loading car details.</p>
        <Link to="/cars" className="btn btn-primary mt-4">
          <ArrowLeft className="h-4 w-4 mr-2" />
          Back to Cars
        </Link>
      </div>
    )
  }

  return (
    <div>
      {/* Header */}
      <div className="flex items-center space-x-4 mb-8">
        <Link to="/cars" className="btn btn-ghost">
          <ArrowLeft className="h-5 w-5" />
        </Link>
        <div>
          <h1 className="text-3xl font-bold text-gray-900">
            {car.brand} {car.model}
          </h1>
          <p className="text-gray-600">{car.licensePlate}</p>
        </div>
        <span className={`inline-flex items-center px-3 py-1 rounded-full text-sm font-medium ${getStatusColor(car.status)}`}>
          {car.status}
        </span>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        {/* Car Details */}
        <div className="lg:col-span-2 space-y-6">
          <div className="card">
            <div className="card-header">
              <h3 className="text-lg font-semibold">Car Information</h3>
            </div>
            <div className="card-content">
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="text-sm font-medium text-gray-500">Type</label>
                  <p className="text-sm text-gray-900">{getCarTypeDisplayName(car.carType)}</p>
                </div>
                <div>
                  <label className="text-sm font-medium text-gray-500">License Plate</label>
                  <p className="text-sm text-gray-900">{car.licensePlate}</p>
                </div>
                <div>
                  <label className="text-sm font-medium text-gray-500">Price per Day</label>
                  <p className="text-lg font-semibold text-gray-900">
                    {formatCurrency(car.pricePerDay, car.currency)}
                  </p>
                </div>
                <div>
                  <label className="text-sm font-medium text-gray-500">Status</label>
                  <p className="text-sm text-gray-900">{car.status}</p>
                </div>
              </div>
            </div>
          </div>

          <div className="card">
            <div className="card-header">
              <h3 className="text-lg font-semibold">Location</h3>
            </div>
            <div className="card-content">
              <div className="flex items-start space-x-3">
                <MapPin className="h-5 w-5 text-gray-400 mt-0.5" />
                <div>
                  <p className="text-sm font-medium text-gray-900">{car.currentLocation.address}</p>
                  <p className="text-sm text-gray-500">{car.currentLocation.city}</p>
                  <p className="text-xs text-gray-400">
                    {car.currentLocation.latitude}, {car.currentLocation.longitude}
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* Actions & Reservations */}
        <div className="space-y-6">
          <div className="card">
            <div className="card-header">
              <h3 className="text-lg font-semibold">Actions</h3>
            </div>
            <div className="card-content">
              <div className="space-y-3">
                <Link 
                  to={`/reservations/create?carId=${car.id}`}
                  className={`btn btn-primary w-full ${car.status !== 'Available' ? 'opacity-50 pointer-events-none' : ''}`}
                >
                  <Calendar className="h-4 w-4 mr-2" />
                  Make Reservation
                </Link>
                <button className="btn btn-outline w-full">
                  Edit Car
                </button>
              </div>
            </div>
          </div>

          {car.reservations && car.reservations.length > 0 && (
            <div className="card">
              <div className="card-header">
                <h3 className="text-lg font-semibold">Recent Reservations</h3>
              </div>
              <div className="card-content">
                <div className="space-y-3">
                  {car.reservations.slice(0, 5).map((reservation) => (
                    <div key={reservation.id} className="flex justify-between items-center p-3 bg-gray-50 rounded-lg">
                      <div>
                        <p className="text-sm font-medium text-gray-900">
                          {new Date(reservation.period.startDate).toLocaleDateString()} - {' '}
                          {new Date(reservation.period.endDate).toLocaleDateString()}
                        </p>
                        <p className="text-xs text-gray-500">{reservation.status}</p>
                      </div>
                      <p className="text-sm font-semibold text-gray-900">
                        {formatCurrency(reservation.totalAmount, reservation.currency)}
                      </p>
                    </div>
                  ))}
                </div>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  )
}
