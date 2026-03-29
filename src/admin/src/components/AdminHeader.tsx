import { useAuthMe } from '../api/auth';
import { LogOut, User } from 'lucide-react';

export function AdminHeader() {
  const { data: user } = useAuthMe();

  return (
    <header className="h-14 border-b border-gray-200 bg-white flex items-center justify-between px-6">
      <div className="flex items-center gap-3">
        <a href="/" className="flex items-center gap-2 text-primary font-heading font-bold text-lg">
          .NET Cameroon
        </a>
        <span className="text-xs text-gray-400 bg-gray-100 px-2 py-0.5 rounded-full font-medium">Admin</span>
      </div>
      <div className="flex items-center gap-4">
        {user && (
          <span className="flex items-center gap-1.5 text-sm text-gray-600">
            <User size={14} />
            {user.email}
          </span>
        )}
        <a
          href="/account/logout"
          className="flex items-center gap-1.5 text-sm text-gray-500 hover:text-red-600 transition-colors"
        >
          <LogOut size={14} />
          Logout
        </a>
      </div>
    </header>
  );
}
