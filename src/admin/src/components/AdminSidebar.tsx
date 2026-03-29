import { useState } from 'react';
import { Link, useMatchRoute } from '@tanstack/react-router';

const navItems = [
  { to: '/', label: 'Events', icon: '📅' },
  { to: '/partners', label: 'Partners', icon: '🤝' },
  { to: '/apps', label: 'Applications', icon: '🔑' },
] as const;

export function AdminSidebar() {
  const [collapsed, setCollapsed] = useState(false);
  const matchRoute = useMatchRoute();

  return (
    <aside
      className={`bg-gray-50 border-r border-gray-200 pt-6 transition-all flex flex-col ${
        collapsed ? 'w-16 p-2 pt-6' : 'w-56 p-4 pt-6'
      }`}
    >
      <button
        onClick={() => setCollapsed(!collapsed)}
        className="mb-6 text-gray-400 hover:text-gray-600 self-end text-xs cursor-pointer"
        title={collapsed ? 'Expand' : 'Collapse'}
      >
        {collapsed ? '▶' : '◀'}
      </button>

      <nav className="flex flex-col gap-1">
        {navItems.map(({ to, label, icon }) => {
          const isActive = matchRoute({ to, fuzzy: to !== '/' }) ||
            (to === '/' && location.pathname === '/admin');
          return (
            <Link
              key={to}
              to={to}
              className={`flex items-center gap-3 px-3 py-2 rounded-lg text-sm transition-colors ${
                isActive
                  ? 'bg-white text-primary font-medium shadow-sm'
                  : 'text-gray-600 hover:bg-white hover:text-gray-900'
              } ${collapsed ? 'justify-center' : ''}`}
            >
              <span>{icon}</span>
              {!collapsed && <span>{label}</span>}
            </Link>
          );
        })}
      </nav>
    </aside>
  );
}
