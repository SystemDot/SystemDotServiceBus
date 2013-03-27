using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.ExternalSources;

namespace SystemDot.Messaging.Specifications
{
    public class TestExternalSourcesConfigurer : IExternalSourcesConfigurer
    {
        public MessageServerConfiguration Configuration { get; set; }

        public void Configure(MessageServerConfiguration toConfigureAgainst)
        {
            this.Configuration = toConfigureAgainst;
        }
    }
}