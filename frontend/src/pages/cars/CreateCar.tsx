import { useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { useCreateCar } from '@/hooks/useCars'
import { ArrowLeft, Car } from 'lucide-react'
import { CreateCarDto, CarType } from '@/types/api'

const carSchema = z.object({
  brand: z.string().min(1, 'Brand is required').max(50, 'Brand too long'),
  model: z.string().min(1, 'Model is required').max(50, 'Model too long'),
  licensePlate: z.string().min(1, 'License plate is required').max(15, 'License plate too long'),
  carType: z.nativeEnum(CarType),
  pricePerDay: z.number().min(0.01, 'Price must be greater than 0').max(10000, 'Price too high'),
  currency: z.string().length(3, 'Currency must be 3 characters (e.g., USD)'),
  location: z.object({
    city: z.string().min(1, 'City is required').max(100, 'City name too long'),
    address: z.string().min(1, 'Address is required').max(200, 'Address too long'),
    latitude: z.number().min(-90, 'Invalid latitude').max(90, 'Invalid latitude'),
    longitude: z.number().min(-180, 'Invalid longitude').max(180, 'Invalid longitude'),
  })
})

type CarFormData = z.infer<typeof carSchema>

export default function CreateCar() {
  const navigate = useNavigate()
  const createCarMutation = useCreateCar()
  const [isSubmitting, setIsSubmitting] = useState(false)

  const {
    register,
    handleSubmit,
    formState: { errors },
    setValue,
    watch
  } = useForm<CarFormData>({
    resolver: zodResolver(carSchema),
    defaultValues: {
      currency: 'USD',
      carType: CarType.Sedan,
      pricePerDay: 50,
      location: {
        latitude: 40.7128,
        longitude: -74.0060
      }
    }
  })

  const onSubmit = async (data: CarFormData) => {
    setIsSubmitting(true)
    try {
      const carData: CreateCarDto = {
        brand: data.brand,
        model: data.model,
        licensePlate: data.licensePlate,
        carType: data.carType,
        pricePerDay: data.pricePerDay,
        currency: data.currency,
        location: data.location
      }
      
      await createCarMutation.mutateAsync(carData)
      navigate('/cars')
    } catch (error) {
      console.error('Failed to create car:', error)
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <div>
      {/* Header */}
      <div className="flex items-center space-x-4 mb-8">
        <Link to="/cars" className="btn btn-ghost">
          <ArrowLeft className="h-5 w-5" />
        </Link>
        <div>
          <h1 className="text-3xl font-bold text-gray-900">Add New Car</h1>
          <p className="text-gray-600">Add a new vehicle to your fleet</p>
        </div>
      </div>

      <div className="max-w-2xl mx-auto">
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
          {/* Basic Information */}
          <div className="card">
            <div className="card-header">
              <h3 className="text-lg font-semibold flex items-center">
                <Car className="h-5 w-5 mr-2" />
                Car Information
              </h3>
            </div>
            <div className="card-content">
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Brand *
                  </label>
                  <input
                    {...register('brand')}
                    type="text"
                    className="input"
                    placeholder="e.g., Toyota"
                  />
                  {errors.brand && (
                    <p className="text-sm text-red-600 mt-1">{errors.brand.message}</p>
                  )}
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Model *
                  </label>
                  <input
                    {...register('model')}
                    type="text"
                    className="input"
                    placeholder="e.g., Camry"
                  />
                  {errors.model && (
                    <p className="text-sm text-red-600 mt-1">{errors.model.message}</p>
                  )}
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    License Plate *
                  </label>
                  <input
                    {...register('licensePlate')}
                    type="text"
                    className="input"
                    placeholder="e.g., ABC-1234"
                  />
                  {errors.licensePlate && (
                    <p className="text-sm text-red-600 mt-1">{errors.licensePlate.message}</p>
                  )}
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Car Type *
                  </label>
                  <select {...register('carType')} className="input">
                    {Object.values(CarType).map(type => (
                      <option key={type} value={type}>{type}</option>
                    ))}
                  </select>
                  {errors.carType && (
                    <p className="text-sm text-red-600 mt-1">{errors.carType.message}</p>
                  )}
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Price per Day *
                  </label>
                  <input
                    {...register('pricePerDay', { valueAsNumber: true })}
                    type="number"
                    step="0.01"
                    className="input"
                    placeholder="50.00"
                  />
                  {errors.pricePerDay && (
                    <p className="text-sm text-red-600 mt-1">{errors.pricePerDay.message}</p>
                  )}
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Currency *
                  </label>
                  <select {...register('currency')} className="input">
                    <option value="USD">USD</option>
                    <option value="EUR">EUR</option>
                    <option value="GBP">GBP</option>
                    <option value="PLN">PLN</option>
                  </select>
                  {errors.currency && (
                    <p className="text-sm text-red-600 mt-1">{errors.currency.message}</p>
                  )}
                </div>
              </div>
            </div>
          </div>

          {/* Location Information */}
          <div className="card">
            <div className="card-header">
              <h3 className="text-lg font-semibold">Location Information</h3>
            </div>
            <div className="card-content">
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div className="md:col-span-2">
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    City *
                  </label>
                  <input
                    {...register('location.city')}
                    type="text"
                    className="input"
                    placeholder="e.g., New York"
                  />
                  {errors.location?.city && (
                    <p className="text-sm text-red-600 mt-1">{errors.location.city.message}</p>
                  )}
                </div>

                <div className="md:col-span-2">
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Address *
                  </label>
                  <input
                    {...register('location.address')}
                    type="text"
                    className="input"
                    placeholder="e.g., 123 Main Street"
                  />
                  {errors.location?.address && (
                    <p className="text-sm text-red-600 mt-1">{errors.location.address.message}</p>
                  )}
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Latitude *
                  </label>
                  <input
                    {...register('location.latitude', { valueAsNumber: true })}
                    type="number"
                    step="0.000001"
                    className="input"
                    placeholder="40.7128"
                  />
                  {errors.location?.latitude && (
                    <p className="text-sm text-red-600 mt-1">{errors.location.latitude.message}</p>
                  )}
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Longitude *
                  </label>
                  <input
                    {...register('location.longitude', { valueAsNumber: true })}
                    type="number"
                    step="0.000001"
                    className="input"
                    placeholder="-74.0060"
                  />
                  {errors.location?.longitude && (
                    <p className="text-sm text-red-600 mt-1">{errors.location.longitude.message}</p>
                  )}
                </div>
              </div>
            </div>
          </div>

          {/* Form Actions */}
          <div className="flex justify-end space-x-4">
            <Link to="/cars" className="btn btn-outline">
              Cancel
            </Link>
            <button
              type="submit"
              disabled={isSubmitting}
              className="btn btn-primary"
            >
              {isSubmitting ? 'Creating...' : 'Create Car'}
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}
