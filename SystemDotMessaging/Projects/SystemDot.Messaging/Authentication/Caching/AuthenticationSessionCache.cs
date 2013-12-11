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
        readonly ConcurrentDictionary<Guid, ServerSession> sessions;
        readonly AuthenticationSessionFactory sessionFactory;
        readonly AuthenticationSessionExpirer sessionExpirer;

        public AuthenticationSessionCache(
            AuthenticationSessionFactory sessionFactory,
            AuthenticationSessionExpirer sessionExpirer,
            ChangeStore changeStore, 
            ICheckpointStrategy checkPointStrategy)
            : base(changeStore, checkPointStrategy)
        {
            Contract.Requires(sessionFactory != null);
            Contract.Requires(sessionExpirer != null);
            Contract.Requires(changeStore != null);

            this.sessionFactory = sessionFactory;
            this.sessionExpirer = sessionExpirer;
            sessions = new ConcurrentDictionary<Guid, ServerSession>();

            Messenger.Register<AuthenticationSessionExpired>(e => DecacheSession(e.Session));

            Id = "AuthenticationSessions";
        }

        void DecacheSession(AuthenticationSession session)
        {
            ServerSession temp;
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
                .OrderBy(s => s.Value.Session.CreatedOn)
                .Last(s => s.Value.Server == forServer).Value.Session;
        }

        public void CacheNewSessionFor(MessageServer forServer, TimeSpan expiresAfter)
        {
            Contract.Requires(forServer != null);
            Contract.Requires(expiresAfter != null);

            CacheSessionFor(this.sessionFactory.Create(expiresAfter), forServer);
        }

        public void CacheSessionFor(AuthenticationSession session, MessageServer forServer)
        {
            Contract.Requires(session != null);
            Contract.Requires(forServer != null);

            AddChange(new AuthenticationSessionCachedChange(forServer, session));
        }

        public void ApplyChange(AuthenticationSessionCachedChange change)
        {
            if (change.Session.ExpiresOn < SystemTime.Current.GetCurrentDate()) return;

            sessions.TryAdd(change.Session.Id, new ServerSession(change.Server, change.Session));
            sessionExpirer.Track(change.Server, change.Session);
        }

        public bool ContainsSession(AuthenticationSession toCheck)
        {
            Contract.Requires(toCheck != null);

            return sessions.Any(s => s.Value.Session == toCheck);
        }

        protected override void UrgeCheckPoint()
        {
            if (sessions.Count == 0)
                CheckPoint(new AuthenticationSessionCheckpointChange());
        }
    }
}