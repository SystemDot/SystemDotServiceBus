using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Configuration.Repeating
{
    public class EscalatingTimeScaleStartingConfiguration<TConfiguration>
        where TConfiguration : IRepeatMessagesConfigurer
    {
        readonly TConfiguration configuration;
        
        public EscalatingTimeScaleStartingConfiguration(TConfiguration configuration)
        {
            Contract.Requires(!configuration.Equals(default(TConfiguration)));

            this.configuration = configuration;
        }

        public EscalatingTimeScaleFactorConfiguration<TConfiguration> StartingAtSeconds(int toStartAt)
        {
            return new EscalatingTimeScaleFactorConfiguration<TConfiguration>(configuration, toStartAt);
        }
    }
}