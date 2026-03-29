import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { get, post, put, del } from './client';
import type { PartnerResponse, CreateOrUpdatePartnerRequest } from './types';

export function usePartners() {
  return useQuery({
    queryKey: ['partners'],
    queryFn: () => get<PartnerResponse[]>('/partners'),
  });
}

export function useCreatePartner() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateOrUpdatePartnerRequest) => post<PartnerResponse>('/partners', data),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['partners'] }),
  });
}

export function useUpdatePartner(id: string) {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateOrUpdatePartnerRequest) => put<PartnerResponse>(`/partners/${id}`, data),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['partners'] }),
  });
}

export function useDeletePartner() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => del(`/partners/${id}`),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['partners'] }),
  });
}
