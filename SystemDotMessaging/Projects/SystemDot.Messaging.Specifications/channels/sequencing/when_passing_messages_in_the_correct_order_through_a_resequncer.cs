using System.Collections.Generic;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.sequencing
{
    [Subject("Message processing")]
    public class when_passing_messages_in_the_correct_order_through_a_resequncer : WithMessageProcessorSubject<Resequencer>
    {
        static MessagePayload message1;
        static MessagePayload message2;
        static List<MessagePayload> processedMessages;

        Establish context = () =>
        {
            Configure<IPersistence>(new InMemoryPersistence());
            The<IPersistence>().SetSequence(2);
            
            processedMessages = new List<MessagePayload>();
            Subject.MessageProcessed += payload => processedMessages.Add(payload);
            
            message1 = new MessagePayload();
            message1.SetSequence(2);
            
            message2 = new MessagePayload();
            message2.SetSequence(3);
        };

        Because of = () =>
        {
            Subject.InputMessage(message1);
            Subject.InputMessage(message2);

        };

        It should_pass_the_messages_through = () => processedMessages.ShouldContain(message1, message2);
    }
}