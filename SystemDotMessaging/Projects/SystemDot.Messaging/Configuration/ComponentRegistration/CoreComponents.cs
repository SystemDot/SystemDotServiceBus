using SystemDot.Files;
using SystemDot.Http;
using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.ExternalSources;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class CoreComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<IIocContainer>(() => container);
            container.RegisterInstance<IExternalSourcesConfigurer, ExternalSourcesConfigurer>();
            container.RegisterInstance<IFileSystem, FileSystem>();
            container.RegisterInstance<IWebRequestor, WebRequestor>();
            container.RegisterInstance<EndpointAddressBuilder, EndpointAddressBuilder>();
            container.RegisterInstance<ISystemTime, SystemTime>();
        }
    }
}