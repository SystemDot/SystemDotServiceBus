using System.Diagnostics.Contracts;
using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.Configuration.Repeating
{
    public class EscalatingTimeScalePeakConfiguration<TConfiguration>
        where TConfiguration : IRepeatMessagesConfigurer
    {
        readonly TConfiguration configuration;
        readonly int toStartAt;
        readonly int multiplier;

        public EscalatingTimeScalePeakConfiguration(TConfiguration configuration, int toStartAt, int multiplier)
        {
            Contract.Requires(!configuration.Equals(default(TConfiguration)));

            this.configuration = configuration;
            this.toStartAt = toStartAt;
            this.multiplier = multiplier;
        }

        public TConfiguration PeakingAtSeconds(int peak)
        {
            configuration.SetMessageRepeatingStrategy(new EscalatingTimeRepeatStrategy
            {
                ToStartAt = toStartAt,
                Multiplier = multiplier,
                Peak = peak
            });

            return configuration;
        }
    }
}