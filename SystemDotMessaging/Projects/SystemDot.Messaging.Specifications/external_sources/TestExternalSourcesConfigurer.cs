using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.ExternalSources;

namespace SystemDot.Messaging.Specifications.external_sources
{
    public class TestExternalSourcesConfigurer : IExternalSourcesConfigurer
    {
        public MessagingConfiguration Configuration { get; set; }
        public MessageServerConfiguration ServerConfiguration { get; set; }

        public void Configure(MessagingConfiguration toConfigureAgainst, MessageServerConfiguration serverToConfigureAgainst)
        {
            this.Configuration = toConfigureAgainst;
            this.ServerConfiguration = serverToConfigureAgainst;
        }
    }
}