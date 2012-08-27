using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject("Publishing configuration")] 
    public class when_configuring_a_subscriber_channel_followed_by_another : WithConfiguationSubject
    {
        const string Channel1Name = "TestChannel1";
        const string Publisher1Name = "TestPublisher1";
        const string Channel2Name = "TestChannel2";
        const string Publisher2Name = "TestPublisher2";

        Establish context = () =>
        {
            IocContainerLocator.SetContainer(new IocContainer());
            ConfigureAndRegister<IMachineIdentifier>(new MachineIdentifier());
            ConfigureAndRegister(new EndpointAddressBuilder(new MachineIdentifier()));
            ConfigureAndRegister<ISubscriberChannelBuilder>();
            ConfigureAndRegister<ISubscriptionRequestChannelBuilder>();
            ConfigureAndRegister<IMessageReciever>();
            ConfigureAndRegister<IBus>();

            The<ISubscriptionRequestChannelBuilder>()
                .WhenToldTo(b => b.Build(
                    GetEndpointAddress(Channel1Name, The<IMachineIdentifier>().GetMachineName()),
                    GetEndpointAddress(Publisher1Name, The<IMachineIdentifier>().GetMachineName())))
                .Return(The<ISubscriptionRequestor>());

            The<ISubscriptionRequestChannelBuilder>()
                .WhenToldTo(b => b.Build(
                    GetEndpointAddress(Channel2Name, The<IMachineIdentifier>().GetMachineName()),
                    GetEndpointAddress(Publisher2Name, The<IMachineIdentifier>().GetMachineName())))
                .Return(The<ISubscriptionRequestor>());
        };

        Because of = () => Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
                .OpenChannel(Channel1Name).ForSubscribingTo(Publisher1Name)
                .OpenChannel(Channel2Name).ForSubscribingTo(Publisher2Name) 
            .Initialise();

        It should_build_the_subscriber_channel_for_both_channels = () => 
            The<ISubscriberChannelBuilder>().WasToldTo(b => 
                b.Build(GetEndpointAddress(Channel2Name, The<IMachineIdentifier>().GetMachineName())));       
    }
}