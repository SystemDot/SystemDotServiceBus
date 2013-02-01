using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Repeating;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.expiry
{
    [Subject("Message expiry")]
    public class when_passing_a_message_that_is_not_repeat_attempt_expired_through_an_expirer : WithSubject<MessageExpirer>
    {
        static MessagePayload message;
        static MessagePayload processed;

        Establish context = () =>
        {
            message = new MessagePayload();
            message.IncreaseAmountSent();

            Configure<IMessageExpiryStrategy>(new RepeatAttemptMessageExpiryStrategy(1));

            With<PersistenceBehaviour>();
    
            Subject.MessageProcessed += m => processed = m;
        };

        Because of = () => Subject.InputMessage(message);

        It should_pass_the_message_through = () => processed.ShouldBeTheSameAs(message);
    }
}