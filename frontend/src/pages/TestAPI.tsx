import { useState } from 'react'
import { api, checkHealth, testApiConnection } from '@/services/api'

export default function TestAPI() {
  const [result, setResult] = useState<any>(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const testConnection = async () => {
    setLoading(true)
    setError(null)
    try {
      const response = await api.get('/api/cars')
      setResult(response.data)
      console.log('API Response:', response.data)
    } catch (err: any) {
      console.error('API Error:', err)
      setError(err.message || 'Unknown error')
    } finally {
      setLoading(false)
    }
  }

  const testHealth = async () => {
    setLoading(true)
    setError(null)
    try {
      const health = await checkHealth()
      setResult(health)
      console.log('Health Response:', health)
    } catch (err: any) {
      console.error('Health Error:', err)
      setError(err.message || 'Unknown error')
    } finally {
      setLoading(false)
    }
  }

  const testApiConnectivity = async () => {
    setLoading(true)
    setError(null)
    try {
      const connectivityResult = await testApiConnection()
      setResult(connectivityResult)
      console.log('API Connectivity:', connectivityResult)
    } catch (err: any) {
      console.error('Connectivity Error:', err)
      setError(err.message || 'Unknown error')
    } finally {
      setLoading(false)
    }
  }

  const testSwagger = async () => {
    setLoading(true)
    setError(null)
    try {
      const response = await api.get('/swagger/v1/swagger.json')
      setResult({
        message: 'Swagger API documentation is available!',
        endpoints: Object.keys(response.data.paths || {})
      })
      console.log('Swagger Response:', response.data)
    } catch (err: any) {
      console.error('Swagger Error:', err)
      setError(err.message || 'Unknown error')
    } finally {
      setLoading(false)
    }
  }

  const createSampleCar = async () => {
    setLoading(true)
    setError(null)
    try {
      const sampleCar = {
        brand: 'Toyota',
        model: 'Camry',
        licensePlate: 'ABC-1234',
        type: 'Sedan',
        pricePerDay: 50.00,
        currency: 'USD',
        city: 'New York',
        address: '123 Main Street',
        latitude: 40.7128,
        longitude: -74.0060
      }
      
      const response = await api.post('/api/cars', sampleCar)
      setResult({
        message: 'Sample car created successfully!',
        car: response.data
      })
      console.log('Created car:', response.data)
    } catch (err: any) {
      console.error('Create car error:', err)
      setError(err.response?.data?.message || err.message || 'Unknown error')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="p-8">
      <h1 className="text-2xl font-bold mb-6">API Connection Test</h1>
      
      <div className="space-y-4">
        <div className="flex flex-wrap gap-4">
          <button
            onClick={testApiConnectivity}
            disabled={loading}
            className="btn btn-primary"
          >
            {loading ? 'Testing...' : 'Test API Connectivity'}
          </button>
          
          <button
            onClick={testHealth}
            disabled={loading}
            className="btn btn-secondary"
          >
            {loading ? 'Testing...' : 'Test Health Endpoint'}
          </button>
          
          <button
            onClick={testConnection}
            disabled={loading}
            className="btn btn-outline"
          >
            {loading ? 'Testing...' : 'Test Cars Endpoint'}
          </button>

          <button
            onClick={testSwagger}
            disabled={loading}
            className="btn btn-outline"
          >
            {loading ? 'Testing...' : 'List All Endpoints'}
          </button>
        </div>

        <div className="border-t pt-4">
          <h3 className="text-lg font-medium text-gray-900 mb-2">Data Operations</h3>
          <button
            onClick={createSampleCar}
            disabled={loading}
            className="btn btn-primary"
          >
            {loading ? 'Creating...' : 'Create Sample Car'}
          </button>
        </div>

        {error && (
          <div className="p-4 bg-red-50 border border-red-200 rounded-md">
            <h3 className="text-red-800 font-medium">Error:</h3>
            <p className="text-red-600">{error}</p>
          </div>
        )}

        {result && (
          <div className="p-4 bg-green-50 border border-green-200 rounded-md">
            <h3 className="text-green-800 font-medium">Success:</h3>
            <pre className="text-green-600 text-sm overflow-auto">
              {JSON.stringify(result, null, 2)}
            </pre>
          </div>
        )}
      </div>

      <div className="mt-8 p-4 bg-gray-50 rounded-md">
        <h3 className="font-medium text-gray-900 mb-2">API Configuration:</h3>
        <p className="text-sm text-gray-600">Base URL: {import.meta.env.VITE_API_URL || 'http://localhost:5170'}</p>
        <p className="text-sm text-gray-600">Environment: {import.meta.env.MODE}</p>
      </div>
    </div>
  )
}
