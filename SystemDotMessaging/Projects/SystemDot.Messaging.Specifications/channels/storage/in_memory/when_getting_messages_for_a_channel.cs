using System.Collections.Generic;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.storage.in_memory
{
    [Subject("Message handling")]
    public class when_getting_messages_for_a_channel : WithSubject<InMemoryPersistence>
    {
        static IEnumerable<MessagePayload> messages;
        static MessagePayload message1;
        static MessagePayload message2;

        Establish context = () =>
        {
            message1 = new MessagePayload();
            Subject.AddMessage(message1);

            message2 = new MessagePayload();
            Subject.AddMessage(message2);
        };

        Because of = () => messages = Subject.GetMessages();

        It should_retreive_the_messages = () => messages.ShouldContain(message1, message2);
    }
}