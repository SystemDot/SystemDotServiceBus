using System;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Specifications;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.expiry
{
    [Subject("Message expiry")]
    public class when_passing_a_message_that_is_time_expired_through_an_expirer : WithSubject<MessageExpirer>
    {
        static MessagePayload message;
        static MessagePayload processed;

        Establish context = () =>
        {
            message = new MessagePayload();
            message.SetSequence(1);
            message.IncreaseAmountSent();

            With<PersistenceBehaviour>();
            The<SendMessageCache>().AddMessageAndIncrementSequence(message);

            var expiryTime = new TimeSpan(0, 1, 0);

            Configure<IMessageExpiryStrategy>(
                new TimeMessageExpiryStrategy(
                    expiryTime, 
                    new TestSystemTime(message.CreatedOn.Add(expiryTime).AddMilliseconds(-1))));

            Subject.MessageProcessed += m => processed = m;
        };

        Because of = () => Subject.InputMessage(message);

        It should_pass_the_message_through = () => processed.ShouldBeTheSameAs(message);

        It should_remove_the_message_from_the_cache = () =>
            The<SendMessageCache>().GetMessages().ShouldContain(message);
    }
}