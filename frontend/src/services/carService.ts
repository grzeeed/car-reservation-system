import { api } from './api';
import { CarSummaryDto, CarDto, CreateCarDto } from '@/types/api';

export const carService = {
  // Get all cars
  async getAllCars(): Promise<CarSummaryDto[]> {
    const response = await api.get<CarSummaryDto[]>('/api/cars');
    return response.data;
  },

  // Get car by ID
  async getCarById(id: string): Promise<CarDto> {
    const response = await api.get<CarDto>(`/api/cars/${id}`);
    return response.data;
  },

  // Create new car
  async createCar(car: CreateCarDto): Promise<CarDto> {
    const response = await api.post<CarDto>('/api/cars', car);
    return response.data;
  },

  // Update car
  async updateCar(id: string, car: Partial<CreateCarDto>): Promise<CarDto> {
    const response = await api.put<CarDto>(`/api/cars/${id}`, car);
    return response.data;
  },

  // Delete car
  async deleteCar(id: string): Promise<void> {
    await api.delete(`/api/cars/${id}`);
  },

  // Search available cars
  async searchAvailableCars(params: {
    startDate?: string;
    endDate?: string;
    location?: string;
    carType?: string;
  }): Promise<CarSummaryDto[]> {
    const response = await api.get<CarSummaryDto[]>('/api/cars/available', {
      params
    });
    return response.data;
  }
};
