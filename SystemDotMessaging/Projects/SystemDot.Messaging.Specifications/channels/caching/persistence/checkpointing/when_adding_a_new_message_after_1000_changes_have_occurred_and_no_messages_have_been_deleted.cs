using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.caching.persistence.checkpointing
{
    [Subject(persistence.SpecificationGroup.Description)]
    public class when_adding_a_new_message_after_1000_changes_have_occurred_and_no_messages_have_been_deleted : WithSubject<MessageCacheFactory>
    {
        static List<MessagePayload> messages;
        static MessageCache messageCache;
        static string changeRootId;

        Establish context = () =>
        {
            Configure<IChangeStore>(new InMemoryChangeStore(new PlatformAgnosticSerialiser()));

            messageCache = Subject.CreateCache(
                PersistenceUseType.SubscriberRequestSend,
                TestEndpointAddressBuilder.Build("GetChannel", "Server"));

            messages = new List<MessagePayload>();

            for (int i = 0; i < 1000; i++)
                messages.Add(new MessagePayload());

            messages.ForEach(messageCache.AddMessageAndIncrementSequence);

            changeRootId = messageCache.Address + "|" + messageCache.UseType;
        };

        Because of = () => messageCache.AddMessageAndIncrementSequence(new MessagePayload());

        It should_not_have_created_a_checkpoint_containing_the_current_state = () =>
            The<IChangeStore>().GetChanges(changeRootId).ShouldNotContain(c => c is MessageCheckpointChange);

        It should_not_have_cleared_any_changes = () =>
            The<IChangeStore>().GetChanges(changeRootId).Count().ShouldEqual(1001);
    }
}