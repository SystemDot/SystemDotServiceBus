using System.Collections.Generic;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.caching.persistence.checkpointing
{
    [Subject(persistence.SpecificationGroup.Description)]
    public class when_adding_a_new_message_after_over_1000_changes_have_occurred_and_all_messages_have_been_deleted : WithSubject<MessageCacheFactory>
    {
        static List<MessagePayload> messages;
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
        };

        Because of = () => messageCache.AddMessageAndIncrementSequence(new MessagePayload());

        It should_have_created_a_checkpoint_containing_the_current_state = () =>
            The<IChangeStore>().GetChanges(changeRootId).ShouldContain(c => c is MessageCheckpointChange);
    }
}