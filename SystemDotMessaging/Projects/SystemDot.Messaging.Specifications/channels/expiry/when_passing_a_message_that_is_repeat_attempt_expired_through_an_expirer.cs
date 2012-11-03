using SystemDot.Messaging.Channels.Expiry;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Repeating;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.expiry
{
    [Subject("Message expiry")]
    public class when_passing_a_message_that_is_repeat_attempt_expired_through_an_expirer : WithSubject<MessageExpirer>
    {
        static MessagePayload message;
        static MessagePayload processed;

        Establish context = () =>
        {
            message = new MessagePayload();
            message.IncreaseAmountSent();
            message.IncreaseAmountSent();

            Configure<IMessageExpiryStrategy>(new RepeatAttemptMessageExpiryStrategy(1));

            Subject.MessageProcessed += m => processed = m;
        };

        Because of = () => Subject.InputMessage(message);

        It should_not_pass_the_message_through = () => processed.ShouldBeNull();
    }
}