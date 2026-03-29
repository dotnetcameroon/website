import { Link, useMatchRoute } from '@tanstack/react-router';
import {
  LayoutDashboard,
  Calendar,
  Handshake,
  KeyRound,
} from 'lucide-react';

const navItems = [
  { to: '/', label: 'Dashboard', icon: LayoutDashboard, exact: true },
  { to: '/events', label: 'Events', icon: Calendar, exact: false },
  { to: '/partners', label: 'Partners', icon: Handshake, exact: false },
  { to: '/apps', label: 'Applications', icon: KeyRound, exact: false },
] as const;

export function AdminSidebar() {
  const matchRoute = useMatchRoute();

  return (
    <aside className="w-[220px] bg-sidebar flex flex-col shrink-0">
      {/* Brand */}
      <div className="h-14 flex items-center px-5 border-b border-white/10">
        <a href="/" className="flex items-center gap-2.5">
          <div className="size-7 rounded-lg bg-primary flex items-center justify-center">
            <span className="text-white text-xs font-bold">.N</span>
          </div>
          <span className="text-white/90 font-heading font-semibold text-sm">.NET Cameroon</span>
        </a>
      </div>

      {/* Navigation */}
      <nav className="flex-1 px-3 py-4 space-y-0.5">
        <p className="section-title text-[10px] text-white/40 px-3 mb-2">Menu</p>
        {navItems.map(({ to, label, icon: Icon, exact }) => {
          const isActive =
            matchRoute({ to, fuzzy: !exact }) ||
            (exact && location.pathname === '/admin') ||
            (exact && location.pathname === '/admin/');
          return (
            <Link
              key={to}
              to={to}
              className={`flex items-center gap-3 px-3 py-2 rounded-lg text-[13px] transition-colors ${
                isActive
                  ? 'bg-sidebar-active text-white font-medium'
                  : 'text-white/50 hover:bg-sidebar-hover hover:text-white/80'
              }`}
            >
              <Icon size={17} strokeWidth={isActive ? 2 : 1.5} />
              <span>{label}</span>
            </Link>
          );
        })}
      </nav>

      {/* Footer */}
      <div className="px-5 py-4 border-t border-white/10">
        <p className="text-[10px] text-white/30">Admin Panel v1.0</p>
      </div>
    </aside>
  );
}
