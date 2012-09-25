using System;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Publishing;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    [Subject("Message publishing")]
    public class when_filtering_for_a_subscription_request_message_and_recieving_something_else
        : WithMessageInputterSubject<SubscriptionRequestFilter>
    {
        static MessagePayload processedMessage;
        static MessagePayload nonRequest;

        Establish context = () =>
        {
            Subject.MessageProcessed += m => processedMessage = m;
            nonRequest = new MessagePayload();
        };

        Because of = () => Subject.InputMessage(nonRequest);

        It should_not_process_the_message = () => processedMessage.ShouldBeNull();
    }
}