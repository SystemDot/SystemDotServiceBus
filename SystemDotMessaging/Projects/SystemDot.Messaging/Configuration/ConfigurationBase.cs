using System;
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

        protected EndpointAddress BuildEndpointAddress(string address, ServerPath serverPath)
        {
            return GetContainer().Resolve<EndpointAddressBuilder>().Build(address, serverPath);
        }

        protected static IIocContainer GetContainer()
        {
            return IocContainerLocator.Locate();
        }
    }
}