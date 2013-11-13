using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Expiry;

namespace SystemDot.Messaging.Configuration.Expiry
{
    public class ExpireMessagesConfiguration<TConfiguration>
        where TConfiguration : IExpireMessagesConfigurer
    {
        readonly TConfiguration configuration;
        readonly ISystemTime systemTime;

        public ExpireMessagesConfiguration(TConfiguration configuration, ISystemTime systemTime)
        {
            Contract.Requires(!configuration.Equals(default(TConfiguration)));

            this.configuration = configuration;
            this.systemTime = systemTime;
        }

        public TConfiguration After(TimeSpan time)
        {
            Contract.Requires(time != null);

            configuration.SetMessageExpiryStrategy(new TimeMessageExpiryStrategy(time, systemTime));
            return configuration;

        }

        public TConfiguration AfterRepeatAttempts(int attempts)
        {
            configuration.SetMessageExpiryStrategy(new RepeatAttemptMessageExpiryStrategy(attempts));
            return configuration;

        }
    }
}