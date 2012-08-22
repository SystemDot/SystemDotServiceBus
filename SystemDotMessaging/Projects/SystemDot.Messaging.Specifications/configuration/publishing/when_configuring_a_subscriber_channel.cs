using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.Http.LongPolling;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject("Publishing configuration")] 
    public class when_configuring_a_subscriber_channel : WithConfiguationSubject
    {
        const string ChannelName = "TestChannel";
        const string PublisherName = "TestPublisher";
        static IBus bus;

        Establish context = () =>
        {
            IocContainerLocator.SetContainer(new IocContainer());
            ConfigureAndRegister<IMachineIdentifier>(new MachineIdentifier());
            ConfigureAndRegister(new EndpointAddressBuilder(new MachineIdentifier()));
            ConfigureAndRegister<ISubscriberChannelBuilder>();
            ConfigureAndRegister<ISubscriptionRequestChannelBuilder>();
            ConfigureAndRegister<IMessageReciever>();
            ConfigureAndRegister<ITaskLooper>();
            ConfigureAndRegister<IBus>();

            The<ISubscriptionRequestChannelBuilder>()
                .WhenToldTo(b => b.Build(
                    GetEndpointAddress(ChannelName, The<IMachineIdentifier>().GetMachineName()),
                    GetEndpointAddress(PublisherName, The<IMachineIdentifier>().GetMachineName())))
                .Return(The<ISubscriptionRequestor>());
        };

        Because of = () => bus = Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
                .OpenChannel(ChannelName).ForSubscribingTo(PublisherName)
            .Initialise();

        It should_build_the_subscriber_channel = () =>
            The<ISubscriberChannelBuilder>().WasToldTo(
                b => b.Build(GetEndpointAddress(ChannelName, The<IMachineIdentifier>().GetMachineName())));

        It should_build_and_start_the_subscription_request_channel = () => 
            The<ISubscriptionRequestor>().WasToldTo(b => b.Start());

        It should_register_the_listening_address_of_the_message_server_with_the_message_reciever = () =>
            The<IMessageReciever>().WasToldTo(r => 
                r.RegisterListeningAddress(GetEndpointAddress(ChannelName, The<IMachineIdentifier>().GetMachineName())));

        It should_start_the_task_looper = () => The<ITaskLooper>().WasToldTo(l => l.Start());

        It should_return_the_bus = () => bus.ShouldBeTheSameAs(The<IBus>());
        
    }
}