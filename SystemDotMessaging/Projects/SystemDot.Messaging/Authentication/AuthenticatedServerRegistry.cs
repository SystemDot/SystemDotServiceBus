using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Authentication
{
    class AuthenticatedServerRegistry
    {
        readonly ConcurrentDictionary<string, string> servers;

        public AuthenticatedServerRegistry()
        {
            servers = new ConcurrentDictionary<string, string>();
        }

        public bool Contains(MessageServer toCheck)
        {
            Contract.Requires(toCheck != null);
            return !toCheck.IsUnspecified && servers.ContainsKey(toCheck.Name);
        }

        public void Register(string toRegister)
        {
            Contract.Requires(!String.IsNullOrEmpty(toRegister));
            servers.TryAdd(toRegister, toRegister);
        }

        public void Register(MessageServer toRegister)
        {
            Contract.Requires(toRegister != null);
            Register(toRegister.Name);
        }
    }
}