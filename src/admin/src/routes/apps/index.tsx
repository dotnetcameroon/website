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
    <div className="max-w-4xl">
      <div className="flex items-center justify-between mb-6">
        <div>
          <h1 className="text-2xl font-heading font-bold text-gray-900">Applications</h1>
          <p className="text-sm text-gray-500 mt-1">Manage external API access</p>
        </div>
        <Link to="/apps/register" className="btn btn-primary">
          <Plus size={16} />
          Register App
        </Link>
      </div>

      {isLoading ? (
        <div className="flex items-center justify-center h-48 text-gray-400">
          <Loader2 size={24} className="animate-spin" />
        </div>
      ) : !data?.items.length ? (
        <div className="card flex flex-col items-center justify-center py-20 text-gray-400">
          <KeyRound size={36} strokeWidth={1.2} />
          <p className="mt-3 text-sm">No applications registered</p>
          <Link to="/apps/register" className="btn btn-primary btn-sm mt-4">
            <Plus size={14} />
            Register your first app
          </Link>
        </div>
      ) : (
        <>
          <div className="card overflow-hidden">
            <table className="w-full text-sm">
              <thead>
                <tr className="border-b border-gray-100 bg-gray-50/60">
                  <th className="text-left px-5 py-3 section-title text-[11px]">Application Name</th>
                  <th className="text-left px-5 py-3 section-title text-[11px]">Application ID</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-100">
                {data.items.map((app) => (
                  <tr key={app.id} className="hover:bg-gray-50/50 transition-colors">
                    <td className="px-5 py-3.5 font-medium text-gray-800">{app.clientName}</td>
                    <td className="px-5 py-3.5 text-gray-400 font-mono text-xs">{app.id}</td>
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
