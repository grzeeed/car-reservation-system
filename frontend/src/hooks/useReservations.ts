import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { reservationService } from '@/services/reservationService';
import { CreateReservationDto } from '@/types/api';

// Query keys
export const reservationKeys = {
  all: ['reservations'] as const,
  lists: () => [...reservationKeys.all, 'list'] as const,
  list: (filters: Record<string, any>) => [...reservationKeys.lists(), { filters }] as const,
  details: () => [...reservationKeys.all, 'detail'] as const,
  detail: (id: string) => [...reservationKeys.details(), id] as const,
  customer: (customerId: string) => [...reservationKeys.all, 'customer', customerId] as const,
};

// Get reservations by customer
export const useGetReservationsByCustomer = (customerId: string) => {
  return useQuery({
    queryKey: reservationKeys.customer(customerId),
    queryFn: () => reservationService.getReservationsByCustomer(customerId),
    enabled: !!customerId,
  });
};

// Get reservation by ID
export const useGetReservationById = (id: string) => {
  return useQuery({
    queryKey: reservationKeys.detail(id),
    queryFn: () => reservationService.getReservationById(id),
    enabled: !!id,
  });
};

// Create reservation mutation
export const useCreateReservation = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (reservation: CreateReservationDto) =>
      reservationService.createReservation(reservation),
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: reservationKeys.customer(data.customerId) });
      // Also invalidate cars data as availability may have changed
      queryClient.invalidateQueries({ queryKey: ['cars'] });
    },
  });
};

// Confirm reservation mutation
export const useConfirmReservation = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => reservationService.confirmReservation(id),
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: reservationKeys.detail(data.id) });
      queryClient.invalidateQueries({ queryKey: reservationKeys.customer(data.customerId) });
    },
  });
};

// Cancel reservation mutation
export const useCancelReservation = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, reason }: { id: string; reason: string }) =>
      reservationService.cancelReservation(id, reason),
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: reservationKeys.detail(data.id) });
      queryClient.invalidateQueries({ queryKey: reservationKeys.customer(data.customerId) });
      // Also invalidate cars data as availability may have changed
      queryClient.invalidateQueries({ queryKey: ['cars'] });
    },
  });
};
