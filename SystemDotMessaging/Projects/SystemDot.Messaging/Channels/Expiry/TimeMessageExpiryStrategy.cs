using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Expiry
{
    public class TimeMessageExpiryStrategy : IMessageExpiryStrategy
    {
        readonly TimeSpan expiryTime;
        readonly ICurrentDateProvider currentDateProvider;

        public TimeMessageExpiryStrategy(TimeSpan expiryTime, ICurrentDateProvider currentDateProvider)
        {
            Contract.Requires(expiryTime != null);
            Contract.Requires(currentDateProvider != null);

            this.expiryTime = expiryTime;
            this.currentDateProvider = currentDateProvider;
        }

        public bool HasExpired(MessagePayload toCheck)
        {
            return toCheck.CreatedOn <= this.currentDateProvider.Get().Subtract(this.expiryTime);
        }
    }
}