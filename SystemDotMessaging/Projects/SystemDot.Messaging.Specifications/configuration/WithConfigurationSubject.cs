using SystemDot.Ioc;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Configuration;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration
{
    public class WithConfigurationSubject : WithSubject<object>
    {
        Establish context = () =>
        {
            IocContainerLocator.SetContainer(new IocContainer());
            MessagePipelineBuilder.BuildSynchronousPipelines = true;
        };

        Cleanup after = () => IocContainerLocator.SetContainer(null);
        
        protected static void ConfigureAndRegister<T>() where T : class
        {
            ConfigureAndRegister(The<T>());
        }

        protected static void ConfigureAndRegister<T>(T toSet) where T : class
        {
            Configure(toSet);
            var concrete = The<T>();

            Register(concrete);
        }

        protected static void Register<T>(T concrete) where T : class
        {
            IocContainerLocator.Locate().RegisterInstance(() => concrete);
        }

        protected static T Resolve<T>() where T : class
        {
            return IocContainerLocator.Locate().Resolve<T>();
        }

        protected static EndpointAddress BuildAddress(string fromAddress)
        {
            return Resolve<EndpointAddressBuilder>().Build(fromAddress, MessageServer.Local().Name);
        }

        protected static EndpointAddress GetEndpointAddress(string channelName, string serverName)
        {
            return The<EndpointAddressBuilder>().Build(channelName, serverName);
        }

        protected static T Deserialise<T>(byte[] toDeserialise)
        {
            return Resolve<ISerialiser>().Deserialise(toDeserialise).As<T>();
        }
    }
}