import { useState, useEffect } from 'react';
import { useNavigate } from '@tanstack/react-router';
import { useEvent, useCreateEvent, useUpdateEvent, usePublishEvent, useCancelEvent } from '../api/events';
import { usePartners } from '../api/partners';
import { NewActivityModal } from './NewActivityModal';
import type {
  CreateOrUpdateEventRequest,
  ActivityRequest,
  EventType,
  EventHostingModel,
  EventStatus,
  PartnerResponse,
} from '../api/types';
import {
  ArrowLeft,
  Save,
  Send,
  XCircle,
  RotateCcw,
  Plus,
  Trash2,
  Upload,
  Loader2,
  X,
} from 'lucide-react';

interface EventFormProps {
  eventId?: string;
}

const eventTypes: EventType[] = ['Conference', 'Webinar', 'Meetup', 'Workshop'];
const hostingModels: EventHostingModel[] = ['Online', 'InPerson', 'Hybrid'];

export function EventForm({ eventId }: EventFormProps) {
  const navigate = useNavigate();
  const isEdit = !!eventId;

  const { data: existingEvent, isLoading: loadingEvent } = useEvent(eventId ?? '');
  const { data: allPartners } = usePartners();
  const createEvent = useCreateEvent();
  const updateEvent = useUpdateEvent(eventId ?? '');
  const publishEvent = usePublishEvent();
  const cancelEvent = useCancelEvent();

  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [isAllDay, setIsAllDay] = useState(false);
  const [eventType, setEventType] = useState<EventType>('Meetup');
  const [hostingModel, setHostingModel] = useState<EventHostingModel>('InPerson');
  const [registrationLink, setRegistrationLink] = useState('');
  const [imageUrl, setImageUrl] = useState('');
  const [location, setLocation] = useState('');
  const [selectedPartnerIds, setSelectedPartnerIds] = useState<string[]>([]);
  const [activities, setActivities] = useState<ActivityRequest[]>([]);
  const [showActivityModal, setShowActivityModal] = useState(false);
  const [status, setStatus] = useState<EventStatus>('Draft');

  useEffect(() => {
    if (!existingEvent) return;
    setTitle(existingEvent.title);
    setDescription(existingEvent.description);
    setStartDate(existingEvent.schedule.start.slice(0, 16));
    setEndDate(existingEvent.schedule.end?.slice(0, 16) ?? '');
    setIsAllDay(existingEvent.schedule.isAllDay);
    setEventType(existingEvent.type);
    setHostingModel(existingEvent.hostingModel);
    setRegistrationLink(existingEvent.registrationLink);
    setImageUrl(existingEvent.imageUrl);
    setLocation(existingEvent.location ?? '');
    setStatus(existingEvent.status);
    setSelectedPartnerIds(existingEvent.partners.map((p) => p.id));
    setActivities(
      existingEvent.activities.map((a) => ({
        title: a.title,
        description: a.description,
        host: a.host,
        schedule: a.schedule,
      })),
    );
  }, [existingEvent]);

  const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;
    const reader = new FileReader();
    reader.onload = () => setImageUrl(reader.result as string);
    reader.readAsDataURL(file);
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const data: CreateOrUpdateEventRequest = {
      title,
      description,
      schedule: {
        start: new Date(startDate).toISOString(),
        end: isAllDay ? null : (endDate ? new Date(endDate).toISOString() : null),
        isAllDay,
      },
      type: eventType,
      status,
      hostingModel,
      attendance: existingEvent?.attendance ?? 0,
      registrationLink,
      imageUrl,
      images: existingEvent?.images ?? '',
      location: location || null,
      activities,
      partnerIds: selectedPartnerIds,
    };
    const mutation = isEdit ? updateEvent : createEvent;
    mutation.mutate(data, {
      onSuccess: () => navigate({ to: '/events' }),
    });
  };

  const handlePartnerSelect = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const id = e.target.value;
    if (id && !selectedPartnerIds.includes(id)) {
      setSelectedPartnerIds([...selectedPartnerIds, id]);
    }
    e.target.value = '';
  };

  const selectedPartners = allPartners?.filter((p: PartnerResponse) => selectedPartnerIds.includes(p.id)) ?? [];
  const isSaving = createEvent.isPending || updateEvent.isPending;

  if (isEdit && loadingEvent) {
    return (
      <div className="flex items-center justify-center h-48 text-gray-400">
        <Loader2 size={24} className="animate-spin" />
      </div>
    );
  }

  return (
    <div className="max-w-4xl">
      {/* Header */}
      <div className="flex items-center justify-between mb-8">
        <div className="flex items-center gap-4">
          <button
            onClick={() => navigate({ to: '/events' })}
            className="p-2 rounded-lg hover:bg-gray-100 text-gray-400 hover:text-gray-600 transition-colors cursor-pointer"
          >
            <ArrowLeft size={20} />
          </button>
          <div>
            <h1 className="text-2xl font-heading font-bold text-gray-900">
              {isEdit ? 'Edit Event' : 'New Event'}
            </h1>
            {isEdit && (
              <span className={`badge mt-1 ${
                status === 'Draft' ? 'badge-draft' :
                status === 'ComingSoon' ? 'badge-coming-soon' :
                status === 'Passed' ? 'badge-passed' : 'badge-cancelled'
              }`}>
                {status === 'ComingSoon' ? 'Upcoming' : status}
              </span>
            )}
          </div>
        </div>
        <div className="flex gap-2">
          {isEdit && status === 'Draft' && (
            <button
              onClick={() => publishEvent.mutate(eventId!, { onSuccess: () => navigate({ to: '/events' }) })}
              className="btn btn-primary btn-sm"
            >
              <Send size={14} />
              Publish
            </button>
          )}
          {isEdit && (status === 'ComingSoon' || status === 'Passed') && (
            <button
              onClick={() => cancelEvent.mutate(eventId!, { onSuccess: () => navigate({ to: '/events' }) })}
              className="btn btn-danger btn-sm"
            >
              <XCircle size={14} />
              Cancel Event
            </button>
          )}
          {isEdit && status === 'Cancelled' && (
            <button
              onClick={() => publishEvent.mutate(eventId!, { onSuccess: () => navigate({ to: '/events' }) })}
              className="btn btn-primary btn-sm"
            >
              <RotateCcw size={14} />
              Re-Publish
            </button>
          )}
        </div>
      </div>

      <form onSubmit={handleSubmit} className="space-y-6">
        {/* Basic Info */}
        <section className="card p-6 space-y-5">
          <h2 className="text-sm font-semibold text-gray-800">Event Details</h2>

          <div>
            <label className="label">Title</label>
            <input
              required
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              placeholder="e.g. .NET Conf Cameroon 2026"
              className="input"
            />
          </div>

          <div>
            <label className="label">Description</label>
            <textarea
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              rows={4}
              placeholder="Describe the event..."
              className="input resize-y"
            />
          </div>

          <div>
            <label className="label">Registration Link</label>
            <input
              type="url"
              value={registrationLink}
              onChange={(e) => setRegistrationLink(e.target.value)}
              placeholder="https://..."
              className="input"
            />
          </div>

          <div>
            <label className="label">Location</label>
            <input
              value={location}
              onChange={(e) => setLocation(e.target.value)}
              placeholder="Optional — e.g. Douala, Cameroon"
              className="input"
            />
          </div>
        </section>

        {/* Schedule & Classification */}
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
          <section className="card p-6 space-y-5">
            <h2 className="text-sm font-semibold text-gray-800">Schedule</h2>

            <div>
              <label className="label">Start Date & Time</label>
              <input
                type="datetime-local"
                required
                value={startDate}
                onChange={(e) => setStartDate(e.target.value)}
                className="input"
              />
            </div>

            {!isAllDay && (
              <div>
                <label className="label">End Date & Time</label>
                <input
                  type="datetime-local"
                  value={endDate}
                  onChange={(e) => setEndDate(e.target.value)}
                  className="input"
                />
              </div>
            )}

            <label className="flex items-center gap-2.5 text-sm text-gray-600 cursor-pointer select-none">
              <input
                type="checkbox"
                checked={isAllDay}
                onChange={(e) => setIsAllDay(e.target.checked)}
                className="size-4 rounded border-gray-300 text-primary focus:ring-primary/30"
              />
              All-day event
            </label>
          </section>

          <section className="card p-6 space-y-5">
            <h2 className="text-sm font-semibold text-gray-800">Classification</h2>

            <div>
              <label className="label">Event Type</label>
              <select
                value={eventType}
                onChange={(e) => setEventType(e.target.value as EventType)}
                className="input"
              >
                {eventTypes.map((t) => (
                  <option key={t} value={t}>{t}</option>
                ))}
              </select>
            </div>

            <div>
              <label className="label">Hosting Model</label>
              <select
                value={hostingModel}
                onChange={(e) => setHostingModel(e.target.value as EventHostingModel)}
                className="input"
              >
                {hostingModels.map((m) => (
                  <option key={m} value={m}>{m === 'InPerson' ? 'In Person' : m}</option>
                ))}
              </select>
            </div>
          </section>
        </div>

        {/* Cover Image */}
        <section className="card p-6 space-y-4">
          <h2 className="text-sm font-semibold text-gray-800">Cover Image</h2>
          {imageUrl ? (
            <div className="relative group">
              <img src={imageUrl} alt="Cover" className="w-full h-52 object-cover rounded-lg" />
              <button
                type="button"
                onClick={() => setImageUrl('')}
                className="absolute top-2 right-2 p-1.5 bg-black/50 rounded-lg text-white opacity-0 group-hover:opacity-100 transition-opacity cursor-pointer"
              >
                <X size={14} />
              </button>
            </div>
          ) : (
            <label className="flex flex-col items-center justify-center h-40 border-2 border-dashed border-gray-200 rounded-lg cursor-pointer hover:border-primary/40 hover:bg-primary/5 transition-colors">
              <Upload size={24} className="text-gray-300" />
              <span className="text-sm text-gray-400 mt-2">Click to upload cover image</span>
              <input type="file" accept="image/*" onChange={handleImageChange} className="hidden" />
            </label>
          )}
        </section>

        {/* Partners */}
        <section className="card p-6 space-y-4">
          <h2 className="text-sm font-semibold text-gray-800">Partners</h2>
          <select onChange={handlePartnerSelect} className="input">
            <option value="">Select a partner to add...</option>
            {allPartners
              ?.filter((p: PartnerResponse) => !selectedPartnerIds.includes(p.id))
              .map((p: PartnerResponse) => (
                <option key={p.id} value={p.id}>{p.name}</option>
              ))}
          </select>
          {selectedPartners.length > 0 && (
            <div className="flex flex-wrap gap-2">
              {selectedPartners.map((p: PartnerResponse) => (
                <span
                  key={p.id}
                  className="inline-flex items-center gap-1.5 pl-3 pr-1.5 py-1.5 bg-gray-100 rounded-lg text-xs font-medium text-gray-700"
                >
                  {p.name}
                  <button
                    type="button"
                    onClick={() => setSelectedPartnerIds(selectedPartnerIds.filter((id) => id !== p.id))}
                    className="p-0.5 rounded hover:bg-gray-200 text-gray-400 hover:text-red-500 transition-colors cursor-pointer"
                  >
                    <X size={13} />
                  </button>
                </span>
              ))}
            </div>
          )}
        </section>

        {/* Activities */}
        <section className="card p-6 space-y-4">
          <div className="flex items-center justify-between">
            <h2 className="text-sm font-semibold text-gray-800">Activities</h2>
            <button
              type="button"
              onClick={() => setShowActivityModal(true)}
              className="btn btn-outline btn-sm"
            >
              <Plus size={14} />
              Add
            </button>
          </div>
          {activities.length === 0 ? (
            <p className="text-sm text-gray-400 py-4 text-center">No activities added yet</p>
          ) : (
            <div className="space-y-2">
              {activities.map((a, i) => (
                <div key={i} className="flex items-center justify-between p-3.5 bg-gray-50 rounded-lg">
                  <div>
                    <p className="text-sm font-medium text-gray-800">{a.title}</p>
                    <p className="text-xs text-gray-400 mt-0.5">
                      {a.schedule.start} – {a.schedule.end} &middot; {a.host.name}
                    </p>
                  </div>
                  <button
                    type="button"
                    onClick={() => setActivities(activities.filter((_, j) => j !== i))}
                    className="p-2 rounded-lg text-gray-400 hover:text-red-500 hover:bg-red-50 transition-colors cursor-pointer"
                  >
                    <Trash2 size={14} />
                  </button>
                </div>
              ))}
            </div>
          )}
        </section>

        {/* Submit */}
        <div className="flex items-center justify-end gap-3 pt-2">
          <button
            type="button"
            onClick={() => navigate({ to: '/events' })}
            className="btn btn-outline"
          >
            Cancel
          </button>
          <button type="submit" disabled={isSaving} className="btn btn-primary">
            {isSaving ? (
              <>
                <Loader2 size={16} className="animate-spin" />
                Saving...
              </>
            ) : (
              <>
                <Save size={16} />
                {isEdit ? 'Update Event' : 'Create Event'}
              </>
            )}
          </button>
        </div>
      </form>

      <NewActivityModal
        open={showActivityModal}
        onClose={() => setShowActivityModal(false)}
        onAdd={(activity) => setActivities([...activities, activity])}
      />
    </div>
  );
}
