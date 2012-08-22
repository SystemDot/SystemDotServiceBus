using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject("Publishing configuration")] 
    public class when_configuring_a_publisher_channel_followed_by_another : WithConfiguationSubject
    {
        const string Channel2Name = "Test2";

        Establish context = () =>
        {
            IocContainerLocator.SetContainer(new IocContainer());
            ConfigureAndRegister<IMachineIdentifier>(new MachineIdentifier());
            ConfigureAndRegister(new EndpointAddressBuilder(new MachineIdentifier()));
            ConfigureAndRegister<ISubscriptionHandlerChannelBuilder>();
            ConfigureAndRegister<IPublisherRegistry>();
            ConfigureAndRegister<IPublisherChannelBuilder>(new TestPublisherChannelBuilder());
            ConfigureAndRegister<IMessageReciever>();
            ConfigureAndRegister<ITaskLooper>();
            ConfigureAndRegister<IBus>();
        };

        Because of = () => Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
                .OpenChannel("Test1").ForPublishing()
                .OpenChannel(Channel2Name).ForPublishing()
            .Initialise();

        It should_build_the_second_publisher_channel = () =>
            The<IPublisherChannelBuilder>().As<TestPublisherChannelBuilder>().ExpectedAddress.ShouldEqual(
                GetEndpointAddress(Channel2Name, The<IMachineIdentifier>().GetMachineName()));


    }
}