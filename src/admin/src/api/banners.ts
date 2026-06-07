import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { get, post, put, del } from './client';
import type { PagedResponse, BannerResponse, CreateOrUpdateBannerRequest } from './types';

export function useBanners(page: number, size: number) {
  const params = new URLSearchParams({ page: String(page), size: String(size) });
  return useQuery({
    queryKey: ['banners', page, size],
    queryFn: () => get<PagedResponse<BannerResponse>>(`/banners?${params}`),
  });
}

export function useBanner(id: string) {
  return useQuery({
    queryKey: ['banners', id],
    queryFn: () => get<BannerResponse>(`/banners/${id}`),
    enabled: !!id,
  });
}

export function useCreateBanner() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateOrUpdateBannerRequest) => post<BannerResponse>('/banners', data),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['banners'] }),
  });
}

export function useUpdateBanner(id: string) {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateOrUpdateBannerRequest) => put<BannerResponse>(`/banners/${id}`, data),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['banners'] }),
  });
}

export function useDeleteBanner() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => del(`/banners/${id}`),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['banners'] }),
  });
}
