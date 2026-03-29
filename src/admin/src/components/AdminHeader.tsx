import { useAuthMe } from '../api/auth';
import { LogOut, CircleUser } from 'lucide-react';

export function AdminHeader() {
  const { data: user } = useAuthMe();

  return (
    <header className="h-14 border-b border-gray-200/80 bg-white flex items-center justify-between px-6">
      <div />
      <div className="flex items-center gap-5">
        {user && (
          <div className="flex items-center gap-2">
            <CircleUser size={18} className="text-gray-400" />
            <span className="text-sm text-gray-600">{user.email}</span>
          </div>
        )}
        <a
          href="/account/logout"
          className="flex items-center gap-1.5 text-sm text-gray-400 hover:text-red-500 transition-colors"
        >
          <LogOut size={15} />
        </a>
      </div>
    </header>
  );
}
