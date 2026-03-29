import { createFileRoute, Link } from '@tanstack/react-router';
import { useState } from 'react';
import { useRegisterApp } from '../../api/apps';
import { ArrowLeft, Copy, Check, ShieldAlert, Loader2 } from 'lucide-react';

export const Route = createFileRoute('/apps/register')({
  component: RegisterAppPage,
});

function RegisterAppPage() {
  const [name, setName] = useState('');
  const registerApp = useRegisterApp();
  const [copied, setCopied] = useState(false);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    registerApp.mutate({ name });
  };

  const handleCopy = () => {
    if (registerApp.data?.clientSecret) {
      navigator.clipboard.writeText(registerApp.data.clientSecret);
      setCopied(true);
      setTimeout(() => setCopied(false), 2000);
    }
  };

  return (
    <div className="max-w-lg">
      <div className="flex items-center gap-4 mb-8">
        <Link
          to="/apps"
          className="p-2 rounded-lg hover:bg-gray-100 text-gray-400 hover:text-gray-600 transition-colors"
        >
          <ArrowLeft size={20} />
        </Link>
        <div>
          <h1 className="text-2xl font-heading font-bold text-gray-900">Register Application</h1>
          <p className="text-sm text-gray-500 mt-1">Create API credentials for an external app</p>
        </div>
      </div>

      {registerApp.data ? (
        <div className="card p-6 space-y-5">
          <div className="flex items-start gap-3 p-4 bg-amber-50 border border-amber-200/60 rounded-lg">
            <ShieldAlert size={18} className="text-amber-600 mt-0.5 shrink-0" />
            <p className="text-sm text-amber-800">
              Copy the client secret now. It won't be shown again.
            </p>
          </div>

          <div>
            <label className="label">Application ID</label>
            <div className="px-3.5 py-2.5 bg-gray-50 rounded-lg text-sm font-mono text-gray-600">{registerApp.data.id}</div>
          </div>

          <div>
            <label className="label">Client Secret</label>
            <div className="flex gap-2">
              <div className="flex-1 px-3.5 py-2.5 bg-gray-50 rounded-lg text-sm font-mono text-gray-600 break-all">
                {registerApp.data.clientSecret}
              </div>
              <button onClick={handleCopy} className="btn btn-outline btn-sm shrink-0">
                {copied ? <><Check size={14} className="text-green-600" /> Copied</> : <><Copy size={14} /> Copy</>}
              </button>
            </div>
          </div>

          <Link to="/apps" className="btn btn-primary w-full">
            Done
          </Link>
        </div>
      ) : (
        <form onSubmit={handleSubmit} className="card p-6 space-y-5">
          <div>
            <label className="label">Application Name</label>
            <input
              required
              value={name}
              onChange={(e) => setName(e.target.value)}
              className="input"
              placeholder="e.g. Mobile App, Partner API"
            />
          </div>
          <button type="submit" disabled={registerApp.isPending} className="btn btn-primary w-full">
            {registerApp.isPending ? <><Loader2 size={16} className="animate-spin" /> Registering...</> : 'Register Application'}
          </button>
        </form>
      )}
    </div>
  );
}
