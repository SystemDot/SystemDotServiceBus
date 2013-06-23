using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.publishing_subscription
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
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPublishing()
                .Initialise();

            nonRequest = new MessagePayload();
            nonRequest.SetToAddress(BuildAddress(ChannelName));
        };

        Because of = () => exception = Catch.Exception(() => Server.ReceiveMessage(nonRequest));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}