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
  ImageIcon,
  Users,
  ListTodo,
  Loader2,
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

  // Form state
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

  // Populate form when editing
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
      onSuccess: () => navigate({ to: '/' }),
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

  if (isEdit && loadingEvent) {
    return (
      <div className="flex items-center justify-center py-16 text-gray-400">
        <Loader2 size={24} className="animate-spin" />
      </div>
    );
  }

  return (
    <div>
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-heading font-bold">
          {isEdit ? 'Edit Event' : 'New Event'}
        </h1>
        <div className="flex gap-2">
          <button onClick={() => navigate({ to: '/' })} className="btn btn-outline text-sm inline-flex items-center gap-1.5">
            <ArrowLeft size={14} />
            Discard
          </button>
          {isEdit && status === 'Draft' && (
            <button
              onClick={() => publishEvent.mutate(eventId!, { onSuccess: () => navigate({ to: '/' }) })}
              className="btn btn-primary text-sm inline-flex items-center gap-1.5"
            >
              <Send size={14} />
              Publish
            </button>
          )}
          {isEdit && (status === 'ComingSoon' || status === 'Passed') && (
            <button
              onClick={() => cancelEvent.mutate(eventId!, { onSuccess: () => navigate({ to: '/' }) })}
              className="btn btn-danger text-sm inline-flex items-center gap-1.5"
            >
              <XCircle size={14} />
              Cancel Event
            </button>
          )}
          {isEdit && status === 'Cancelled' && (
            <button
              onClick={() => publishEvent.mutate(eventId!, { onSuccess: () => navigate({ to: '/' }) })}
              className="btn btn-primary text-sm inline-flex items-center gap-1.5"
            >
              <RotateCcw size={14} />
              Re-Publish
            </button>
          )}
        </div>
      </div>

      <form onSubmit={handleSubmit} className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Left column */}
        <div className="lg:col-span-2 space-y-4">
          <div className="bg-white rounded-xl border border-gray-200 p-5 space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Title</label>
              <input
                required
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-primary/30 focus:border-primary"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Description</label>
              <textarea
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                rows={5}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-primary/30 resize-y"
              />
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Start</label>
                <input
                  type="datetime-local"
                  required
                  value={startDate}
                  onChange={(e) => setStartDate(e.target.value)}
                  className="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-primary/30"
                />
              </div>
              {!isAllDay && (
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">End</label>
                  <input
                    type="datetime-local"
                    value={endDate}
                    onChange={(e) => setEndDate(e.target.value)}
                    className="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-primary/30"
                  />
                </div>
              )}
            </div>

            <label className="flex items-center gap-2 text-sm">
              <input
                type="checkbox"
                checked={isAllDay}
                onChange={(e) => setIsAllDay(e.target.checked)}
                className="rounded"
              />
              All day event
            </label>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Registration Link</label>
              <input
                type="url"
                value={registrationLink}
                onChange={(e) => setRegistrationLink(e.target.value)}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-primary/30"
              />
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Type</label>
                <select
                  value={eventType}
                  onChange={(e) => setEventType(e.target.value as EventType)}
                  className="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-primary/30"
                >
                  {eventTypes.map((t) => (
                    <option key={t} value={t}>{t}</option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Hosting Model</label>
                <select
                  value={hostingModel}
                  onChange={(e) => setHostingModel(e.target.value as EventHostingModel)}
                  className="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-primary/30"
                >
                  {hostingModels.map((m) => (
                    <option key={m} value={m}>{m}</option>
                  ))}
                </select>
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Location</label>
              <input
                value={location}
                onChange={(e) => setLocation(e.target.value)}
                placeholder="Optional"
                className="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-primary/30"
              />
            </div>
          </div>

          {/* Partners */}
          <div className="bg-white rounded-xl border border-gray-200 p-5">
            <h3 className="text-sm font-medium text-gray-700 mb-3 flex items-center gap-2">
              <Users size={16} className="text-gray-400" />
              Partners
            </h3>
            <select onChange={handlePartnerSelect} className="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm mb-3">
              <option value="">Select a partner...</option>
              {allPartners
                ?.filter((p: PartnerResponse) => !selectedPartnerIds.includes(p.id))
                .map((p: PartnerResponse) => (
                  <option key={p.id} value={p.id}>{p.name}</option>
                ))}
            </select>
            <div className="flex flex-wrap gap-2">
              {selectedPartners.map((p: PartnerResponse) => (
                <span key={p.id} className="inline-flex items-center gap-1.5 px-2.5 py-1 bg-gray-100 rounded-full text-xs">
                  {p.name}
                  <button
                    type="button"
                    onClick={() => setSelectedPartnerIds(selectedPartnerIds.filter((id) => id !== p.id))}
                    className="text-gray-400 hover:text-red-500 cursor-pointer"
                  >
                    <XCircle size={13} />
                  </button>
                </span>
              ))}
            </div>
          </div>

          {/* Activities */}
          <div className="bg-white rounded-xl border border-gray-200 p-5">
            <div className="flex items-center justify-between mb-3">
              <h3 className="text-sm font-medium text-gray-700 flex items-center gap-2">
                <ListTodo size={16} className="text-gray-400" />
                Activities
              </h3>
              <button
                type="button"
                onClick={() => setShowActivityModal(true)}
                className="btn btn-outline text-xs !px-3 !py-1.5 inline-flex items-center gap-1"
              >
                <Plus size={13} />
                New Activity
              </button>
            </div>
            {activities.length === 0 ? (
              <p className="text-sm text-gray-400">No activities yet.</p>
            ) : (
              <div className="space-y-2">
                {activities.map((a, i) => (
                  <div key={i} className="flex items-center justify-between p-3 bg-gray-50 rounded-lg">
                    <div>
                      <p className="text-sm font-medium">{a.title}</p>
                      <p className="text-xs text-gray-500">
                        {a.schedule.start} - {a.schedule.end} | {a.host.name}
                      </p>
                    </div>
                    <button
                      type="button"
                      onClick={() => setActivities(activities.filter((_, j) => j !== i))}
                      className="p-1.5 rounded-md text-gray-400 hover:text-red-600 hover:bg-red-50 transition-colors cursor-pointer"
                      title="Remove activity"
                    >
                      <Trash2 size={14} />
                    </button>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>

        {/* Right column */}
        <div className="space-y-4">
          <div className="bg-white rounded-xl border border-gray-200 p-5">
            <h3 className="text-sm font-medium text-gray-700 mb-3 flex items-center gap-2">
              <ImageIcon size={16} className="text-gray-400" />
              Cover Image
            </h3>
            {imageUrl ? (
              <img src={imageUrl} alt="Cover" className="w-full h-40 object-cover rounded-lg mb-3" />
            ) : (
              <div className="w-full h-40 bg-gray-50 rounded-lg mb-3 flex flex-col items-center justify-center text-gray-300">
                <ImageIcon size={32} strokeWidth={1.2} />
                <span className="text-xs mt-1">No image</span>
              </div>
            )}
            <input type="file" accept="image/*" onChange={handleImageChange} className="text-sm" />
          </div>

          <button
            type="submit"
            disabled={createEvent.isPending || updateEvent.isPending}
            className="btn btn-secondary w-full inline-flex items-center justify-center gap-2"
          >
            {createEvent.isPending || updateEvent.isPending ? (
              <>
                <Loader2 size={16} className="animate-spin" />
                Saving...
              </>
            ) : (
              <>
                <Save size={16} />
                Save
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
