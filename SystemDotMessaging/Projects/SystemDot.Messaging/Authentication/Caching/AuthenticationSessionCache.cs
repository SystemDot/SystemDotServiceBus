using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication.Caching.Changes;
using SystemDot.Messaging.Authentication.Expiry;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Authentication.Caching
{
    class AuthenticationSessionCache : ChangeRoot
    {
        readonly ConcurrentDictionary<Guid, AuthenticationSession> sessions;
        readonly AuthenticationSessionFactory sessionFactory;
        readonly AuthenticationSessionExpirer sessionExpirer;

        public AuthenticationSessionCache(
            AuthenticationSessionFactory sessionFactory, 
            AuthenticationSessionExpirer sessionExpirer, 
            IChangeStore changeStore) 
            : base(changeStore)
        {
            Contract.Requires(sessionFactory != null);
            Contract.Requires(sessionExpirer != null);
            Contract.Requires(changeStore != null);

            this.sessionFactory = sessionFactory;
            this.sessionExpirer = sessionExpirer;
            sessions = new ConcurrentDictionary<Guid, AuthenticationSession>();

            Messenger.Register<AuthenticationSessionExpired>(e => DecacheSession(e.Session));
            
            Id = "AuthenticationSessions";
        }

        void DecacheSession(AuthenticationSession session)
        {
            AuthenticationSession temp;
            sessions.TryRemove(session.Id, out temp);
        }

        public bool HasCurrentSessionFor(MessageServer forServer)
        {
            Contract.Requires(forServer != null);

            return forServer != MessageServer.None 
                && sessions.Any(s => s.Value.Server == forServer);
        }

        public AuthenticationSession GetCurrentSessionFor(MessageServer forServer)
        {
            Contract.Requires(forServer != null);

            return sessions
                .OrderBy(s => s.Value.CreatedOn)
                .Last(s => s.Value.Server == forServer).Value;
        }

        public void CacheNewSessionFor(MessageServer forServer, TimeSpan expiresAfter)
        {
            Contract.Requires(forServer != null);
            Contract.Requires(expiresAfter != null);

            CacheSessionFor(sessionFactory.Create(forServer, expiresAfter));
        }

        public void CacheSessionFor(AuthenticationSession session)
        {
            Contract.Requires(session != null);

            AddChange(new AuthenticationSessionCachedChange(session));
        }

        public void ApplyChange(AuthenticationSessionCachedChange change)
        {
            if (change.Session.ExpiresOn < SystemTime.Current.GetCurrentDate()) return;

            sessions.TryAdd(change.Session.Id, change.Session);
            sessionExpirer.Track(change.Session);
        }

        public bool ContainsSession(AuthenticationSession toCheck)
        {
            Contract.Requires(toCheck != null);

            return sessions.Any(s => s.Value == toCheck);
        }

        protected override void UrgeCheckPoint()
        {
            if (sessions.Count == 0)
                CheckPoint(new AuthenticationSessionCheckpointChange());
        }
    }
}