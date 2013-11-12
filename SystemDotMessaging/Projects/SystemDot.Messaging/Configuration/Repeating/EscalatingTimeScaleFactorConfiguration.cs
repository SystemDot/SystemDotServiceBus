using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Configuration.Repeating
{
    public class EscalatingTimeScaleFactorConfiguration<TConfiguration>
        where TConfiguration : IRepeatMessagesConfigurer
    {
        readonly TConfiguration configuration;
        readonly int toStartAt;

        public EscalatingTimeScaleFactorConfiguration(TConfiguration configuration, int toStartAt)
        {
            Contract.Requires(!configuration.Equals(default(TConfiguration)));

            this.configuration = configuration;
            this.toStartAt = toStartAt;
        }

        public EscalatingTimeScalePeakConfiguration<TConfiguration> EscalatingByAFactorOf(int multiplier)
        {
            return new EscalatingTimeScalePeakConfiguration<TConfiguration>(configuration, toStartAt, multiplier);
        }
    }
}