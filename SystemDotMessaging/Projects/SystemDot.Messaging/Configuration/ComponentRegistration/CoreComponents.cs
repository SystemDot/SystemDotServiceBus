using SystemDot.Files;
using SystemDot.Http;
using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class CoreComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<IIocContainer>(() => container);
            container.RegisterInstance<IFileSystem, FileSystem>();
            container.RegisterInstance<IWebRequestor, WebRequestor>();
            container.RegisterInstance<ISerialiser, PlatformAgnosticSerialiser>();
            container.RegisterInstance<EndpointAddressBuilder, EndpointAddressBuilder>();
            container.RegisterInstance<ICurrentDateProvider, CurrentDateProvider>();
        }
    }
}