import { type ClassValue, clsx } from 'clsx';

export function cn(...inputs: ClassValue[]) {
  return clsx(inputs);
}

export function formatCurrency(amount: number, currency: string = 'USD'): string {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: currency,
  }).format(amount);
}

export function formatDate(date: string | Date): string {
  return new Intl.DateTimeFormat('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  }).format(new Date(date));
}

export function formatDateTime(date: string | Date): string {
  return new Intl.DateTimeFormat('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: 'numeric',
    minute: '2-digit',
  }).format(new Date(date));
}

export function calculateDays(startDate: string, endDate: string): number {
  const start = new Date(startDate);
  const end = new Date(endDate);
  const diffTime = Math.abs(end.getTime() - start.getTime());
  return Math.ceil(diffTime / (1000 * 60 * 60 * 24)) + 1; // +1 to include both start and end day
}

export function getCarTypeDisplayName(carType: string): string {
  const typeMap: Record<string, string> = {
    Sedan: 'Sedan',
    SUV: 'SUV',
    Hatchback: 'Hatchback',
    Coupe: 'Coupe',
    Convertible: 'Convertible',
    Minivan: 'Minivan',
    Truck: 'Truck',
  };
  return typeMap[carType] || carType;
}

export function getStatusColor(status: string): string {
  const statusColors: Record<string, string> = {
    Available: 'text-green-600 bg-green-50',
    Reserved: 'text-yellow-600 bg-yellow-50',
    InMaintenance: 'text-red-600 bg-red-50',
    OutOfService: 'text-gray-600 bg-gray-50',
    Pending: 'text-yellow-600 bg-yellow-50',
    Active: 'text-blue-600 bg-blue-50',
    Completed: 'text-green-600 bg-green-50',
    Cancelled: 'text-red-600 bg-red-50',
  };
  return statusColors[status] || 'text-gray-600 bg-gray-50';
}

export function debounce<T extends (...args: any[]) => any>(
  func: T,
  wait: number
): (...args: Parameters<T>) => void {
  let timeout: NodeJS.Timeout;
  return (...args: Parameters<T>) => {
    clearTimeout(timeout);
    timeout = setTimeout(() => func(...args), wait);
  };
}
