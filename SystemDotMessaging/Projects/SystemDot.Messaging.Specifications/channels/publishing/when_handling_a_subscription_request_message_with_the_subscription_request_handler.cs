using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Publishing;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    [Subject("Message publishing")]
    public class when_handling_a_subscription_request_message_with_the_subscription_request_handler
        : WithMessageInputterSubject<SubscriptionRequestHandler>
    {
        static MessagePayload processedMessage;
        static MessagePayload request;

        Establish context = () =>
        {
            Subject.MessageProcessed += m => processedMessage = m;
            request = new MessagePayload();
            request.SetSubscriptionRequest(new SubscriptionSchema());
        };

        Because of = () => Subject.InputMessage(request);

        It should_process_the_message = () => processedMessage.ShouldBeTheSameAs(request);
    }
}