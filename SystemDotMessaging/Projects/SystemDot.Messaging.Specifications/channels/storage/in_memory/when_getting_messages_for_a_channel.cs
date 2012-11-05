using System.Collections.Generic;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.storage.in_memory
{
    [Subject("Message storage")]
    public class when_getting_messages_for_a_channel : WithSubject<PersistenceFactory>
    {
        static IEnumerable<MessagePayload> messages;
        static MessagePayload message1;
        static MessagePayload message2;
        static IPersistence persistence;

        Establish context = () =>
        {
            Configure<IChangeStore>(new InMemoryChangeStore(new PlatformAgnosticSerialiser()));

            persistence = Subject.CreatePersistence(
                PersistenceUseType.SubscriberRequestSend, 
                new EndpointAddress("Channel", "Server"));

            message1 = new MessagePayload();
            persistence.AddMessageAndIncrementSequence(message1);

            message2 = new MessagePayload();
            persistence.AddMessageAndIncrementSequence(message2);
        };

        Because of = () => messages = persistence.GetMessages();

        It should_retreive_the_messages = () => messages.ShouldContain(message1, message2);
    }
}