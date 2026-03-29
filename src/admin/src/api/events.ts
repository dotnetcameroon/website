import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { get, post, put, del } from './client';
import type {
  PagedResponse,
  EventSummary,
  EventDetail,
  CreateOrUpdateEventRequest,
} from './types';

export function useEvents(page: number, size: number, search?: string) {
  const params = new URLSearchParams({ page: String(page), size: String(size) });
  if (search) params.set('search', search);

  return useQuery({
    queryKey: ['events', page, size, search],
    queryFn: () => get<PagedResponse<EventSummary>>(`/events?${params}`),
  });
}

export function useEvent(id: string) {
  return useQuery({
    queryKey: ['events', id],
    queryFn: () => get<EventDetail>(`/events/${id}`),
    enabled: !!id,
  });
}

export function useCreateEvent() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateOrUpdateEventRequest) => post<EventDetail>('/events', data),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['events'] }),
  });
}

export function useUpdateEvent(id: string) {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateOrUpdateEventRequest) => put<EventDetail>(`/events/${id}`, data),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['events'] }),
  });
}

export function useDeleteEvent() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => del(`/events/${id}`),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['events'] }),
  });
}

export function usePublishEvent() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => post<EventDetail>(`/events/${id}/publish`),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['events'] }),
  });
}

export function useCancelEvent() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => post<EventDetail>(`/events/${id}/cancel`),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['events'] }),
  });
}
