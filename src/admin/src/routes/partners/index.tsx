import { createFileRoute } from '@tanstack/react-router';
import { useState } from 'react';
import { usePartners, useCreatePartner, useDeletePartner } from '../../api/partners';
import type { CreateOrUpdatePartnerRequest } from '../../api/types';
import { Plus, Trash2, ExternalLink, Handshake, Loader2, X, Upload } from 'lucide-react';

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
    <div className="max-w-4xl">
      <div className="flex items-center justify-between mb-6">
        <div>
          <h1 className="text-2xl font-heading font-bold text-gray-900">Partners</h1>
          <p className="text-sm text-gray-500 mt-1">Manage community partners</p>
        </div>
        <button
          onClick={() => setShowForm(!showForm)}
          className={`btn ${showForm ? 'btn-outline' : 'btn-primary'}`}
        >
          {showForm ? <><X size={16} /> Cancel</> : <><Plus size={16} /> Add Partner</>}
        </button>
      </div>

      {showForm && (
        <form onSubmit={handleSubmit} className="card p-6 mb-6 space-y-4">
          <h2 className="text-sm font-semibold text-gray-800">New Partner</h2>
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div>
              <label className="label">Name</label>
              <input required value={name} onChange={(e) => setName(e.target.value)} className="input" placeholder="Partner name" />
            </div>
            <div>
              <label className="label">Website</label>
              <input type="url" value={website} onChange={(e) => setWebsite(e.target.value)} className="input" placeholder="https://..." />
            </div>
          </div>
          <div>
            <label className="label">Logo</label>
            {logo ? (
              <div className="flex items-center gap-3">
                <img src={logo} alt="Preview" className="h-10 object-contain" />
                <button type="button" onClick={() => setLogo('')} className="text-xs text-gray-400 hover:text-red-500 cursor-pointer">Remove</button>
              </div>
            ) : (
              <label className="flex items-center gap-3 px-4 py-3 border-2 border-dashed border-gray-200 rounded-lg cursor-pointer hover:border-primary/40 hover:bg-primary/5 transition-colors">
                <Upload size={16} className="text-gray-300" />
                <span className="text-sm text-gray-400">Upload logo</span>
                <input type="file" accept="image/*" onChange={handleLogoChange} className="hidden" />
              </label>
            )}
          </div>
          <div className="flex justify-end">
            <button type="submit" disabled={createPartner.isPending} className="btn btn-primary btn-sm">
              {createPartner.isPending ? <><Loader2 size={14} className="animate-spin" /> Saving...</> : 'Save Partner'}
            </button>
          </div>
        </form>
      )}

      {isLoading ? (
        <div className="flex items-center justify-center h-48 text-gray-400">
          <Loader2 size={24} className="animate-spin" />
        </div>
      ) : !partners?.length ? (
        <div className="card flex flex-col items-center justify-center py-20 text-gray-400">
          <Handshake size={36} strokeWidth={1.2} />
          <p className="mt-3 text-sm">No partners yet</p>
        </div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
          {partners.map((p) => (
            <div key={p.id} className="card p-5 flex flex-col items-center gap-3 group">
              {p.logo ? (
                <img src={p.logo} alt={p.name} className="h-12 object-contain" />
              ) : (
                <div className="size-12 rounded-lg bg-gray-100 flex items-center justify-center">
                  <Handshake size={20} className="text-gray-300" />
                </div>
              )}
              <p className="font-medium text-sm text-gray-800">{p.name}</p>
              {p.website && (
                <a href={p.website} target="_blank" rel="noreferrer" className="flex items-center gap-1 text-xs text-gray-400 hover:text-secondary transition-colors">
                  <ExternalLink size={11} />
                  Visit website
                </a>
              )}
              <button
                onClick={() => {
                  if (confirm(`Delete partner "${p.name}"?`)) deletePartner.mutate(p.id);
                }}
                className="flex items-center gap-1 text-xs text-gray-300 hover:text-red-500 transition-colors cursor-pointer opacity-0 group-hover:opacity-100"
              >
                <Trash2 size={11} />
                Delete
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
