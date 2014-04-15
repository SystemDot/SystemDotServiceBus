using SystemDot.Configuration;
using SystemDot.Messaging.Configuration.ExternalSources;
using SystemDot.Messaging.Hooks.External;

namespace SystemDot.Messaging.Configuration
{
    public class ConfigurationBuilderComponent : IConfigurationBuilderComponent
    {
        public void Configure(ConfigurationBuilder builder)
        {
            builder.RegisterBuildAction(c => c.RegisterInstance<IExternalSourcesConfigurer, ExternalSourcesConfigurer>());
            builder.RegisterBuildAction(c => c.RegisterInstance<ExternalInspectorHook, ExternalInspectorHook>());
            builder.RegisterBuildAction(c => c.RegisterInstance<IExternalInspectorLoader, ExternalInspectorLoader>());
        }
    }
}