import { createFileRoute, Link, useNavigate } from '@tanstack/react-router';
import { useState } from 'react';
import { useEvents, useDeleteEvent } from '../api/events';
import { Pagination } from '../components/Pagination';
import type { EventStatus } from '../api/types';
import { Plus, Search, Pencil, Trash2, Calendar, Loader2 } from 'lucide-react';

export const Route = createFileRoute('/')({
  component: EventsListPage,
});

const statusColors: Record<EventStatus, string> = {
  Draft: 'bg-gray-100 text-gray-600',
  ComingSoon: 'bg-emerald-50 text-emerald-700',
  Passed: 'bg-blue-50 text-blue-700',
  Cancelled: 'bg-red-50 text-red-700',
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
    <div>
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-heading font-bold">Events</h1>
        <Link to="/events/new" className="btn btn-primary inline-flex items-center gap-2">
          <Plus size={16} />
          New Event
        </Link>
      </div>

      <form onSubmit={handleSearch} className="mb-4 flex gap-2">
        <div className="relative flex-1">
          <Search size={16} className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
          <input
            type="text"
            placeholder="Search events..."
            value={searchInput}
            onChange={(e) => setSearchInput(e.target.value)}
            className="w-full pl-9 pr-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-primary/30 focus:border-primary"
          />
        </div>
        <button type="submit" className="btn btn-outline text-sm !py-2 inline-flex items-center gap-1.5">
          <Search size={14} />
          Search
        </button>
      </form>

      {isLoading ? (
        <div className="flex items-center justify-center py-16 text-gray-400">
          <Loader2 size={24} className="animate-spin" />
        </div>
      ) : !data?.items.length ? (
        <div className="flex flex-col items-center justify-center py-16 text-gray-400">
          <Calendar size={40} strokeWidth={1.2} />
          <p className="mt-3 text-sm">No events found.</p>
        </div>
      ) : (
        <>
          <div className="bg-white rounded-xl border border-gray-200 overflow-hidden">
            <table className="w-full text-sm">
              <thead>
                <tr className="border-b border-gray-200 bg-gray-50/80">
                  <th className="text-left px-4 py-3 font-medium text-gray-500 text-xs uppercase tracking-wider">Date</th>
                  <th className="text-left px-4 py-3 font-medium text-gray-500 text-xs uppercase tracking-wider">Title</th>
                  <th className="text-left px-4 py-3 font-medium text-gray-500 text-xs uppercase tracking-wider">Status</th>
                  <th className="text-right px-4 py-3 font-medium text-gray-500 text-xs uppercase tracking-wider">Actions</th>
                </tr>
              </thead>
              <tbody>
                {data.items.map((event) => (
                  <tr key={event.id} className="border-b border-gray-100 last:border-0 hover:bg-gray-50/50 transition-colors">
                    <td className="px-4 py-3 text-gray-500">
                      {new Date(event.schedule.start).toLocaleDateString()}
                    </td>
                    <td className="px-4 py-3 font-medium">{event.title}</td>
                    <td className="px-4 py-3">
                      <span className={`inline-block px-2.5 py-0.5 rounded-full text-xs font-medium ${statusColors[event.status]}`}>
                        {event.status}
                      </span>
                    </td>
                    <td className="px-4 py-3 text-right">
                      <div className="flex items-center justify-end gap-1">
                        <Link
                          to="/events/$id/edit"
                          params={{ id: event.id }}
                          className="p-1.5 rounded-md text-gray-400 hover:text-secondary hover:bg-secondary/10 transition-colors"
                          title="Edit"
                        >
                          <Pencil size={15} />
                        </Link>
                        <button
                          onClick={() => {
                            if (confirm('Delete this event?')) {
                              deleteEvent.mutate(event.id, {
                                onSuccess: () => navigate({ to: '/' }),
                              });
                            }
                          }}
                          className="p-1.5 rounded-md text-gray-400 hover:text-red-600 hover:bg-red-50 transition-colors cursor-pointer"
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
