using System;

namespace SystemDot.Messaging.Authentication
{
    class AuthenticationSessionFactory
    {
        readonly ISystemTime systemTime;

        public AuthenticationSessionFactory(ISystemTime systemTime)
        {
            this.systemTime = systemTime;
        }

        public AuthenticationSession CreateFromPlan(ExpiryPlan from)
        {
            DateTime expiresOn = GetExpiresOnFromPlan(@from);
            DateTime gracePeriodEndsOn = GetGracePeriodEndsOn(@from, expiresOn);

            return new AuthenticationSession(expiresOn, gracePeriodEndsOn);
        }

        static DateTime GetGracePeriodEndsOn(ExpiryPlan from, DateTime expiresOn)
        {
            return @from.AfterTime == TimeSpan.MaxValue ? DateTime.MaxValue : expiresOn.Add(@from.GracePeriod);
        }

        DateTime GetExpiresOnFromPlan(ExpiryPlan from)
        {
            return @from.GracePeriod == TimeSpan.MaxValue ? DateTime.MaxValue : systemTime.GetCurrentDate().Add(@from.AfterTime);
        }
    }
}