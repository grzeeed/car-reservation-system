import { Calendar, Clock, User } from 'lucide-react'

export default function ReservationList() {
  return (
    <div>
      <div className="flex justify-between items-center mb-8">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">Reservations</h1>
          <p className="text-gray-600 mt-2">
            Manage all car reservations
          </p>
        </div>
      </div>

      <div className="text-center py-12">
        <Calendar className="mx-auto h-12 w-12 text-gray-400" />
        <h3 className="mt-2 text-sm font-medium text-gray-900">No reservations yet</h3>
        <p className="mt-1 text-sm text-gray-500">
          Reservation management will be implemented when backend API is ready.
        </p>
      </div>
    </div>
  )
}
