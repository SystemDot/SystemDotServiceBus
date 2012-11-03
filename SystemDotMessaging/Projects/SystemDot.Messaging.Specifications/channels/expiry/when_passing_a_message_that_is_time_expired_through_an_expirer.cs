using System;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Expiry;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;
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
            message.IncreaseAmountSent();

            With<PersistenceBehaviour>();
            The<IPersistence>().AddMessageAndIncrementSequence(message);

            var expiryTime = new TimeSpan(0, 1, 0);

            Configure<IMessageExpiryStrategy>(
                new TimeMessageExpiryStrategy(
                    expiryTime, 
                    new TestCurrentDateProvider(message.CreatedOn.Add(expiryTime).AddMilliseconds(-1))));

            Subject.MessageProcessed += m => processed = m;
        };

        Because of = () => Subject.InputMessage(message);

        It should_pass_the_message_through = () => processed.ShouldBeTheSameAs(message);

        It should_remove_the_message_from_the_cache = () =>
            The<IPersistence>().GetMessages().ShouldContain(message);
    }
}