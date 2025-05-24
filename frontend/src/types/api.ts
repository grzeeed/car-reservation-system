// Base types
export interface LocationDto {
  city: string;
  address: string;
  latitude: number;
  longitude: number;
}

export interface DateRangeDto {
  startDate: string; // ISO string
  endDate: string;   // ISO string
}

export interface Money {
  amount: number;
  currency: string;
}

// Car types
export interface CarSummaryDto {
  id: string;
  brand: string;
  model: string;
  licensePlate: string;
  carType: string;
  pricePerDay: number;
  currency: string;
  status: string;
  currentLocation: LocationDto;
}

export interface CarDto extends CarSummaryDto {
  reservations: ReservationDto[];
}

export interface CreateCarDto {
  brand: string;
  model: string;
  licensePlate: string;
  carType: string;
  pricePerDay: number;
  currency: string;
  location: CreateLocationDto;
}

export interface CreateLocationDto {
  city: string;
  address: string;
  latitude: number;
  longitude: number;
}

// Reservation types
export interface ReservationDto {
  id: string;
  carId: string;
  customerId: string;
  period: DateRangeDto;
  totalAmount: number;
  currency: string;
  status: string;
  createdAt: string;
  confirmedAt?: string;
  cancelledAt?: string;
}

export interface ReservationSummaryDto {
  id: string;
  carId: string;
  period: DateRangeDto;
  totalAmount: number;
  currency: string;
  status: string;
  createdAt: string;
}

export interface CreateReservationDto {
  carId: string;
  customerId: string;
  period: DateRangeDto;
}

// Enums
export enum CarType {
  Sedan = 'Sedan',
  SUV = 'SUV',
  Hatchback = 'Hatchback',
  Coupe = 'Coupe',
  Convertible = 'Convertible',
  Minivan = 'Minivan',
  Truck = 'Truck'
}

export enum CarStatus {
  Available = 'Available',
  Reserved = 'Reserved',
  InMaintenance = 'InMaintenance',
  OutOfService = 'OutOfService'
}

export enum ReservationStatus {
  Pending = 'Pending',
  Active = 'Active',
  Completed = 'Completed',
  Cancelled = 'Cancelled'
}

// API Response types
export interface ApiResponse<T> {
  data: T;
  success: boolean;
  error?: string;
}
