import { useAuthMe } from '../api/auth';

export function AdminHeader() {
  const { data: user } = useAuthMe();

  return (
    <header className="h-16 border-b border-gray-200 bg-white flex items-center justify-between px-6">
      <a href="/" className="flex items-center gap-2 text-primary font-heading font-bold text-lg">
        .NET Cameroon
      </a>
      <div className="flex items-center gap-4">
        {user && (
          <span className="text-sm text-gray-600">{user.email}</span>
        )}
        <a
          href="/account/logout"
          className="text-sm text-red-600 hover:text-red-800 transition-colors"
        >
          Logout
        </a>
      </div>
    </header>
  );
}
