import { createFileRoute } from '@tanstack/react-router';
import { EventForm } from '../../components/EventForm';

export const Route = createFileRoute('/events/new')({
  component: () => <EventForm />,
});
