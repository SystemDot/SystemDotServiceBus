using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Publishing;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    [Subject("Message publishing")]
    public class when_filtering_for_a_subscription_request_message_and_recieving_one
        : WithMessageInputterSubject<SubscriptionRequestFilter>
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