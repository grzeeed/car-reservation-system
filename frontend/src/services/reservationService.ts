import { api } from './api';
import { ReservationDto, CreateReservationDto, ReservationSummaryDto } from '@/types/api';

export const reservationService = {
  // Create reservation
  async createReservation(reservation: CreateReservationDto): Promise<ReservationDto> {
    const response = await api.post<ReservationDto>('/api/reservations', reservation);
    return response.data;
  },

  // Get reservations by customer
  async getReservationsByCustomer(customerId: string): Promise<ReservationSummaryDto[]> {
    const response = await api.get<ReservationSummaryDto[]>(`/api/reservations/customer/${customerId}`);
    return response.data;
  },

  // Get reservation by ID
  async getReservationById(id: string): Promise<ReservationDto> {
    const response = await api.get<ReservationDto>(`/api/reservations/${id}`);
    return response.data;
  },

  // Confirm reservation
  async confirmReservation(id: string): Promise<ReservationDto> {
    const response = await api.post<ReservationDto>(`/api/reservations/${id}/confirm`);
    return response.data;
  },

  // Cancel reservation
  async cancelReservation(id: string, reason: string): Promise<ReservationDto> {
    const response = await api.post<ReservationDto>(`/api/reservations/${id}/cancel`, { reason });
    return response.data;
  }
};
