using SystemDot.Http;
using SystemDot.Messaging.Messages;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class CoreComponents
    {
        public static void Register()
        {
            IocContainer.Register<IWebRequestor>(new WebRequestor());
            IocContainer.Register<ISerialiser>(new PlatformAgnosticSerialiser());
            IocContainer.Register<IMachineIdentifier>(new MachineIdentifier());
            IocContainer.Register(new EndpointAddressBuilder(IocContainer.Resolve<IMachineIdentifier>()));
        }
    }
}