import { useQuery } from '@tanstack/react-query';
import { get } from './client';
import type { AuthMeResponse } from './types';

export function fetchAuthMe() {
  return get<AuthMeResponse>('/auth/me');
}

export function useAuthMe() {
  return useQuery({
    queryKey: ['auth'],
    queryFn: fetchAuthMe,
    retry: false,
  });
}
