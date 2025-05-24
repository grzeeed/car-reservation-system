import { useState } from 'react'
import { Link } from 'react-router-dom'
import { useGetAllCars, useDeleteCar } from '@/hooks/useCars'
import { Plus, Search, Filter, Car, MapPin, Trash2, Edit, Eye } from 'lucide-react'
import { formatCurrency, getStatusColor, getCarTypeDisplayName } from '@/utils/helpers'
import { CarSummaryDto } from '@/types/api'

export default function CarList() {
  const { data: cars = [], isLoading, error } = useGetAllCars()
  const deleteCarMutation = useDeleteCar()
  const [searchTerm, setSearchTerm] = useState('')
  const [statusFilter, setStatusFilter] = useState<string>('all')

  const filteredCars = cars.filter(car => {
    const matchesSearch = 
      car.brand.toLowerCase().includes(searchTerm.toLowerCase()) ||
      car.model.toLowerCase().includes(searchTerm.toLowerCase()) ||
      car.licensePlate.toLowerCase().includes(searchTerm.toLowerCase())
    
    const matchesStatus = statusFilter === 'all' || car.status === statusFilter
    
    return matchesSearch && matchesStatus
  })

  const handleDeleteCar = async (id: string) => {
    if (window.confirm('Are you sure you want to delete this car?')) {
      try {
        await deleteCarMutation.mutateAsync(id)
      } catch (error) {
        console.error('Failed to delete car:', error)
      }
    }
  }

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-primary-600"></div>
      </div>
    )
  }

  if (error) {
    return (
      <div className="text-center py-12">
        <p className="text-red-600">Error loading cars. Please try again.</p>
      </div>
    )
  }

  return (
    <div>
      {/* Header */}
      <div className="flex justify-between items-center mb-8">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">Cars</h1>
          <p className="text-gray-600 mt-2">
            Manage your fleet of {cars.length} vehicles
          </p>
        </div>
        <Link
          to="/cars/create"
          className="btn btn-primary"
        >
          <Plus className="h-5 w-5 mr-2" />
          Add Car
        </Link>
      </div>

      {/* Filters */}
      <div className="card mb-6">
        <div className="card-content">
          <div className="flex flex-col sm:flex-row gap-4">
            <div className="flex-1 relative">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 h-5 w-5" />
              <input
                type="text"
                placeholder="Search cars..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="input pl-10"
              />
            </div>
            <div className="flex items-center space-x-2">
              <Filter className="h-5 w-5 text-gray-400" />
              <select
                value={statusFilter}
                onChange={(e) => setStatusFilter(e.target.value)}
                className="input"
              >
                <option value="all">All Status</option>
                <option value="Available">Available</option>
                <option value="Reserved">Reserved</option>
                <option value="InMaintenance">In Maintenance</option>
                <option value="OutOfService">Out of Service</option>
              </select>
            </div>
          </div>
        </div>
      </div>

      {/* Cars Grid */}
      {filteredCars.length === 0 ? (
        <div className="text-center py-12">
          <Car className="mx-auto h-12 w-12 text-gray-400" />
          <h3 className="mt-2 text-sm font-medium text-gray-900">No cars found</h3>
          <p className="mt-1 text-sm text-gray-500">
            {cars.length === 0 
              ? "Get started by adding your first car." 
              : "Try adjusting your search or filter criteria."
            }
          </p>
          {cars.length === 0 && (
            <div className="mt-6">
              <Link to="/cars/create" className="btn btn-primary">
                <Plus className="h-5 w-5 mr-2" />
                Add Car
              </Link>
            </div>
          )}
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {filteredCars.map((car) => (
            <CarCard
              key={car.id}
              car={car}
              onDelete={() => handleDeleteCar(car.id)}
              isDeleting={deleteCarMutation.isPending}
            />
          ))}
        </div>
      )}
    </div>
  )
}

interface CarCardProps {
  car: CarSummaryDto
  onDelete: () => void
  isDeleting: boolean
}

function CarCard({ car, onDelete, isDeleting }: CarCardProps) {
  return (
    <div className="card hover:shadow-lg transition-shadow">
      <div className="card-header">
        <div className="flex justify-between items-start">
          <div>
            <h3 className="text-lg font-semibold text-gray-900">
              {car.brand} {car.model}
            </h3>
            <p className="text-sm text-gray-500">{car.licensePlate}</p>
          </div>
          <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${getStatusColor(car.status)}`}>
            {car.status}
          </span>
        </div>
      </div>
      
      <div className="card-content">
        <div className="space-y-3">
          <div className="flex items-center text-sm text-gray-600">
            <Car className="h-4 w-4 mr-2" />
            {getCarTypeDisplayName(car.carType)}
          </div>
          <div className="flex items-center text-sm text-gray-600">
            <MapPin className="h-4 w-4 mr-2" />
            {car.currentLocation.city}
          </div>
          <div className="text-lg font-semibold text-gray-900">
            {formatCurrency(car.pricePerDay, car.currency)}/day
          </div>
        </div>
      </div>
      
      <div className="card-footer">
        <div className="flex space-x-2 w-full">
          <Link
            to={`/cars/${car.id}`}
            className="btn btn-outline btn-sm flex-1"
          >
            <Eye className="h-4 w-4 mr-1" />
            View
          </Link>
          <button
            onClick={onDelete}
            disabled={isDeleting}
            className="btn btn-outline btn-icon-sm text-red-600 hover:bg-red-50 border-red-300"
          >
            <Trash2 className="h-4 w-4" />
          </button>
        </div>
      </div>
    </div>
  )
}
