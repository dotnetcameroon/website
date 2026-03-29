import { createFileRoute, Link } from '@tanstack/react-router';
import { useState } from 'react';
import { useApps } from '../../api/apps';
import { Pagination } from '../../components/Pagination';
import { Plus, KeyRound, Loader2 } from 'lucide-react';

export const Route = createFileRoute('/apps/')({
  component: AppsListPage,
});

function AppsListPage() {
  const [page, setPage] = useState(1);
  const { data, isLoading } = useApps(page, 10);

  return (
    <div>
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-heading font-bold">External Applications</h1>
        <Link to="/apps/register" className="btn btn-primary inline-flex items-center gap-2">
          <Plus size={16} />
          Register App
        </Link>
      </div>

      {isLoading ? (
        <div className="flex items-center justify-center py-16 text-gray-400">
          <Loader2 size={24} className="animate-spin" />
        </div>
      ) : !data?.items.length ? (
        <div className="flex flex-col items-center justify-center py-16 text-gray-400">
          <KeyRound size={40} strokeWidth={1.2} />
          <p className="mt-3 text-sm">No applications found.</p>
        </div>
      ) : (
        <>
          <div className="bg-white rounded-xl border border-gray-200 overflow-hidden">
            <table className="w-full text-sm">
              <thead>
                <tr className="border-b border-gray-200 bg-gray-50/80">
                  <th className="text-left px-4 py-3 font-medium text-gray-500 text-xs uppercase tracking-wider">Application Name</th>
                  <th className="text-left px-4 py-3 font-medium text-gray-500 text-xs uppercase tracking-wider">Application ID</th>
                </tr>
              </thead>
              <tbody>
                {data.items.map((app) => (
                  <tr key={app.id} className="border-b border-gray-100 last:border-0 hover:bg-gray-50/50 transition-colors">
                    <td className="px-4 py-3 font-medium">{app.clientName}</td>
                    <td className="px-4 py-3 text-gray-500 font-mono text-xs">{app.id}</td>
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
