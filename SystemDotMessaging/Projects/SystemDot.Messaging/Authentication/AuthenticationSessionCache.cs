using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Authentication
{
    class AuthenticationSessionCache
    {
        readonly ConcurrentDictionary<MessageServer, AuthenticationSession> sessions;
        readonly AuthenticationSessionFactory sessionFactory;
        readonly AuthenticationSessionExpirer sessionExpirer;

        public AuthenticationSessionCache(AuthenticationSessionFactory sessionFactory, AuthenticationSessionExpirer sessionExpirer)
        {
            this.sessionFactory = sessionFactory;
            this.sessionExpirer = sessionExpirer;
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

            CacheSessionFor(forServer, sessionFactory.CreateFromPlan(expiryPlan));
        }

        public void CacheSessionFor(MessageServer forServer, AuthenticationSession session)
        {
            Contract.Requires(forServer != null);
            Contract.Requires(session != null);

            sessions.TryAdd(forServer, session);
            sessionExpirer.Track(this, session);
        }

        public bool HasCurrentSessionFor(MessageServer forServer)
        {
            Contract.Requires(forServer != null);
            
            return forServer != MessageServer.None && sessions.ContainsKey(forServer);
        }

        public void DecacheSession(AuthenticationSession toDecache)
        {
            sessions.Values.Remove(toDecache);
        }
    }

    class AuthenticationSessionExpirer
    {
        readonly ISystemTime systemTime;
        readonly ITaskScheduler taskScheduler;

        public AuthenticationSessionExpirer(ISystemTime systemTime, ITaskScheduler taskScheduler)
        {
            this.systemTime = systemTime;
            this.taskScheduler = taskScheduler;
        }

        public void Track(AuthenticationSessionCache cache, AuthenticationSession toTrack)
        {
            if (toTrack.GracePeriodEndOn == DateTime.MaxValue) return;
            taskScheduler.ScheduleTask(toTrack.GracePeriodEndOn.Subtract(systemTime.GetCurrentDate()), () => cache.DecacheSession(toTrack));
        }
    }
}