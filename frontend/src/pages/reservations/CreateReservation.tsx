import { Calendar } from 'lucide-react'

export default function CreateReservation() {
  return (
    <div>
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900">Create Reservation</h1>
        <p className="text-gray-600 mt-2">
          Make a new car reservation
        </p>
      </div>

      <div className="text-center py-12">
        <Calendar className="mx-auto h-12 w-12 text-gray-400" />
        <h3 className="mt-2 text-sm font-medium text-gray-900">Reservation form coming soon</h3>
        <p className="mt-1 text-sm text-gray-500">
          Reservation creation will be implemented when backend API is ready.
        </p>
      </div>
    </div>
  )
}
