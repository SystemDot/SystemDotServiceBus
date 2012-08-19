using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Configuration
{
    public abstract class Configurer 
    {
        protected static T Resolve<T>()
        {
            return IocContainer.Resolve<T>();
        }

        protected EndpointAddress BuildEndpointAddress(string address, string defaultServerName)
        {
            return IocContainer.Resolve<EndpointAddressBuilder>().Build(address, defaultServerName);
        }
    }
}