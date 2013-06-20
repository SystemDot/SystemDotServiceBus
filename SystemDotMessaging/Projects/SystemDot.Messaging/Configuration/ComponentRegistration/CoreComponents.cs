using SystemDot.Configuration;
using SystemDot.Files;
using SystemDot.Http;
using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.ExternalSources;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    static class CoreComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<IIocContainer>(() => container);
            container.RegisterInstance<IExternalSourcesConfigurer, ExternalSourcesConfigurer>();
            container.RegisterInstance<IFileSystem, FileSystem>();
            container.RegisterInstance<IWebRequestor, WebRequestor>();
            container.RegisterInstance<EndpointAddressBuilder, EndpointAddressBuilder>();
            container.RegisterInstance<ServerPathBuilder, ServerPathBuilder>();
            container.RegisterInstance<ISystemTime, SystemTime>();
            container.RegisterInstance<IConfigurationReader, ConfigurationReader>();
            container.RegisterInstance<ServerAddressLoader, ServerAddressLoader>();
            container.RegisterInstance<ServerAddressRegistry, ServerAddressRegistry>();
        }
    }
}