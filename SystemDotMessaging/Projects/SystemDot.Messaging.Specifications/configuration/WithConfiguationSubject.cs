using SystemDot.Ioc;
using SystemDot.Messaging.Channels;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration
{
    public class WithConfiguationSubject : WithSubject<object>
    {
        Establish context = () => IocContainerLocator.SetContainer(new IocContainer());

        Cleanup after = () => IocContainerLocator.SetContainer(null);
        
        protected static void ConfigureAndRegister<T>() where T : class
        {
            ConfigureAndRegister(The<T>());
        }

        protected static void ConfigureAndRegister<T>(T toSet) where T : class
        {
            Configure(toSet);
            var concrete = The<T>();

            IocContainerLocator.Locate().RegisterInstance(() => concrete);
        }

        protected static EndpointAddress GetEndpointAddress(string channelName, string serverName)
        {
            return The<EndpointAddressBuilder>().Build(channelName, serverName);
        }
    }
}