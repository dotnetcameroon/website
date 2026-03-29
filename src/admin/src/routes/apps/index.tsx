import { createFileRoute, Link } from '@tanstack/react-router';
import { useState } from 'react';
import { useApps } from '../../api/apps';
import { Pagination } from '../../components/Pagination';

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
        <Link to="/apps/register" className="btn btn-primary">
          Register App
        </Link>
      </div>

      {isLoading ? (
        <div className="text-center py-12 text-gray-500">Loading...</div>
      ) : !data?.items.length ? (
        <div className="text-center py-12 text-gray-500">No applications found.</div>
      ) : (
        <>
          <div className="bg-white rounded-xl border border-gray-200 overflow-hidden">
            <table className="w-full text-sm">
              <thead>
                <tr className="border-b border-gray-200 bg-gray-50">
                  <th className="text-left px-4 py-3 font-medium text-gray-600">Application Name</th>
                  <th className="text-left px-4 py-3 font-medium text-gray-600">Application ID</th>
                </tr>
              </thead>
              <tbody>
                {data.items.map((app) => (
                  <tr key={app.id} className="border-b border-gray-100 last:border-0 hover:bg-gray-50/50">
                    <td className="px-4 py-3 font-medium">{app.clientName}</td>
                    <td className="px-4 py-3 text-gray-600 font-mono text-xs">{app.id}</td>
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
