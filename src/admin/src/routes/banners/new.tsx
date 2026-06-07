import { createFileRoute } from '@tanstack/react-router';
import { BannerForm } from '../../components/BannerForm';

export const Route = createFileRoute('/banners/new')({
  component: () => <BannerForm />,
});
