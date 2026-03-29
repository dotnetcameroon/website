import { createRootRoute } from '@tanstack/react-router';
import { AdminLayout } from '../components/AdminLayout';
import { fetchAuthMe } from '../api/auth';

export const Route = createRootRoute({
  beforeLoad: async () => {
    try {
      await fetchAuthMe();
    } catch {
      window.location.href = '/account/login?ReturnUrl=/admin';
      throw new Error('Redirecting to login');
    }
  },
  component: AdminLayout,
});
