using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Expiry
{
    public class TimeMessageExpiryStrategy : IMessageExpiryStrategy
    {
        readonly TimeSpan expiryTime;
        readonly ISystemTime systemTime;

        public TimeMessageExpiryStrategy(TimeSpan expiryTime, ISystemTime systemTime)
        {
            Contract.Requires(expiryTime != null);
            Contract.Requires(systemTime != null);

            this.expiryTime = expiryTime;
            this.systemTime = systemTime;
        }

        public bool HasExpired(MessagePayload toCheck)
        {
            return toCheck.CreatedOn <= this.systemTime.GetCurrentDate().Subtract(this.expiryTime);
        }
    }
}