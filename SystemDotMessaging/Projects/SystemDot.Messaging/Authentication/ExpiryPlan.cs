using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Authentication
{
    public class ExpiryPlan
    {
        public static ExpiryPlan ExpiresAfter(TimeSpan time)
        {
            Contract.Requires(time != null);
            return new ExpiryPlan(time);
        }

        public static ExpiryPlan Never()
        {
            return new ExpiryPlan(TimeSpan.MinValue).WithGracePeriodOf(TimeSpan.MinValue);
        }

        public TimeSpan AfterTime { get; private set; }

        public TimeSpan GracePeriod { get; private set; }

        ExpiryPlan(TimeSpan afterTime)
        {
            AfterTime = afterTime;
        }

        public ExpiryPlan WithGracePeriodOf(TimeSpan time)
        {
            Contract.Requires(time != null);

            GracePeriod = time;
            return this;
        }
    }
}