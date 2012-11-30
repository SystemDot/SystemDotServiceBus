using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.caching.persistence.checkpointing
{
    public class when_adding_a_new_message_after_less_than_1000_messages_have_occurred_after_a_checkpoint : WithSubject<MessageCacheFactory>
    {
        static List<MessagePayload> messages;
        static MessagePayload message;
        static MessageCache messageCache;
        static string changeRootId;

        Establish context = () =>
        {
            Configure<IChangeStore>(new InMemoryChangeStore(new PlatformAgnosticSerialiser()));

            messageCache = Subject.CreateCache(
                PersistenceUseType.SubscriberRequestSend,
                new EndpointAddress("GetChannel", "Server"));

            messages = new List<MessagePayload>();

            for (int i = 0; i < 501; i++)
                messages.Add(new MessagePayload());

            messages.ForEach(messageCache.AddMessageAndIncrementSequence);
            messages.ForEach(m => messageCache.Delete(m.Id));

            changeRootId = messageCache.Address + "|" + messageCache.UseType;
            message = new MessagePayload();
        };

        Because of = () =>
        {
            messageCache.AddMessageAndIncrementSequence(message);
            messageCache.Delete(message.Id);
        };

        It should_not_have_cleared_all_previous_changes_up_to_the_second_checkpoint = () =>
            The<IChangeStore>().GetChanges(changeRootId).Count().ShouldNotEqual(2);
    }
}