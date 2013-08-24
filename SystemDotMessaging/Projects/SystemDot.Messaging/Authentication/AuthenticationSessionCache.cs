using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Authentication
{
    class AuthenticationSessionCache
    {
        readonly ConcurrentDictionary<MessageServer, AuthenticationSession> sessions;
        readonly AuthenticationSessionFactory sessionFactory;
        readonly AuthenticationSessionExpirer sessionExpirer;

        public AuthenticationSessionCache(AuthenticationSessionFactory sessionFactory, AuthenticationSessionExpirer sessionExpirer)
        {
            Contract.Requires(sessionFactory != null);
            Contract.Requires(sessionExpirer != null);

            this.sessionFactory = sessionFactory;
            this.sessionExpirer = sessionExpirer;

            sessions = new ConcurrentDictionary<MessageServer, AuthenticationSession>();

            Messenger.Register<SessionExpired>(DecacheSession);
        }

        void DecacheSession(SessionExpired sessionExpired)
        {
            AuthenticationSession temp;
            sessions.TryRemove(sessionExpired.Session.Server, out temp);
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

            CacheSessionFor(sessionFactory.CreateFromPlan(forServer, expiryPlan));
        }

        public void CacheSessionFor(AuthenticationSession session)
        {
            Contract.Requires(session != null);

            sessions.TryAdd(session.Server, session);
            sessionExpirer.Track(session);
        }

        public bool HasCurrentSessionFor(MessageServer forServer)
        {
            Contract.Requires(forServer != null);
            return forServer != MessageServer.None && sessions.ContainsKey(forServer);
        }

        public bool Contains(AuthenticationSession toCheck)
        {
            Contract.Requires(toCheck != null);
            return GetCurrentSessionFor(toCheck.Server) == toCheck;
        }
    }
}