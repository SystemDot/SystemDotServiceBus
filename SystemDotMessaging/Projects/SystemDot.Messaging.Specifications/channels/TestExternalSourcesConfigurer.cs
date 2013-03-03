using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.ExternalSources;

namespace SystemDot.Messaging.Specifications.channels
{
    public class TestExternalSourcesConfigurer : IExternalSourcesConfigurer
    {
        public MessagingConfiguration Configuration { get; set; }
        
        public void Configure(MessagingConfiguration toConfigureAgainst)
        {
            this.Configuration = toConfigureAgainst;
        }
    }
}