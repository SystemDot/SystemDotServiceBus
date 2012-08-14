using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Configuration.HttpMessaging;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.Http.LongPolling;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject("Publishing configuration")] 
    public class when_configuring_a_publisher_channel : WithConfiguationSubject
    {
        const string ChannelName = "Test";
        static IBus bus;
        
        Establish context = () =>
        {
            Components.Registration = () =>
            {
                ConfigureAndRegister<IMachineIdentifier>(new MachineIdentifier());
                ConfigureAndRegister<EndpointAddressBuilder>(new EndpointAddressBuilder(IocContainer.Resolve<IMachineIdentifier>()));
                ConfigureAndRegister<ISubscriptionHandlerChannelBuilder>();
                ConfigureAndRegister<IPublisherRegistry>();
                ConfigureAndRegister<IPublisherChannelBuilder>();
                ConfigureAndRegister<IMessageReciever>();
                ConfigureAndRegister<ITaskLooper>();
                ConfigureAndRegister<IBus>();

                The<IPublisherChannelBuilder>().WhenToldTo(b => b.Build()).Return(The<IDistributor>());
            };
        };

        Because of = () => bus = Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
                .OpenChannel(ChannelName).ForPublishing()
            .Initialise();

        It should_build_the_subscription_handler_channel = () => The<ISubscriptionHandlerChannelBuilder>().WasToldTo(l => l.Build());

        It should_build_and_register_the_publisher_channel = () => 
            The<IPublisherRegistry>().WasToldTo(l => 
                l.RegisterPublisher(
                    GetEndpointAddress(ChannelName, The<IMachineIdentifier>().GetMachineName()),
                    The<IDistributor>()));

        It should_register_the_listening_address_of_the_message_server_with_the_message_reciever = () =>
            The<IMessageReciever>().WasToldTo(r => 
                r.RegisterListeningAddress(GetEndpointAddress(ChannelName, The<IMachineIdentifier>().GetMachineName())));

        It should_start_the_task_looper = () => The<ITaskLooper>().WasToldTo(l => l.Start());

        It should_return_the_bus = () => bus.ShouldBeTheSameAs(The<IBus>());
        
    }
}