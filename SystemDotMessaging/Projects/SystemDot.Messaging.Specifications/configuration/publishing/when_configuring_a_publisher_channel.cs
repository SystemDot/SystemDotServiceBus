using System;
using SystemDot.Ioc;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Messages.Processing.Filtering;
using SystemDot.Messaging.Transport;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject("Publishing configuration")] 
    public class when_configuring_a_publisher_channel : WithPublisherSubject
    {
        const string ChannelName = "Test";
        static IBus bus;

        Because of = () => bus = Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
                .OpenChannel(ChannelName).ForPublishing()
            .Initialise();

        It should_build_the_subscription_handler_channel = () => The<ISubscriptionHandlerChannelBuilder>().WasToldTo(l => l.Build());

        It should_build_the_publisher_channel_with_the_expected_address = () =>
            The<IPublisherChannelBuilder>().As<TestPublisherChannelBuilder>().ExpectedAddress.ShouldEqual(
                GetEndpointAddress(ChannelName, Environment.MachineName));

        It should_build_the_publisher_channel_with_the_default_pass_through_message_filterer = () =>
            The<IPublisherChannelBuilder>().As<TestPublisherChannelBuilder>().ExpectedMessageFilterStrategy
                .ShouldBeOfType<PassThroughMessageFilterStategy>();

        It should_register_the_listening_address_of_the_message_server_with_the_message_reciever = () =>
            The<IMessageReciever>().WasToldTo(r => 
                r.StartPolling(GetEndpointAddress(ChannelName, Environment.MachineName)));

        It should_return_the_bus = () => bus.ShouldBeTheSameAs(The<IBus>());
    }
}