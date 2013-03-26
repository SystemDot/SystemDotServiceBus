using System;
using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Configuration
{
    public abstract class Configurer 
    {
        protected static T Resolve<T>() where T : class
        {
            return IocContainerLocator.Locate().Resolve<T>();
        }

        protected static object Resolve(Type type)
        {
            return IocContainerLocator.Locate().Resolve(type);
        }

        protected EndpointAddress BuildEndpointAddress(string address, ServerPath serverPath)
        {
            return IocContainerLocator.Locate().Resolve<EndpointAddressBuilder>().Build(address, serverPath);
        }
    }
}