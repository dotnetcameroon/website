import { useAuthMe } from '../api/auth';
import { useRouterState } from '@tanstack/react-router';
import { LogOut, CircleUser } from 'lucide-react';

const TITLES: Array<{ match: RegExp; title: string }> = [
  { match: /^\/events\/new$/, title: 'New event' },
  { match: /^\/events\/[^/]+\/edit$/, title: 'Edit event' },
  { match: /^\/events/, title: 'Events' },
  { match: /^\/partners/, title: 'Partners' },
  { match: /^\/banners\/new$/, title: 'New banner' },
  { match: /^\/banners\/[^/]+\/edit$/, title: 'Edit banner' },
  { match: /^\/banners/, title: 'Banners' },
  { match: /^\/apps\/register$/, title: 'Register application' },
  { match: /^\/apps/, title: 'Applications' },
  { match: /^\/$/, title: 'Dashboard' },
];

function titleForPath(pathname: string): string {
  return TITLES.find((t) => t.match.test(pathname))?.title ?? '';
}

export function AdminHeader() {
  const { data: user } = useAuthMe();
  const { location } = useRouterState();
  const title = titleForPath(location.pathname);

  return (
    <header className="h-14 border-b border-gray-200/80 bg-white/95 backdrop-blur-sm flex items-center justify-between px-6 sticky top-0 z-10">
      <div className="flex items-center gap-2.5 min-w-0">
        {title && (
          <h1 className="text-sm font-medium text-gray-700 truncate">{title}</h1>
        )}
      </div>
      <div className="flex items-center gap-3">
        {user && (
          <div className="flex items-center gap-2 px-2.5 py-1 rounded-md text-gray-600">
            <CircleUser size={16} className="text-gray-400" />
            <span className="text-sm">{user.email}</span>
          </div>
        )}
        <a
          href="/account/logout"
          className="inline-flex items-center justify-center size-8 rounded-md text-gray-400 hover:text-red-500 hover:bg-red-50 transition-colors"
          aria-label="Sign out"
          title="Sign out"
        >
          <LogOut size={15} />
        </a>
      </div>
    </header>
  );
}
