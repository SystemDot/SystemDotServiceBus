using System.Collections.Generic;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.storage.in_memory
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
            Subject.AddMessage(message1, address);

            message2 = new MessagePayload();
            message2.SetFromAddress(address);
            Subject.AddMessage(message2, address);

            var message3 = new MessagePayload();
            message3.SetFromAddress(new EndpointAddress("Channel1", "Server1"));
            Subject.AddMessage(message3, new EndpointAddress("Channel1", "Server1"));
        };

        Because of = () => messages = Subject.GetMessages(address);

        It should_not_let_the_message_pass_through = () => messages.ShouldContainOnly(message1, message2);
    }
}