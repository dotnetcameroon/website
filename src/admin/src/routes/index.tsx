import { createFileRoute, Link } from '@tanstack/react-router';
import { useEvents } from '../api/events';
import { usePartners } from '../api/partners';
import { useApps } from '../api/apps';
import type { EventStatus, EventType } from '../api/types';
import {
  Calendar,
  Handshake,
  KeyRound,
  TrendingUp,
  ArrowRight,
  Loader2,
} from 'lucide-react';
import {
  PieChart,
  Pie,
  Cell,
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from 'recharts';

export const Route = createFileRoute('/')({
  component: DashboardPage,
});

const STATUS_COLORS: Record<EventStatus, string> = {
  Draft: '#94a3b8',
  ComingSoon: '#10b981',
  Passed: '#3b82f6',
  Cancelled: '#ef4444',
};

const TYPE_COLORS: Record<EventType, string> = {
  Conference: '#512bd4',
  Webinar: '#0a855f',
  Meetup: '#e1a325',
  Workshop: '#3b82f6',
};

function DashboardPage() {
  const { data: eventsData, isLoading: loadingEvents } = useEvents(1, 100);
  const { data: partners, isLoading: loadingPartners } = usePartners();
  const { data: appsData, isLoading: loadingApps } = useApps(1, 100);

  const events = eventsData?.items ?? [];
  const upcoming = events.filter((e) => e.status === 'ComingSoon');
  const totalPartners = partners?.length ?? 0;
  const totalApps = appsData?.items.length ?? 0;

  const isLoading = loadingEvents || loadingPartners || loadingApps;

  // Charts data
  const statusData = (['Draft', 'ComingSoon', 'Passed', 'Cancelled'] as EventStatus[])
    .map((status) => ({
      name: status === 'ComingSoon' ? 'Upcoming' : status,
      value: events.filter((e) => e.status === status).length,
      color: STATUS_COLORS[status],
    }))
    .filter((d) => d.value > 0);

  const typeData = (['Conference', 'Webinar', 'Meetup', 'Workshop'] as EventType[])
    .map((type) => ({
      name: type,
      count: events.filter((e) => e.type === type).length,
      fill: TYPE_COLORS[type],
    }))
    .filter((d) => d.count > 0);

  const recentEvents = [...events]
    .sort((a, b) => new Date(b.schedule.start).getTime() - new Date(a.schedule.start).getTime())
    .slice(0, 5);

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64 text-gray-400">
        <Loader2 size={24} className="animate-spin" />
      </div>
    );
  }

  return (
    <div className="max-w-6xl space-y-8">
      <div>
        <h1 className="text-2xl font-heading font-bold text-gray-900">Dashboard</h1>
        <p className="text-sm text-gray-500 mt-1">Overview of your community activity</p>
      </div>

      {/* Stats */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        <StatCard
          label="Total Events"
          value={events.length}
          icon={Calendar}
          color="bg-primary-light text-primary"
        />
        <StatCard
          label="Upcoming"
          value={upcoming.length}
          icon={TrendingUp}
          color="bg-emerald-50 text-emerald-600"
        />
        <StatCard
          label="Partners"
          value={totalPartners}
          icon={Handshake}
          color="bg-violet-50 text-violet-600"
        />
        <StatCard
          label="Applications"
          value={totalApps}
          icon={KeyRound}
          color="bg-amber-50 text-amber-600"
        />
      </div>

      {/* Charts */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Events by status */}
        <div className="card p-6">
          <h2 className="text-sm font-semibold text-gray-800 mb-4">Events by Status</h2>
          {statusData.length === 0 ? (
            <p className="text-sm text-gray-400 py-8 text-center">No events yet</p>
          ) : (
            <div className="flex items-center gap-6">
              <ResponsiveContainer width="50%" height={180}>
                <PieChart>
                  <Pie
                    data={statusData}
                    cx="50%"
                    cy="50%"
                    innerRadius={45}
                    outerRadius={75}
                    dataKey="value"
                    strokeWidth={2}
                    stroke="#fff"
                  >
                    {statusData.map((entry, i) => (
                      <Cell key={i} fill={entry.color} />
                    ))}
                  </Pie>
                  <Tooltip
                    contentStyle={{
                      fontSize: 12,
                      borderRadius: 8,
                      border: '1px solid #e5e7eb',
                      boxShadow: '0 4px 6px -1px rgba(0,0,0,.06)',
                    }}
                  />
                </PieChart>
              </ResponsiveContainer>
              <div className="space-y-2.5">
                {statusData.map((entry) => (
                  <div key={entry.name} className="flex items-center gap-2.5">
                    <div
                      className="size-2.5 rounded-full"
                      style={{ backgroundColor: entry.color }}
                    />
                    <span className="text-xs text-gray-600">{entry.name}</span>
                    <span className="text-xs font-semibold text-gray-800">{entry.value}</span>
                  </div>
                ))}
              </div>
            </div>
          )}
        </div>

        {/* Events by type */}
        <div className="card p-6">
          <h2 className="text-sm font-semibold text-gray-800 mb-4">Events by Type</h2>
          {typeData.length === 0 ? (
            <p className="text-sm text-gray-400 py-8 text-center">No events yet</p>
          ) : (
            <ResponsiveContainer width="100%" height={180}>
              <BarChart data={typeData} barSize={32}>
                <CartesianGrid strokeDasharray="3 3" vertical={false} />
                <XAxis dataKey="name" tick={{ fontSize: 11 }} axisLine={false} tickLine={false} />
                <YAxis allowDecimals={false} tick={{ fontSize: 11 }} axisLine={false} tickLine={false} />
                <Tooltip
                  contentStyle={{
                    fontSize: 12,
                    borderRadius: 8,
                    border: '1px solid #e5e7eb',
                    boxShadow: '0 4px 6px -1px rgba(0,0,0,.06)',
                  }}
                />
                <Bar dataKey="count" radius={[4, 4, 0, 0]}>
                  {typeData.map((entry, i) => (
                    <Cell key={i} fill={entry.fill} />
                  ))}
                </Bar>
              </BarChart>
            </ResponsiveContainer>
          )}
        </div>
      </div>

      {/* Recent events */}
      <div className="card">
        <div className="flex items-center justify-between px-6 py-4 border-b border-gray-100">
          <h2 className="text-sm font-semibold text-gray-800">Recent Events</h2>
          <Link to="/events" className="text-xs text-primary hover:text-primary-dark font-medium flex items-center gap-1 transition-colors">
            View all
            <ArrowRight size={12} />
          </Link>
        </div>
        {recentEvents.length === 0 ? (
          <p className="text-sm text-gray-400 py-10 text-center">No events yet</p>
        ) : (
          <div className="divide-y divide-gray-100">
            {recentEvents.map((event) => (
              <div key={event.id} className="flex items-center justify-between px-6 py-3.5">
                <div className="flex items-center gap-4">
                  <div className="text-center w-11">
                    <p className="text-[10px] text-gray-400 uppercase leading-none">
                      {new Date(event.schedule.start).toLocaleDateString('en', { month: 'short' })}
                    </p>
                    <p className="text-lg font-heading font-bold text-gray-800 leading-tight">
                      {new Date(event.schedule.start).getDate()}
                    </p>
                  </div>
                  <div>
                    <p className="text-sm font-medium text-gray-800">{event.title}</p>
                    <p className="text-xs text-gray-400 mt-0.5">
                      {event.type} &middot; {event.hostingModel}
                    </p>
                  </div>
                </div>
                <StatusBadge status={event.status} />
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}

function StatCard({
  label,
  value,
  icon: Icon,
  color,
}: {
  label: string;
  value: number;
  icon: React.ComponentType<{ size?: number }>;
  color: string;
}) {
  return (
    <div className="card p-5 flex items-start gap-4">
      <div className={`size-10 rounded-lg flex items-center justify-center ${color}`}>
        <Icon size={20} />
      </div>
      <div>
        <p className="text-2xl font-heading font-bold text-gray-900">{value}</p>
        <p className="text-xs text-gray-500 mt-0.5">{label}</p>
      </div>
    </div>
  );
}

const badgeClass: Record<EventStatus, string> = {
  Draft: 'badge-draft',
  ComingSoon: 'badge-coming-soon',
  Passed: 'badge-passed',
  Cancelled: 'badge-cancelled',
};

function StatusBadge({ status }: { status: EventStatus }) {
  return (
    <span className={`badge ${badgeClass[status]}`}>
      {status === 'ComingSoon' ? 'Upcoming' : status}
    </span>
  );
}
