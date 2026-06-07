import { useState, useEffect } from 'react';
import { useNavigate } from '@tanstack/react-router';
import { useBanner, useCreateBanner, useUpdateBanner } from '../api/banners';
import type { CreateOrUpdateBannerRequest, BannerVariant } from '../api/types';
import { ArrowLeft, Save, Loader2 } from 'lucide-react';

interface BannerFormProps {
  bannerId?: string;
}

const variants: BannerVariant[] = ['Announcement', 'Advertisement', 'Promo', 'Event', 'Maintenance'];

function toLocalInput(iso: string): string {
  const d = new Date(iso);
  const pad = (n: number) => String(n).padStart(2, '0');
  return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}T${pad(d.getHours())}:${pad(d.getMinutes())}`;
}

export function BannerForm({ bannerId }: BannerFormProps) {
  const navigate = useNavigate();
  const isEdit = !!bannerId;

  const { data: existing, isLoading: loadingExisting } = useBanner(bannerId ?? '');
  const createBanner = useCreateBanner();
  const updateBanner = useUpdateBanner(bannerId ?? '');

  const [titleEn, setTitleEn] = useState('');
  const [titleFr, setTitleFr] = useState('');
  const [subtitleEn, setSubtitleEn] = useState('');
  const [subtitleFr, setSubtitleFr] = useState('');
  const [messageEn, setMessageEn] = useState('');
  const [messageFr, setMessageFr] = useState('');
  const [variant, setVariant] = useState<BannerVariant>('Announcement');
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [link, setLink] = useState('');
  const [linkLabelEn, setLinkLabelEn] = useState('');
  const [linkLabelFr, setLinkLabelFr] = useState('');
  const [dismissible, setDismissible] = useState(true);
  const [priority, setPriority] = useState(0);
  const [isEnabled, setIsEnabled] = useState(true);

  useEffect(() => {
    if (!existing) return;
    setTitleEn(existing.titleEn ?? '');
    setTitleFr(existing.titleFr ?? '');
    setSubtitleEn(existing.subtitleEn ?? '');
    setSubtitleFr(existing.subtitleFr ?? '');
    setMessageEn(existing.messageEn);
    setMessageFr(existing.messageFr);
    setVariant(existing.variant);
    setStartDate(toLocalInput(existing.startDate));
    setEndDate(toLocalInput(existing.endDate));
    setLink(existing.link ?? '');
    setLinkLabelEn(existing.linkLabelEn ?? '');
    setLinkLabelFr(existing.linkLabelFr ?? '');
    setDismissible(existing.dismissible);
    setPriority(existing.priority);
    setIsEnabled(existing.isEnabled);
  }, [existing]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const data: CreateOrUpdateBannerRequest = {
      titleEn: titleEn.trim() || null,
      titleFr: titleFr.trim() || null,
      subtitleEn: subtitleEn.trim() || null,
      subtitleFr: subtitleFr.trim() || null,
      messageEn,
      messageFr,
      variant,
      startDate: new Date(startDate).toISOString(),
      endDate: new Date(endDate).toISOString(),
      link: link.trim() || null,
      linkLabelEn: linkLabelEn.trim() || null,
      linkLabelFr: linkLabelFr.trim() || null,
      dismissible,
      priority,
      isEnabled,
    };

    const onSuccess = () => navigate({ to: '/banners' });
    if (isEdit) {
      updateBanner.mutate(data, { onSuccess });
    } else {
      createBanner.mutate(data, { onSuccess });
    }
  };

  if (isEdit && loadingExisting) {
    return (
      <div className="flex items-center justify-center h-48 text-gray-400">
        <Loader2 size={24} className="animate-spin" />
      </div>
    );
  }

  const saving = createBanner.isPending || updateBanner.isPending;

  return (
    <div className="max-w-3xl">
      <div className="flex items-center gap-3 mb-6">
        <button
          type="button"
          onClick={() => navigate({ to: '/banners' })}
          className="p-2 rounded-lg text-gray-400 hover:text-gray-700 hover:bg-gray-100 transition-colors cursor-pointer"
        >
          <ArrowLeft size={18} />
        </button>
        <div>
          <h1 className="text-2xl font-heading font-bold text-gray-900">
            {isEdit ? 'Edit Banner' : 'New Banner'}
          </h1>
          <p className="text-sm text-gray-500 mt-1">
            Scheduled site-wide announcements
          </p>
        </div>
      </div>

      <form onSubmit={handleSubmit} className="space-y-6">
        <div className="card p-6 space-y-4">
          <div>
            <h2 className="text-sm font-semibold text-gray-800">Headline (optional)</h2>
            <p className="text-xs text-gray-500 mt-0.5">
              If set, the banner uses the rich layout (title + subtitle + message + CTA).
              Leave empty for a compact single-line banner.
            </p>
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div>
              <label className="label">Title (English)</label>
              <input
                value={titleEn}
                onChange={(e) => setTitleEn(e.target.value)}
                maxLength={120}
                className="input"
                placeholder="AI Skills Fest"
              />
            </div>
            <div>
              <label className="label">Title (Français)</label>
              <input
                value={titleFr}
                onChange={(e) => setTitleFr(e.target.value)}
                maxLength={120}
                className="input"
                placeholder="Festival des compétences IA"
              />
            </div>
            <div>
              <label className="label">Subtitle (English)</label>
              <input
                value={subtitleEn}
                onChange={(e) => setSubtitleEn(e.target.value)}
                maxLength={80}
                className="input"
                placeholder="June 8-12, 2026"
              />
            </div>
            <div>
              <label className="label">Subtitle (Français)</label>
              <input
                value={subtitleFr}
                onChange={(e) => setSubtitleFr(e.target.value)}
                maxLength={80}
                className="input"
                placeholder="8-12 juin 2026"
              />
            </div>
          </div>
        </div>

        <div className="card p-6 space-y-4">
          <h2 className="text-sm font-semibold text-gray-800">Message</h2>
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div>
              <label className="label">Message (English)</label>
              <textarea
                required
                value={messageEn}
                onChange={(e) => setMessageEn(e.target.value)}
                rows={3}
                maxLength={500}
                className="input"
                placeholder="Build your AI skills with chances to earn prizes and certification vouchers"
              />
            </div>
            <div>
              <label className="label">Message (Français)</label>
              <textarea
                required
                value={messageFr}
                onChange={(e) => setMessageFr(e.target.value)}
                rows={3}
                maxLength={500}
                className="input"
                placeholder="Développez vos compétences en IA avec des prix et bons à gagner"
              />
            </div>
          </div>
          <div>
            <label className="label">Variant</label>
            <select
              value={variant}
              onChange={(e) => setVariant(e.target.value as BannerVariant)}
              className="input"
            >
              {variants.map((v) => (
                <option key={v} value={v}>
                  {v}
                </option>
              ))}
            </select>
          </div>
        </div>

        <div className="card p-6 space-y-4">
          <h2 className="text-sm font-semibold text-gray-800">Optional call-to-action</h2>
          <div>
            <label className="label">Link URL</label>
            <input
              type="url"
              value={link}
              onChange={(e) => setLink(e.target.value)}
              className="input"
              placeholder="https://..."
            />
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div>
              <label className="label">Link label (English)</label>
              <input
                value={linkLabelEn}
                onChange={(e) => setLinkLabelEn(e.target.value)}
                maxLength={100}
                className="input"
                placeholder="Learn more"
              />
            </div>
            <div>
              <label className="label">Link label (Français)</label>
              <input
                value={linkLabelFr}
                onChange={(e) => setLinkLabelFr(e.target.value)}
                maxLength={100}
                className="input"
                placeholder="En savoir plus"
              />
            </div>
          </div>
        </div>

        <div className="card p-6 space-y-4">
          <h2 className="text-sm font-semibold text-gray-800">Schedule &amp; behavior</h2>
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div>
              <label className="label">Start date</label>
              <input
                required
                type="datetime-local"
                value={startDate}
                onChange={(e) => setStartDate(e.target.value)}
                className="input"
              />
            </div>
            <div>
              <label className="label">End date</label>
              <input
                required
                type="datetime-local"
                value={endDate}
                onChange={(e) => setEndDate(e.target.value)}
                className="input"
              />
            </div>
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
            <div>
              <label className="label">Priority</label>
              <input
                type="number"
                value={priority}
                onChange={(e) => setPriority(Number(e.target.value))}
                className="input"
              />
              <p className="text-xs text-gray-400 mt-1">Higher shows on top</p>
            </div>
            <label className="flex items-center gap-2 text-sm text-gray-700 mt-7">
              <input
                type="checkbox"
                checked={dismissible}
                onChange={(e) => setDismissible(e.target.checked)}
              />
              Dismissible by visitors
            </label>
            <label className="flex items-center gap-2 text-sm text-gray-700 mt-7">
              <input
                type="checkbox"
                checked={isEnabled}
                onChange={(e) => setIsEnabled(e.target.checked)}
              />
              Enabled
            </label>
          </div>
        </div>

        <div className="flex justify-end gap-2">
          <button
            type="button"
            onClick={() => navigate({ to: '/banners' })}
            className="btn btn-outline"
          >
            Cancel
          </button>
          <button type="submit" disabled={saving} className="btn btn-primary">
            {saving ? (
              <>
                <Loader2 size={14} className="animate-spin" /> Saving...
              </>
            ) : (
              <>
                <Save size={14} /> {isEdit ? 'Save changes' : 'Create banner'}
              </>
            )}
          </button>
        </div>
      </form>
    </div>
  );
}
