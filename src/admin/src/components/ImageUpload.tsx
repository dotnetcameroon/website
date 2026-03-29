import { useState } from 'react';
import { uploadFile } from '../api/files';
import { Upload, X, Loader2, AlertCircle } from 'lucide-react';

interface ImageUploadProps {
  value: string;
  onChange: (url: string) => void;
  folder?: string;
  label?: string;
  height?: string;
}

export function ImageUpload({ value, onChange, folder = 'images', label = 'Click to upload image', height = 'h-40' }: ImageUploadProps) {
  const [uploading, setUploading] = useState(false);
  const [error, setError] = useState('');

  const handleFileChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;

    setUploading(true);
    setError('');

    try {
      const result = await uploadFile(file, folder);
      onChange(result.url);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Upload failed');
    } finally {
      setUploading(false);
      e.target.value = '';
    }
  };

  if (uploading) {
    return (
      <div className={`flex flex-col items-center justify-center ${height} border-2 border-dashed border-primary/30 rounded-lg bg-primary/5`}>
        <Loader2 size={24} className="animate-spin text-primary" />
        <span className="text-sm text-primary mt-2">Uploading...</span>
      </div>
    );
  }

  if (value) {
    return (
      <div className="space-y-2">
        <div className="relative group">
          <img src={value} alt="Uploaded" className={`w-full ${height} object-cover rounded-lg`} />
          <button
            type="button"
            onClick={() => onChange('')}
            className="absolute top-2 right-2 p-1.5 bg-black/50 rounded-lg text-white opacity-0 group-hover:opacity-100 transition-opacity cursor-pointer"
          >
            <X size={14} />
          </button>
        </div>
        {error && (
          <div className="flex items-center gap-1.5 text-xs text-red-500">
            <AlertCircle size={12} />
            {error}
          </div>
        )}
      </div>
    );
  }

  return (
    <div className="space-y-2">
      <label className={`flex flex-col items-center justify-center ${height} border-2 border-dashed border-gray-200 rounded-lg cursor-pointer hover:border-primary/40 hover:bg-primary/5 transition-colors`}>
        <Upload size={24} className="text-gray-300" />
        <span className="text-sm text-gray-400 mt-2">{label}</span>
        <input type="file" accept="image/*" onChange={handleFileChange} className="hidden" />
      </label>
      {error && (
        <div className="flex items-center gap-1.5 text-xs text-red-500">
          <AlertCircle size={12} />
          {error}
        </div>
      )}
    </div>
  );
}
