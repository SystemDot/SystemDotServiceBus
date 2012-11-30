using System.Collections.Generic;
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
    public class when_getting_messages_for_a_channel : WithSubject<MessageCacheFactory>
    {
        static IEnumerable<MessagePayload> messages;
        static MessagePayload message1;
        static MessagePayload message2;
        static MessageCache messageCache;

        Establish context = () =>
        {
            Configure<IChangeStore>(new InMemoryChangeStore(new PlatformAgnosticSerialiser()));

            messageCache = Subject.CreateCache(
                PersistenceUseType.SubscriberRequestSend, 
                new EndpointAddress("GetChannel", "Server"));

            message1 = new MessagePayload();
            messageCache.AddMessageAndIncrementSequence(message1);

            message2 = new MessagePayload();
            messageCache.AddMessageAndIncrementSequence(message2);
        };

        Because of = () => messages = messageCache.GetMessages();

        It should_retreive_the_messages = () => messages.ShouldContain(message1, message2);
    }
}