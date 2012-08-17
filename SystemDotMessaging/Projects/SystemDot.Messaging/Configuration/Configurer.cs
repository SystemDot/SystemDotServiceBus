using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Configuration
{
    public abstract class Configurer 
    {
        protected static T Resolve<T>()
        {
            return IocContainer.Resolve<T>();
        }

        protected static TType Resolve<TType, TConstructorArg>(TConstructorArg arg)
        {
            return IocContainer.Resolve<TType, TConstructorArg>(arg);
        }

        protected EndpointAddress BuildEndpointAddress(string address, string defaultServerName)
        {
            return IocContainer.Resolve<EndpointAddressBuilder>().Build(address, defaultServerName);
        }
    }
}