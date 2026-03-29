import { createFileRoute } from '@tanstack/react-router';
import { EventForm } from '../../components/EventForm';

export const Route = createFileRoute('/events/$id/edit')({
  component: EditEventPage,
});

function EditEventPage() {
  const { id } = Route.useParams();
  return <EventForm eventId={id} />;
}
