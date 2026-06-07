import { createFileRoute } from '@tanstack/react-router';
import { BannerForm } from '../../components/BannerForm';

export const Route = createFileRoute('/banners/$id/edit')({
  component: EditBannerPage,
});

function EditBannerPage() {
  const { id } = Route.useParams();
  return <BannerForm bannerId={id} />;
}
