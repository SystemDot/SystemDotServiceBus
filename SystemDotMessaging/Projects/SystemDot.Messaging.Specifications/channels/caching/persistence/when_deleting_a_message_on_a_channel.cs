using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.caching.persistence
{
    [Subject(SpecificationGroup.Description)]
    public class when_deleting_a_message_on_a_channel : WithSubject<MessageCacheFactory>
    {
        static MessagePayload message;
        static MessageCache messageCache;

        Establish context = () =>
        {
            Configure<IChangeStore>(new InMemoryChangeStore(new PlatformAgnosticSerialiser()));

            messageCache = Subject.CreateCache(
                PersistenceUseType.SubscriberRequestSend,
                new EndpointAddress("GetChannel", "Server"));

            message = new MessagePayload();
            messageCache.AddMessageAndIncrementSequence(message);
        };

        Because of = () => messageCache.Delete(message.Id);

        It should_have_deleted_the_message = () => messageCache.GetMessages().ShouldBeEmpty();
    }
}