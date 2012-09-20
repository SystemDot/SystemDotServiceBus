using System.Collections.Generic;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Messaging.Messages.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.storage.in_memory
{
    [Subject("Message handling")]
    public class when_getting_messages_for_a_channel : WithSubject<InMemoryPersistence>
    {
        static IEnumerable<MessagePayload> messages;
        static EndpointAddress address;
        static MessagePayload message1;
        static MessagePayload message2;

        Establish context = () =>
        {
            address = new EndpointAddress("Channel", "Server");

            message1 = new MessagePayload();
            message1.SetFromAddress(address);
            Subject.StoreMessage(message1);

            message2 = new MessagePayload();
            message2.SetFromAddress(address);
            Subject.StoreMessage(message2);

            var message3 = new MessagePayload();
            message3.SetFromAddress(new EndpointAddress("Channel1", "Server1"));
            Subject.StoreMessage(message3);
        };

        Because of = () => messages = Subject.GetMessages(address);

        It should_not_let_the_message_pass_through = () => messages.ShouldContainOnly(message1, message2);
    }
}