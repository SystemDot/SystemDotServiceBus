using SystemDot.Configuration;
using SystemDot.Messaging.Configuration.ExternalSources;

namespace SystemDot.Messaging.Configuration
{
    public class ConfigurationBuilderComponent : IConfigurationBuilderComponent
    {
        public void Configure(ConfigurationBuilder builder)
        {
            builder.RegisterBuildAction(c => c.RegisterInstance<IExternalSourcesConfigurer, ExternalSourcesConfigurer>());
        }
    }
}