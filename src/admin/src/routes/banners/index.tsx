import { createFileRoute, Link, useNavigate } from '@tanstack/react-router';
import { useState } from 'react';
import { useBanners, useDeleteBanner } from '../../api/banners';
import { Pagination } from '../../components/Pagination';
import type { BannerVariant } from '../../api/types';
import { Plus, Pencil, Trash2, Megaphone, Loader2, CircleDot, CircleOff } from 'lucide-react';

export const Route = createFileRoute('/banners/')({
  component: BannersListPage,
});

const variantBadge: Record<BannerVariant, string> = {
  Announcement: 'badge bg-secondary/10 text-secondary',
  Advertisement: 'badge bg-tertiary/15 text-tertiary',
  Promo: 'badge bg-primary/10 text-primary',
  Event: 'badge bg-dark-purple/10 text-dark-purple',
  Maintenance: 'badge bg-amber-500/10 text-amber-700',
};

function isActiveNow(start: string, end: string, isEnabled: boolean) {
  if (!isEnabled) return false;
  const now = new Date();
  return new Date(start) <= now && new Date(end) >= now;
}

function BannersListPage() {
  const [page, setPage] = useState(1);
  const { data, isLoading } = useBanners(page, 10);
  const deleteBanner = useDeleteBanner();
  const navigate = useNavigate();

  return (
    <div className="max-w-5xl">
      <div className="flex items-center justify-between mb-6">
        <div>
          <h1 className="text-2xl font-heading font-bold text-gray-900">Banners</h1>
          <p className="text-sm text-gray-500 mt-1">
            Site-wide announcements scheduled by start &amp; end date
          </p>
        </div>
        <Link to="/banners/new" className="btn btn-primary">
          <Plus size={16} />
          New Banner
        </Link>
      </div>

      {isLoading ? (
        <div className="flex items-center justify-center h-48 text-gray-400">
          <Loader2 size={24} className="animate-spin" />
        </div>
      ) : !data?.items.length ? (
        <div className="card flex flex-col items-center justify-center py-20 text-gray-400">
          <Megaphone size={36} strokeWidth={1.2} />
          <p className="mt-3 text-sm">No banners yet</p>
          <Link to="/banners/new" className="btn btn-primary btn-sm mt-4">
            <Plus size={14} />
            Create your first banner
          </Link>
        </div>
      ) : (
        <>
          <div className="card overflow-hidden">
            <table className="w-full text-sm">
              <thead>
                <tr className="border-b border-gray-100 bg-gray-50/60">
                  <th className="text-left px-5 py-3 section-title text-[11px]">Message</th>
                  <th className="text-left px-5 py-3 section-title text-[11px]">Variant</th>
                  <th className="text-left px-5 py-3 section-title text-[11px]">Schedule</th>
                  <th className="text-left px-5 py-3 section-title text-[11px]">Status</th>
                  <th className="text-right px-5 py-3 section-title text-[11px]">Actions</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-100">
                {data.items.map((b) => {
                  const active = isActiveNow(b.startDate, b.endDate, b.isEnabled);
                  return (
                    <tr key={b.id} className="hover:bg-gray-50/50 transition-colors">
                      <td className="px-5 py-3.5 max-w-md">
                        {b.titleEn && (
                          <div className="font-semibold text-gray-900 truncate">{b.titleEn}</div>
                        )}
                        <div className={`${b.titleEn ? 'text-xs text-gray-500' : 'font-medium text-gray-800'} truncate`}>
                          {b.messageEn}
                        </div>
                      </td>
                      <td className="px-5 py-3.5">
                        <span className={variantBadge[b.variant]}>{b.variant}</span>
                      </td>
                      <td className="px-5 py-3.5 text-gray-500 tabular-nums text-xs">
                        {new Date(b.startDate).toLocaleDateString()} →{' '}
                        {new Date(b.endDate).toLocaleDateString()}
                      </td>
                      <td className="px-5 py-3.5">
                        {active ? (
                          <span className="inline-flex items-center gap-1 text-xs text-primary font-medium">
                            <CircleDot size={12} /> Live
                          </span>
                        ) : (
                          <span className="inline-flex items-center gap-1 text-xs text-gray-400">
                            <CircleOff size={12} />
                            {b.isEnabled ? 'Scheduled' : 'Disabled'}
                          </span>
                        )}
                      </td>
                      <td className="px-5 py-3.5 text-right">
                        <div className="flex items-center justify-end gap-0.5">
                          <Link
                            to="/banners/$id/edit"
                            params={{ id: b.id }}
                            className="p-2 rounded-lg text-gray-400 hover:text-secondary hover:bg-secondary/5 transition-colors"
                            title="Edit"
                          >
                            <Pencil size={15} />
                          </Link>
                          <button
                            onClick={() => {
                              if (confirm('Delete this banner?')) {
                                deleteBanner.mutate(b.id, {
                                  onSuccess: () => navigate({ to: '/banners' }),
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
                  );
                })}
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
