using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject("Publishing configuration")] 
    public class when_configuring_a_publisher_channel_followed_by_another : WithConfiguationSubject
    {
        const string Channel2Name = "Test2";
        
        Establish context = () =>
        {
            Components.Registration = () =>
            {
                ConfigureAndRegister<IMachineIdentifier>(new MachineIdentifier());
                ConfigureAndRegister(new EndpointAddressBuilder(IocContainer.Resolve<IMachineIdentifier>()));
                ConfigureAndRegister<ISubscriptionHandlerChannelBuilder>();
                ConfigureAndRegister<IPublisherRegistry>();
                ConfigureAndRegister<IPublisherChannelBuilder>();
                ConfigureAndRegister<IMessageReciever>();
                ConfigureAndRegister<ITaskLooper>();
                ConfigureAndRegister<IBus>();

                The<IPublisherChannelBuilder>().WhenToldTo(b => b.Build()).Return(The<IDistributor>());
            };
        };

        Because of = () => Configuration.Configure
            .WithLocalMessageServer()
                .OpenChannel("Test1")
                    .AsPublisher()
                .OpenChannel(Channel2Name)
                    .AsPublisher()
            .Initialise();

        It should_build_and_register_the_second_publisher_channel = () => 
            The<IPublisherRegistry>().WasToldTo(l => 
                l.RegisterPublisher(
                    GetEndpointAddress(Channel2Name, The<IMachineIdentifier>().GetMachineName()),
                    The<IDistributor>()));

        
    }
}