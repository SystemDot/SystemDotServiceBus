using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Authentication
{
    public class ExpiryPlan
    {
        public static ExpiryPlan ExpiresAfter(TimeSpan time)
        {
            return new ExpiryPlan(time);
        }

        public static ExpiryPlan Never()
        {
            return new ExpiryPlan(new TimeSpan(0));
        }

        public TimeSpan AfterTime { get; set; }

        public TimeSpan GracePeriod { get; set; }

        ExpiryPlan(TimeSpan afterTime)
        {
            Contract.Requires(afterTime != null);
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