using System;
using SystemDot.Messaging.Channels.Packaging;
using Machine.Specifications;
using SystemDot.Messaging.Channels.Packaging.Headers;

namespace SystemDot.Messaging.Specifications.configuration.publishing.subscription
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_non_subscription_request_message_on_a_subscription_channel
        : WithPublisherSubject
    {
        const string ChannelName = "Test";

        static Exception exception;
        static MessagePayload nonRequest;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPublishing()
                .Initialise();

            nonRequest = new MessagePayload();
            nonRequest.SetToAddress(BuildAddress(ChannelName));
        };

        Because of = () => exception = Catch.Exception(() => MessageReciever.ReceiveMessage(nonRequest));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}