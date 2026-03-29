import { useState } from 'react';
import { Link, useMatchRoute } from '@tanstack/react-router';
import { Calendar, Handshake, KeyRound, PanelLeftClose, PanelLeftOpen } from 'lucide-react';

const navItems = [
  { to: '/', label: 'Events', icon: Calendar },
  { to: '/partners', label: 'Partners', icon: Handshake },
  { to: '/apps', label: 'Applications', icon: KeyRound },
] as const;

export function AdminSidebar() {
  const [collapsed, setCollapsed] = useState(false);
  const matchRoute = useMatchRoute();

  return (
    <aside
      className={`bg-white border-r border-gray-200 pt-6 transition-all flex flex-col ${
        collapsed ? 'w-16 px-2 pt-6' : 'w-60 px-3 pt-6'
      }`}
    >
      <button
        onClick={() => setCollapsed(!collapsed)}
        className="mb-6 text-gray-400 hover:text-gray-600 self-end p-1.5 rounded-md hover:bg-gray-100 transition-colors cursor-pointer"
        title={collapsed ? 'Expand' : 'Collapse'}
      >
        {collapsed ? <PanelLeftOpen size={18} /> : <PanelLeftClose size={18} />}
      </button>

      <nav className="flex flex-col gap-1">
        {navItems.map(({ to, label, icon: Icon }) => {
          const isActive = matchRoute({ to, fuzzy: to !== '/' }) ||
            (to === '/' && location.pathname === '/admin');
          return (
            <Link
              key={to}
              to={to}
              className={`flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm transition-colors ${
                isActive
                  ? 'bg-primary/10 text-primary font-medium'
                  : 'text-gray-600 hover:bg-gray-50 hover:text-gray-900'
              } ${collapsed ? 'justify-center' : ''}`}
              title={collapsed ? label : undefined}
            >
              <Icon size={18} strokeWidth={isActive ? 2.2 : 1.8} />
              {!collapsed && <span>{label}</span>}
            </Link>
          );
        })}
      </nav>
    </aside>
  );
}
