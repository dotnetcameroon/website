import { useState } from 'react';
import type { ActivityRequest } from '../api/types';
import { X, Plus } from 'lucide-react';

interface NewActivityModalProps {
  open: boolean;
  onClose: () => void;
  onAdd: (activity: ActivityRequest) => void;
}

export function NewActivityModal({ open, onClose, onAdd }: NewActivityModalProps) {
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [startTime, setStartTime] = useState('');
  const [endTime, setEndTime] = useState('');
  const [hostName, setHostName] = useState('');
  const [hostEmail, setHostEmail] = useState('');
  const [hostImage, setHostImage] = useState('/assets/utils/avatar.png');

  if (!open) return null;

  const handleHostImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;
    const reader = new FileReader();
    reader.onload = () => setHostImage(reader.result as string);
    reader.readAsDataURL(file);
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onAdd({
      title,
      description,
      schedule: { start: startTime, end: endTime },
      host: { name: hostName, email: hostEmail, imageUrl: hostImage },
    });
    setTitle('');
    setDescription('');
    setStartTime('');
    setEndTime('');
    setHostName('');
    setHostEmail('');
    setHostImage('/assets/utils/avatar.png');
    onClose();
  };

  return (
    <div className="fixed inset-0 bg-black/40 z-50 flex items-center justify-center p-4">
      <form
        onSubmit={handleSubmit}
        className="bg-white rounded-xl w-full max-w-lg max-h-[90vh] overflow-y-auto shadow-xl"
      >
        {/* Header */}
        <div className="flex items-center justify-between px-6 py-4 border-b border-gray-100">
          <h3 className="text-base font-heading font-semibold text-gray-900">Add Activity</h3>
          <button
            type="button"
            onClick={onClose}
            className="p-1.5 rounded-lg text-gray-400 hover:text-gray-600 hover:bg-gray-100 transition-colors cursor-pointer"
          >
            <X size={18} />
          </button>
        </div>

        {/* Body */}
        <div className="px-6 py-5 space-y-4">
          <div>
            <label className="label">Title</label>
            <input required value={title} onChange={(e) => setTitle(e.target.value)} className="input" placeholder="Activity title" />
          </div>

          <div>
            <label className="label">Description</label>
            <textarea value={description} onChange={(e) => setDescription(e.target.value)} rows={3} className="input resize-y" placeholder="Optional description" />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="label">Start Time</label>
              <input type="time" required value={startTime} onChange={(e) => setStartTime(e.target.value)} className="input" />
            </div>
            <div>
              <label className="label">End Time</label>
              <input type="time" required value={endTime} onChange={(e) => setEndTime(e.target.value)} className="input" />
            </div>
          </div>

          <hr className="border-gray-100" />
          <p className="text-sm font-semibold text-gray-800">Host Information</p>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="label">Name</label>
              <input required value={hostName} onChange={(e) => setHostName(e.target.value)} className="input" placeholder="Host name" />
            </div>
            <div>
              <label className="label">Email</label>
              <input type="email" required value={hostEmail} onChange={(e) => setHostEmail(e.target.value)} className="input" placeholder="host@email.com" />
            </div>
          </div>

          <div>
            <label className="label">Picture</label>
            <input type="file" accept="image/*" onChange={handleHostImageChange} className="text-sm text-gray-500" />
          </div>
        </div>

        {/* Footer */}
        <div className="flex justify-end gap-2 px-6 py-4 border-t border-gray-100">
          <button type="button" onClick={onClose} className="btn btn-outline btn-sm">Cancel</button>
          <button type="submit" className="btn btn-primary btn-sm">
            <Plus size={14} />
            Add Activity
          </button>
        </div>
      </form>
    </div>
  );
}
