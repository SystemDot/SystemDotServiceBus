using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Ioc;

namespace SystemDot.Messaging.Configuration
{
    public abstract class ConfigurationBase 
    {
        protected static T Resolve<T>() where T : class
        {
            return GetContainer().Resolve<T>();
        }

        protected EndpointAddress BuildEndpointAddress(string address, MessageServer server)
        {
            return GetContainer().Resolve<EndpointAddressBuilder>().Build(address, server);
        }

        protected static IIocContainer GetContainer()
        {
            return IocContainerLocator.Locate();
        }
    }
}