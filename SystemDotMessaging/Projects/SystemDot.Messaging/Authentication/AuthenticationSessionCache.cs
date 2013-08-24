using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Authentication
{
    class AuthenticationSessionCache
    {
        readonly ConcurrentDictionary<Guid, AuthenticationSession> sessions;
        readonly AuthenticationSessionFactory sessionFactory;
        readonly AuthenticationSessionExpirer sessionExpirer;

        public AuthenticationSessionCache(AuthenticationSessionFactory sessionFactory, AuthenticationSessionExpirer sessionExpirer)
        {
            Contract.Requires(sessionFactory != null);
            Contract.Requires(sessionExpirer != null);

            this.sessionFactory = sessionFactory;
            this.sessionExpirer = sessionExpirer;

            sessions = new ConcurrentDictionary<Guid, AuthenticationSession>();

            Messenger.Register<SessionExpired>(DecacheSession);
        }

        void DecacheSession(SessionExpired sessionExpired)
        {
            AuthenticationSession temp;
            sessions.TryRemove(sessionExpired.Session.Id, out temp);
        }

        public bool HasCurrentSessionFor(MessageServer forServer)
        {
            Contract.Requires(forServer != null);
            return forServer != MessageServer.None && sessions.Any(s => s.Value.Server == forServer);
        }

        public AuthenticationSession GetCurrentSessionFor(MessageServer forServer)
        {
            Contract.Requires(forServer != null);

            return sessions.OrderBy(s => s.Value.ExpiresOn).Last(s => s.Value.Server == forServer).Value;
        }

        public void CacheNewSessionFor(MessageServer forServer, ExpiryPlan expiryPlan)
        {
            Contract.Requires(forServer != null);
            Contract.Requires(expiryPlan != null);

            CacheSessionFor(sessionFactory.CreateFromPlan(forServer, expiryPlan));
        }

        public void CacheSessionFor(AuthenticationSession session)
        {
            Contract.Requires(session != null);

            sessions.TryAdd(session.Id, session);
            sessionExpirer.Track(session);
        }

        public bool ContainsSession(AuthenticationSession toCheck)
        {
            Contract.Requires(toCheck != null);
            return sessions.Any(s => s.Value == toCheck);
        }
    }
}