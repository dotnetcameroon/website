import { createFileRoute } from '@tanstack/react-router';
import { useState } from 'react';
import { usePartners, useCreatePartner, useDeletePartner } from '../../api/partners';
import type { CreateOrUpdatePartnerRequest } from '../../api/types';

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
        <button onClick={() => setShowForm(!showForm)} className="btn btn-primary">
          {showForm ? 'Cancel' : 'Add Partner'}
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
          <button type="submit" disabled={createPartner.isPending} className="btn btn-secondary text-sm">
            {createPartner.isPending ? 'Saving...' : 'Save Partner'}
          </button>
        </form>
      )}

      {isLoading ? (
        <div className="text-center py-12 text-gray-500">Loading...</div>
      ) : !partners?.length ? (
        <div className="text-center py-12 text-gray-500">No partners yet.</div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
          {partners.map((p) => (
            <div key={p.id} className="bg-white rounded-xl border border-gray-200 p-4 flex flex-col items-center gap-3">
              {p.logo && (
                <img src={p.logo} alt={p.name} className="h-12 object-contain" />
              )}
              <p className="font-medium text-sm">{p.name}</p>
              {p.website && (
                <a href={p.website} target="_blank" rel="noreferrer" className="text-xs text-secondary hover:underline">
                  {p.website}
                </a>
              )}
              <button
                onClick={() => {
                  if (confirm(`Delete partner "${p.name}"?`)) {
                    deletePartner.mutate(p.id);
                  }
                }}
                className="text-xs text-red-500 hover:text-red-700 cursor-pointer"
              >
                Delete
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
