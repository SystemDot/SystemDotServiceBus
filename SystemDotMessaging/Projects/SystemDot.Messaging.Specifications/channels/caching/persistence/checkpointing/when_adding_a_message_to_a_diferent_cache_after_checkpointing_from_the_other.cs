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
    public class when_adding_a_message_to_a_diferent_cache_after_checkpointing_from_the_other 
        : WithSubject<MessageCacheFactory>
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

            for (int i = 0; i < 500; i++)
                messages.Add(new MessagePayload());

            messages.ForEach(messageCache.AddMessageAndIncrementSequence);
            messages.ForEach(m => messageCache.Delete(m.Id));

            MessageCache otherMessageCache = Subject.CreateCache(
                PersistenceUseType.SubscriberRequestSend,
                TestEndpointAddressBuilder.Build("GetChannel1", "Server1"));

            otherMessageCache.AddMessageAndIncrementSequence(new MessagePayload());

            changeRootId = otherMessageCache.Address + "|" + otherMessageCache.UseType;
        };

        Because of = () => messageCache.AddMessageAndIncrementSequence(new MessagePayload());

        It should_not_have_cleared_any_changes_from_the_other_cache = () =>
            The<IChangeStore>().GetChanges(changeRootId).Count().ShouldEqual(1);
    }
}