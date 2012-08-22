using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Configuration
{
    public abstract class Configurer 
    {
        protected static T Resolve<T>()  where T : class
        {
            return IocContainerLocator.Locate().Resolve<T>();
        }

        protected EndpointAddress BuildEndpointAddress(string address, string defaultServerName)
        {
            return IocContainerLocator.Locate().Resolve<EndpointAddressBuilder>().Build(address, defaultServerName);
        }
    }
}