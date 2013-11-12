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
            configuration.SetMessageRepeatingStrategy(new ConstantTimeRepeatStrategy { RepeatEvery = toRepeat });
            return configuration;
        }

        internal void WithDefaultEscalationStrategy()
        {
            OnAnEscalatingTimeScale()
                .StartingAtSeconds(4)
                .EscalatingByAFactorOf(2)
                .PeakingAtSeconds(16);
        }

        public EscalatingTimeScaleStartingConfiguration<TConfiguration> OnAnEscalatingTimeScale()
        {
            return new EscalatingTimeScaleStartingConfiguration<TConfiguration>(configuration);
        }
    }
}