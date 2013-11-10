using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.Configuration.Repeating
{
    public class RepeatMessagesConfiguration<TConfiguration>
        where TConfiguration : IRepeatMessagesConfigurer
    {
        readonly TConfiguration configuration;

        public RepeatMessagesConfiguration(TConfiguration configuration)
        {
            Contract.Requires(!configuration.Equals(default(TConfiguration)));

            this.configuration = configuration;
        }

        public TConfiguration Every(TimeSpan toRepeat)
        {
            configuration.SetMessageRepeatingStrategy(new ConstantTimeRepeatStrategy(toRepeat));
            return configuration;
        }

        internal void WithDefaultEscalationStrategy()
        {
            configuration.SetMessageRepeatingStrategy(EscalatingTimeRepeatStrategy.Default);
        }
    }
}