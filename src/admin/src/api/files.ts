const BASE = '/api/admin';

export interface FileUploadResponse {
  url: string;
  fileName: string;
}

export async function uploadFile(file: File, folder?: string): Promise<FileUploadResponse> {
  const formData = new FormData();
  formData.append('file', file);

  const params = new URLSearchParams();
  if (folder) params.set('folder', folder);

  const res = await fetch(`${BASE}/files/upload?${params}`, {
    method: 'POST',
    credentials: 'include',
    body: formData,
  });

  if (res.status === 401) {
    window.location.href = '/account/login?ReturnUrl=/admin';
    throw new Error('Unauthorized');
  }

  if (!res.ok) {
    const text = await res.text();
    throw new Error(text || res.statusText);
  }

  return res.json();
}
