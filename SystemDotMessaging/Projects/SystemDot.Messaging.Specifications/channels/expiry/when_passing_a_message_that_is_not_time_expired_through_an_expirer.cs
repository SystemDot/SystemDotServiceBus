using System;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Expiry;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;
using SystemDot.Specifications;
using Machine.Fakes;
using Machine.Specifications;
using SystemDot.Messaging.Channels.Repeating;

namespace SystemDot.Messaging.Specifications.channels.expiry
{
    [Subject("Message expiry")]
    public class when_passing_a_message_that_is_not_time_expired_through_an_expirer : WithSubject<MessageExpirer>
    {
        static MessagePayload message;
        static MessagePayload processed;

        Establish context = () =>
        {
            message = new MessagePayload();
            message.IncreaseAmountSent();
            
            Configure<IPersistence>(new InMemoryPersistence());
            Configure<IMessageCache>(new MessageCache(The<IPersistence>()));
            
            The<IMessageCache>().Cache(message);

            var expiryTime = new TimeSpan(0, 1, 0);
            
            Configure<IMessageExpiryStrategy>(
                new TimeMessageExpiryStrategy(
                    expiryTime, 
                    new TestCurrentDateProvider(message.CreatedOn.Add(expiryTime))));

            Subject.MessageProcessed += m => processed = m;
        };

        Because of = () => Subject.InputMessage(message);

        It should_not_pass_the_message_through = () => processed.ShouldBeNull();

        It should_remove_the_message_from_the_cache = () => The<IMessageCache>().GetAll().ShouldBeEmpty();
    }
}