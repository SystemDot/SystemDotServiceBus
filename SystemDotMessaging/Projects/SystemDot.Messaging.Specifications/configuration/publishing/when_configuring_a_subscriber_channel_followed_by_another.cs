using System;
using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Configuration;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject("Publishing configuration")] 
    public class when_configuring_a_subscriber_channel_followed_by_another : WithSubscriberSubject
    {
        const string Channel1Name = "TestChannel1";
        const string Publisher1Name = "TestPublisher1";
        const string Channel2Name = "TestChannel2";
        const string Publisher2Name = "TestPublisher2";

        Establish context = () =>
        {
            The<ISubscriptionRequestChannelBuilder>()
                .WhenToldTo(b => b.Build(
                    GetEndpointAddress(Channel1Name, Environment.MachineName),
                    GetEndpointAddress(Publisher1Name, Environment.MachineName)))
                .Return(The<ISubscriptionRequestor>());

            The<ISubscriptionRequestChannelBuilder>()
                .WhenToldTo(b => b.Build(
                    GetEndpointAddress(Channel2Name, Environment.MachineName),
                    GetEndpointAddress(Publisher2Name, Environment.MachineName)))
                .Return(The<ISubscriptionRequestor>());
        };

        Because of = () => Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
                .OpenChannel(Channel1Name).ForSubscribingTo(Publisher1Name)
                .OpenChannel(Channel2Name).ForSubscribingTo(Publisher2Name) 
            .Initialise();

        It should_build_the_subscriber_channel_for_both_channels = () => 
            The<ISubscriberChannelBuilder>().WasToldTo(b => 
                b.Build(GetEndpointAddress(Channel2Name, Environment.MachineName)));       
    }
}