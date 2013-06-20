using System.Xml;
using SystemDot.Configuration;
using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Pipelines;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications
{
    public class WithConfigurationSubject : WithSubject<object>
    {
        protected static TestTaskRepeater TaskRepeater;
        protected static TestServerAddressConfigurationReader ServerAddressConfiguration;

        Establish context = () =>
        {
            Reset();
            MessagePipelineBuilder.BuildSynchronousPipelines = true;
            ReInitialise();
        };

        protected static void ReInitialise()
        {
            RegisterComponents();
        }

        static void RegisterComponents()
        {
            ConfigureAndRegister<ISerialiser>(new JsonSerialiser());

            TaskRepeater = new TestTaskRepeater();
            ConfigureAndRegister<ITaskRepeater>(TaskRepeater);

            ConfigureAndRegister<MessageHandlerRouter>(new MessageHandlerRouter());

            ServerAddressConfiguration = new TestServerAddressConfigurationReader();
            ConfigureAndRegister<IConfigurationReader>(ServerAddressConfiguration);
        }

        protected static void Reset()
        {
            Messenger.Reset();
            IocContainerLocator.SetContainer(new IocContainer());
        }

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
            Register<T>(IocContainerLocator.Locate(), concrete);
        }
        
        protected static void Register<T>(IIocContainer container, T concrete) where T : class
        {
            container.RegisterInstance(() => concrete);
        }

        protected static void Register<TInterface, TConcrete>() 
            where TInterface : class
            where TConcrete : class
        {
            IocContainerLocator.Locate().RegisterInstance<TInterface, TConcrete>();
        }

        protected static T Resolve<T>() where T : class
        {
            return IocContainerLocator.Locate().Resolve<T>();
        }

        protected static EndpointAddress BuildAddress(string toBuild)
        {
            return new EndpointAddress(toBuild, ServerPath.None);
        }

        protected static EndpointAddress BuildAddress(string channelName, string serverName)
        {
            return TestEndpointAddressBuilder.Build(channelName, serverName);
        }

        protected static EndpointAddress BuildAddress(string channelName, string serverName, string serverAddress)
        {
            return TestEndpointAddressBuilder.Build(channelName, serverName, serverAddress);
        }

        protected static EndpointAddress BuildAddressWithProxy(string channelName, string serverName, string proxyName)
        {
            return TestEndpointAddressBuilder.BuildWithProxy(channelName, serverName, proxyName);
        }
    }
}