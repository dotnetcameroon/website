import { createFileRoute, Link, useNavigate } from '@tanstack/react-router';
import { useState } from 'react';
import { useEvents, useDeleteEvent } from '../../api/events';
import { Pagination } from '../../components/Pagination';
import type { EventStatus } from '../../api/types';
import { Plus, Search, Pencil, Trash2, Calendar, Loader2 } from 'lucide-react';

export const Route = createFileRoute('/events/')({
  component: EventsListPage,
});

const badgeClass: Record<EventStatus, string> = {
  Draft: 'badge-draft',
  ComingSoon: 'badge-coming-soon',
  Passed: 'badge-passed',
  Cancelled: 'badge-cancelled',
};

function EventsListPage() {
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');
  const [searchInput, setSearchInput] = useState('');
  const { data, isLoading } = useEvents(page, 10, search || undefined);
  const deleteEvent = useDeleteEvent();
  const navigate = useNavigate();

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    setSearch(searchInput);
    setPage(1);
  };

  return (
    <div className="max-w-5xl">
      <div className="flex items-center justify-between mb-6">
        <div>
          <h1 className="text-2xl font-heading font-bold text-gray-900">Events</h1>
          <p className="text-sm text-gray-500 mt-1">Manage community events</p>
        </div>
        <Link to="/events/new" className="btn btn-primary">
          <Plus size={16} />
          New Event
        </Link>
      </div>

      <form onSubmit={handleSearch} className="mb-5">
        <div className="relative">
          <Search size={16} className="absolute left-3.5 top-1/2 -translate-y-1/2 text-gray-400" />
          <input
            type="text"
            placeholder="Search events..."
            value={searchInput}
            onChange={(e) => setSearchInput(e.target.value)}
            className="input pl-10 !py-2.5"
          />
        </div>
      </form>

      {isLoading ? (
        <div className="flex items-center justify-center h-48 text-gray-400">
          <Loader2 size={24} className="animate-spin" />
        </div>
      ) : !data?.items.length ? (
        <div className="card flex flex-col items-center justify-center py-20 text-gray-400">
          <Calendar size={36} strokeWidth={1.2} />
          <p className="mt-3 text-sm">No events found</p>
          <Link to="/events/new" className="btn btn-primary btn-sm mt-4">
            <Plus size={14} />
            Create your first event
          </Link>
        </div>
      ) : (
        <>
          <div className="card overflow-hidden">
            <table className="w-full text-sm">
              <thead>
                <tr className="border-b border-gray-100 bg-gray-50/60">
                  <th className="text-left px-5 py-3 section-title text-[11px]">Date</th>
                  <th className="text-left px-5 py-3 section-title text-[11px]">Title</th>
                  <th className="text-left px-5 py-3 section-title text-[11px]">Type</th>
                  <th className="text-left px-5 py-3 section-title text-[11px]">Status</th>
                  <th className="text-right px-5 py-3 section-title text-[11px]">Actions</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-100">
                {data.items.map((event) => (
                  <tr key={event.id} className="hover:bg-gray-50/50 transition-colors">
                    <td className="px-5 py-3.5 text-gray-500 tabular-nums">
                      {new Date(event.schedule.start).toLocaleDateString()}
                    </td>
                    <td className="px-5 py-3.5 font-medium text-gray-800">{event.title}</td>
                    <td className="px-5 py-3.5 text-gray-500">{event.type}</td>
                    <td className="px-5 py-3.5">
                      <span className={`badge ${badgeClass[event.status]}`}>
                        {event.status === 'ComingSoon' ? 'Upcoming' : event.status}
                      </span>
                    </td>
                    <td className="px-5 py-3.5 text-right">
                      <div className="flex items-center justify-end gap-0.5">
                        <Link
                          to="/events/$id/edit"
                          params={{ id: event.id }}
                          className="p-2 rounded-lg text-gray-400 hover:text-secondary hover:bg-secondary/5 transition-colors"
                          title="Edit"
                        >
                          <Pencil size={15} />
                        </Link>
                        <button
                          onClick={() => {
                            if (confirm('Delete this event?')) {
                              deleteEvent.mutate(event.id, {
                                onSuccess: () => navigate({ to: '/events' }),
                              });
                            }
                          }}
                          className="p-2 rounded-lg text-gray-400 hover:text-red-500 hover:bg-red-50 transition-colors cursor-pointer"
                          title="Delete"
                        >
                          <Trash2 size={15} />
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          <Pagination
            page={data.pageNumber}
            totalPages={data.totalPages}
            hasPrevious={data.hasPreviousPage}
            hasNext={data.hasNextPage}
            onPageChange={setPage}
          />
        </>
      )}
    </div>
  );
}
