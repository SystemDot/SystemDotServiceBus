using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Configuration
{
    public abstract class Configurer 
    {
        public static TType Resolve<TType, TConstructorArg>(TConstructorArg arg)
        {
            return IocContainer.Resolve<TType, TConstructorArg>(arg);
        }

        public static TType Resolve<TType>()
        {
            return IocContainer.Resolve<TType>();
        }

        protected EndpointAddress BuildEndpointAddress(string address, string defaultServerName)
        {
            return Resolve<EndpointAddressBuilder>().Build(address, defaultServerName);
        }
    }
}