using System;
using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject("Publishing configuration")]
    public class when_configuring_a_subscriber_channel : WithSubscriberSubject
    {
        const string ChannelName = "TestChannel";
        const string PublisherName = "TestPublisher";
        static IBus bus;

        Establish context = () =>
            The<ISubscriptionRequestChannelBuilder>()
                .WhenToldTo(b => b.Build(
                    GetEndpointAddress(ChannelName, Environment.MachineName),
                    GetEndpointAddress(PublisherName, Environment.MachineName)))
                .Return(The<ISubscriptionRequestor>());
        
        Because of = () => bus = Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
                .OpenChannel(ChannelName).ForSubscribingTo(PublisherName)
            .Initialise();

        It should_build_the_subscriber_channel = () =>
            The<ISubscriberChannelBuilder>().WasToldTo(
                b => b.Build(GetEndpointAddress(ChannelName, Environment.MachineName)));

        It should_build_and_start_the_subscription_request_channel = () => 
            The<ISubscriptionRequestor>().WasToldTo(b => b.Start());

        It should_register_the_listening_address_of_the_message_server_with_the_message_reciever = () =>
            The<IMessageReciever>().WasToldTo(r => 
                r.StartPolling(GetEndpointAddress(ChannelName, Environment.MachineName)));

        It should_return_the_bus = () => bus.ShouldBeTheSameAs(The<IBus>());
        
    }
}