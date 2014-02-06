using System;
using System.Diagnostics.Contracts;
using SystemDot.Core;
using SystemDot.Logging;
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
            Logger.Debug("Checking expiry due to time expiry: {0}", toCheck.Id);
            return toCheck.CreatedOn <= systemTime.GetCurrentDate().Subtract(expiryTime);
        }
    }
}