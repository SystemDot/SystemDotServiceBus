using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.caching.persistence
{
    [Subject(SpecificationGroup.Description)]
    public class when_getting_two_messages_for_a_channel_that_are_not_stored_in_sequence_order : WithSubject<MessageCacheFactory>
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
                TestEndpointAddressBuilder.Build("GetChannel", "Server"));

            message2 = new MessagePayload();
            message2.SetSequence(2);
            messageCache.AddMessage(message2);

            message1 = new MessagePayload();
            message1.SetSequence(1);
            messageCache.AddMessage(message1);
        };

        Because of = () => messages = messageCache.GetMessages();

        It should_retreive_the_first_message_in_order = () => messages.First().ShouldBeTheSameAs(message1);

        It should_retreive_the_second_message_in_order = () => messages.Last().ShouldBeTheSameAs(message2);
    }
}