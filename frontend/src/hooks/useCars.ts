import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { carService } from '@/services/carService';
import { CreateCarDto } from '@/types/api';

// Query keys
export const carKeys = {
  all: ['cars'] as const,
  lists: () => [...carKeys.all, 'list'] as const,
  list: (filters: Record<string, any>) => [...carKeys.lists(), { filters }] as const,
  details: () => [...carKeys.all, 'detail'] as const,
  detail: (id: string) => [...carKeys.details(), id] as const,
  available: (params: Record<string, any>) => [...carKeys.all, 'available', params] as const,
};

// Get all cars
export const useGetAllCars = () => {
  return useQuery({
    queryKey: carKeys.lists(),
    queryFn: carService.getAllCars,
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
};

// Get car by ID
export const useGetCarById = (id: string) => {
  return useQuery({
    queryKey: carKeys.detail(id),
    queryFn: () => carService.getCarById(id),
    enabled: !!id,
  });
};

// Create car mutation
export const useCreateCar = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (car: CreateCarDto) => carService.createCar(car),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: carKeys.lists() });
    },
  });
};

// Update car mutation
export const useUpdateCar = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, car }: { id: string; car: Partial<CreateCarDto> }) =>
      carService.updateCar(id, car),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: carKeys.lists() });
      queryClient.invalidateQueries({ queryKey: carKeys.detail(variables.id) });
    },
  });
};

// Delete car mutation
export const useDeleteCar = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => carService.deleteCar(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: carKeys.lists() });
    },
  });
};

// Search available cars
export const useSearchAvailableCars = (params: {
  startDate?: string;
  endDate?: string;
  location?: string;
  carType?: string;
}) => {
  return useQuery({
    queryKey: carKeys.available(params),
    queryFn: () => carService.searchAvailableCars(params),
    enabled: Object.values(params).some(value => value !== undefined),
  });
};
