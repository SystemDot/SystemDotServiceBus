using System;
using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Expiry;

namespace SystemDot.Messaging.Configuration
{
    public class MessageExpiry
    {
        public static IMessageExpiryStrategy ByTime(TimeSpan time)
        {
            Contract.Requires(time != null);

            return new TimeMessageExpiryStrategy(time, IocContainerLocator.Locate().Resolve<ICurrentDateProvider>());
        }

        public static IMessageExpiryStrategy ByRepeatAttempt(int attempts)
        {
            return new RepeatAttemptMessageExpiryStrategy(attempts);
        }
    }
}