using System;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Publishing;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    [Subject("Message publishing")]
    public class when_handling_a_non_subscription_request_message_with_the_subscription_request_handler
        : WithMessageInputterSubject<SubscriptionRequestChecker>
    {
        static Exception exception;
        static MessagePayload nonRequest;

        Establish context = () =>
        {
            Configure<PublisherRegistry>(new PublisherRegistry());
            nonRequest = new MessagePayload();
        };

        Because of = () => exception = Catch.Exception(() => Subject.InputMessage(nonRequest));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}