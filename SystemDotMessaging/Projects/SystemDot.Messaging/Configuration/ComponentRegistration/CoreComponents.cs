using SystemDot.Http;
using SystemDot.Ioc;
using SystemDot.Messaging.Messages;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class CoreComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<IWebRequestor, WebRequestor>();
            container.RegisterInstance<ISerialiser, PlatformAgnosticSerialiser>();
            container.RegisterInstance<IMachineIdentifier, MachineIdentifier>();
            container.RegisterInstance<ITypeExtender, TypeExtender>();
            container.RegisterInstance<EndpointAddressBuilder, EndpointAddressBuilder>();
        }
    }
}