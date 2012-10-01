using System;
using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Expiry;

namespace SystemDot.Messaging.Configuration
{
    public class MessageExpiry
    {
        public static IMessageExpiryStrategy ByTime(TimeSpan time)
        {
            Contract.Requires(time != null);

            return new TimeMessageExpiryStrategy(time, IocContainerLocator.Locate().Resolve<ICurrentDateProvider>());
        }
    }
}