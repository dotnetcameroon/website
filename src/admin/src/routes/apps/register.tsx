import { createFileRoute, Link } from '@tanstack/react-router';
import { useState } from 'react';
import { useRegisterApp } from '../../api/apps';

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
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-heading font-bold">Register Application</h1>
        <Link to="/apps" className="btn btn-outline text-sm">
          Back
        </Link>
      </div>

      {registerApp.data ? (
        <div className="bg-white rounded-xl border border-gray-200 p-5 space-y-4">
          <div className="bg-yellow-50 border border-yellow-200 rounded-lg p-4">
            <p className="text-sm font-medium text-yellow-800">
              Save the client secret now — you won't be able to see it again.
            </p>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Application ID</label>
            <code className="block px-3 py-2 bg-gray-50 rounded-lg text-sm">{registerApp.data.id}</code>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Client Secret</label>
            <div className="flex gap-2">
              <code className="flex-1 px-3 py-2 bg-gray-50 rounded-lg text-sm break-all">
                {registerApp.data.clientSecret}
              </code>
              <button onClick={handleCopy} className="btn btn-outline text-sm !py-2">
                {copied ? 'Copied!' : 'Copy'}
              </button>
            </div>
          </div>

          <Link to="/apps" className="btn btn-primary inline-block text-center">
            Done
          </Link>
        </div>
      ) : (
        <form onSubmit={handleSubmit} className="bg-white rounded-xl border border-gray-200 p-5 space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Application Name</label>
            <input
              required
              value={name}
              onChange={(e) => setName(e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-primary/30 focus:border-primary"
            />
          </div>
          <button
            type="submit"
            disabled={registerApp.isPending}
            className="btn btn-primary"
          >
            {registerApp.isPending ? 'Registering...' : 'Register'}
          </button>
        </form>
      )}
    </div>
  );
}
