using SystemDot.Http;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Messages;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class CoreComponents
    {
        public static void Register()
        {
            IocContainer.RegisterInstance<IWebRequestor, WebRequestor>();
            IocContainer.RegisterInstance<ISerialiser, PlatformAgnosticSerialiser>();
            IocContainer.RegisterInstance<IMachineIdentifier, MachineIdentifier>();
            IocContainer.RegisterInstance<EndpointAddressBuilder, EndpointAddressBuilder>();
        }
    }
}