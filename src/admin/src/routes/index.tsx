import { createFileRoute, Link, useNavigate } from '@tanstack/react-router';
import { useState } from 'react';
import { useEvents, useDeleteEvent } from '../api/events';
import { Pagination } from '../components/Pagination';
import type { EventStatus } from '../api/types';

export const Route = createFileRoute('/')({
  component: EventsListPage,
});

const statusColors: Record<EventStatus, string> = {
  Draft: 'bg-gray-100 text-gray-700',
  ComingSoon: 'bg-green-100 text-green-700',
  Passed: 'bg-blue-100 text-blue-700',
  Cancelled: 'bg-red-100 text-red-700',
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
        <Link to="/events/new" className="btn btn-primary">
          New Event
        </Link>
      </div>

      <form onSubmit={handleSearch} className="mb-4 flex gap-2">
        <input
          type="text"
          placeholder="Search events..."
          value={searchInput}
          onChange={(e) => setSearchInput(e.target.value)}
          className="flex-1 px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-primary/30 focus:border-primary"
        />
        <button type="submit" className="btn btn-outline text-sm !py-2">
          Search
        </button>
      </form>

      {isLoading ? (
        <div className="text-center py-12 text-gray-500">Loading...</div>
      ) : !data?.items.length ? (
        <div className="text-center py-12 text-gray-500">No events found.</div>
      ) : (
        <>
          <div className="bg-white rounded-xl border border-gray-200 overflow-hidden">
            <table className="w-full text-sm">
              <thead>
                <tr className="border-b border-gray-200 bg-gray-50">
                  <th className="text-left px-4 py-3 font-medium text-gray-600">Date</th>
                  <th className="text-left px-4 py-3 font-medium text-gray-600">Title</th>
                  <th className="text-left px-4 py-3 font-medium text-gray-600">Status</th>
                  <th className="text-right px-4 py-3 font-medium text-gray-600">Actions</th>
                </tr>
              </thead>
              <tbody>
                {data.items.map((event) => (
                  <tr key={event.id} className="border-b border-gray-100 last:border-0 hover:bg-gray-50/50">
                    <td className="px-4 py-3 text-gray-600">
                      {new Date(event.schedule.start).toLocaleDateString()}
                    </td>
                    <td className="px-4 py-3 font-medium">{event.title}</td>
                    <td className="px-4 py-3">
                      <span className={`inline-block px-2 py-0.5 rounded-full text-xs font-medium ${statusColors[event.status]}`}>
                        {event.status}
                      </span>
                    </td>
                    <td className="px-4 py-3 text-right">
                      <div className="flex items-center justify-end gap-2">
                        <Link
                          to="/events/$id/edit"
                          params={{ id: event.id }}
                          className="text-secondary hover:text-secondary-accentuation text-xs"
                        >
                          Edit
                        </Link>
                        <button
                          onClick={() => {
                            if (confirm('Delete this event?')) {
                              deleteEvent.mutate(event.id, {
                                onSuccess: () => navigate({ to: '/' }),
                              });
                            }
                          }}
                          className="text-red-600 hover:text-red-800 text-xs cursor-pointer"
                        >
                          Delete
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
