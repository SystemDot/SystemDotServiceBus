using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Authentication
{
    class AuthenticationSessionFactory
    {
        readonly ISystemTime systemTime;

        public AuthenticationSessionFactory(ISystemTime systemTime)
        {
            Contract.Requires(systemTime != null);
            this.systemTime = systemTime;
        }

        public AuthenticationSession CreateFromPlan(MessageServer server, ExpiryPlan from)
        {
            Contract.Requires(server != null);
            Contract.Requires(from != null);

            DateTime expiresOn = GetExpiresOnFromPlan(@from);
            DateTime gracePeriodEndsOn = GetGracePeriodEndsOn(@from, expiresOn);

            return new AuthenticationSession(server, expiresOn, gracePeriodEndsOn);
        }

        static DateTime GetGracePeriodEndsOn(ExpiryPlan from, DateTime expiresOn)
        {
            return from.AfterTime == TimeSpan.MinValue 
                ? DateTime.MinValue 
                : expiresOn.Add(@from.GracePeriod);
        }

        DateTime GetExpiresOnFromPlan(ExpiryPlan from)
        {
            return from.GracePeriod == TimeSpan.MinValue 
                ? DateTime.MinValue 
                : systemTime.GetCurrentDate().Add(@from.AfterTime);
        }
    }
}