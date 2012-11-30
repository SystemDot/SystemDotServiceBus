using SystemDot.Messaging.Channels.Expiry;
using SystemDot.Messaging.Channels.Packaging;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.expiry
{
    [Subject("Message expiry")]
    public class when_passing_a_message_through_a_expirer_with_a_pass_through_strategy : WithSubject<MessageExpirer>
    {
        static MessagePayload message;
        static MessagePayload processed;

        Establish context = () =>
        {
            message = new MessagePayload();

            Configure<IMessageExpiryStrategy>(new PassthroughMessageExpiryStrategy());
            With<PersistenceBehaviour>();
    
            Subject.MessageProcessed += m => processed = m;
        };

        Because of = () => Subject.InputMessage(message);

        It should_pass_the_message_through = () => processed.ShouldBeTheSameAs(message);
    }
}