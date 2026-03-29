import { createFileRoute } from '@tanstack/react-router';
import { useState } from 'react';
import { usePartners, useCreatePartner, useDeletePartner } from '../../api/partners';
import type { CreateOrUpdatePartnerRequest } from '../../api/types';
import { Plus, Trash2, ExternalLink, Handshake, Loader2, X } from 'lucide-react';

export const Route = createFileRoute('/partners/')({
  component: PartnersPage,
});

function PartnersPage() {
  const { data: partners, isLoading } = usePartners();
  const createPartner = useCreatePartner();
  const deletePartner = useDeletePartner();
  const [showForm, setShowForm] = useState(false);
  const [name, setName] = useState('');
  const [logo, setLogo] = useState('');
  const [website, setWebsite] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const data: CreateOrUpdatePartnerRequest = { name, logo, website };
    createPartner.mutate(data, {
      onSuccess: () => {
        setShowForm(false);
        setName('');
        setLogo('');
        setWebsite('');
      },
    });
  };

  const handleLogoChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;
    const reader = new FileReader();
    reader.onload = () => setLogo(reader.result as string);
    reader.readAsDataURL(file);
  };

  return (
    <div>
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-heading font-bold">Partners</h1>
        <button
          onClick={() => setShowForm(!showForm)}
          className={`btn ${showForm ? 'btn-outline' : 'btn-primary'} inline-flex items-center gap-2`}
        >
          {showForm ? (
            <>
              <X size={16} />
              Cancel
            </>
          ) : (
            <>
              <Plus size={16} />
              Add Partner
            </>
          )}
        </button>
      </div>

      {showForm && (
        <form onSubmit={handleSubmit} className="bg-white rounded-xl border border-gray-200 p-5 mb-6 space-y-3">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Name</label>
            <input
              required
              value={name}
              onChange={(e) => setName(e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-primary/30"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Logo</label>
            <input type="file" accept="image/*" onChange={handleLogoChange} className="text-sm" />
            {logo && <img src={logo} alt="Preview" className="h-10 mt-2" />}
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Website</label>
            <input
              type="url"
              value={website}
              onChange={(e) => setWebsite(e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-primary/30"
            />
          </div>
          <button
            type="submit"
            disabled={createPartner.isPending}
            className="btn btn-secondary text-sm inline-flex items-center gap-2"
          >
            {createPartner.isPending ? (
              <>
                <Loader2 size={14} className="animate-spin" />
                Saving...
              </>
            ) : (
              'Save Partner'
            )}
          </button>
        </form>
      )}

      {isLoading ? (
        <div className="flex items-center justify-center py-16 text-gray-400">
          <Loader2 size={24} className="animate-spin" />
        </div>
      ) : !partners?.length ? (
        <div className="flex flex-col items-center justify-center py-16 text-gray-400">
          <Handshake size={40} strokeWidth={1.2} />
          <p className="mt-3 text-sm">No partners yet.</p>
        </div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
          {partners.map((p) => (
            <div key={p.id} className="bg-white rounded-xl border border-gray-200 p-4 flex flex-col items-center gap-3">
              {p.logo && (
                <img src={p.logo} alt={p.name} className="h-12 object-contain" />
              )}
              <p className="font-medium text-sm">{p.name}</p>
              {p.website && (
                <a href={p.website} target="_blank" rel="noreferrer" className="inline-flex items-center gap-1 text-xs text-secondary hover:underline">
                  <ExternalLink size={12} />
                  {p.website}
                </a>
              )}
              <button
                onClick={() => {
                  if (confirm(`Delete partner "${p.name}"?`)) {
                    deletePartner.mutate(p.id);
                  }
                }}
                className="inline-flex items-center gap-1 text-xs text-gray-400 hover:text-red-600 transition-colors cursor-pointer"
              >
                <Trash2 size={12} />
                Delete
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
