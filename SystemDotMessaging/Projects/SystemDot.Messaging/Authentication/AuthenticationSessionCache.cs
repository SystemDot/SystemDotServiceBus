using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.Authentication;

namespace SystemDot.Messaging.Authentication
{
    public class AuthenticationSessionCache
    {
        readonly ConcurrentDictionary<MessageServer, AuthenticationSession> sessions;
        readonly ISystemTime systemTime;

        public AuthenticationSessionCache(ISystemTime systemTime)
        {
            this.systemTime = systemTime;
            sessions = new ConcurrentDictionary<MessageServer, AuthenticationSession>();
        }

        public AuthenticationSession GetCurrentSessionFor(MessageServer forServer)
        {
            Contract.Requires(forServer != null);

            AuthenticationSession temp;

            sessions.TryGetValue(forServer, out temp);

            return temp;
        }

        public void CacheNewSessionFor(MessageServer forServer, ExpiryPlan expiryPlan)
        {
            Contract.Requires(forServer != null);
            Contract.Requires(expiryPlan != null);

            CacheSessionFor(forServer, AuthenticationSession.FromPlan(systemTime.GetCurrentDate(), expiryPlan));
        }

        public void CacheSessionFor(MessageServer forServer, AuthenticationSession session)
        {
            Contract.Requires(forServer != null);
            Contract.Requires(session != null);

            sessions.TryAdd(forServer, session);
        }

        public bool HasCurrentSessionFor(MessageServer forServer)
        {
            Contract.Requires(forServer != null);

            return forServer != MessageServer.None && sessions.ContainsKey(forServer);
        }

        
    }
}