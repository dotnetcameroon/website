import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { get, post } from './client';
import type {
  PagedResponse,
  AppSummary,
  RegisterAppRequest,
  RegisterAppResponse,
} from './types';

export function useApps(page: number, size: number) {
  const params = new URLSearchParams({ page: String(page), size: String(size) });
  return useQuery({
    queryKey: ['apps', page, size],
    queryFn: () => get<PagedResponse<AppSummary>>(`/apps?${params}`),
  });
}

export function useRegisterApp() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: RegisterAppRequest) => post<RegisterAppResponse>('/apps', data),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['apps'] }),
  });
}
