using System.Collections.Generic;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Sequencing;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.sequencing
{
    [Subject("Message processing")]
    public class when_running_a_resequncer_with_messages_in_the_correct_order_previously_passed_to_it : WithMessageProcessorSubject<Resequencer>
    {
        static MessagePayload message1;
        static MessagePayload message2;
        static List<MessagePayload> processedMessages;

        Establish context = () =>
        {
            processedMessages = new List<MessagePayload>();
            Subject.MessageProcessed += payload => processedMessages.Add(payload);
            
            message1 = new MessagePayload();
            message1.SetSequence(1);
            Subject.InputMessage(message1);

            message2 = new MessagePayload();
            message2.SetSequence(2);
            Subject.InputMessage(message2);
        };

        Because of = () => Subject.Start();

        It should_pass_the_messages_through = () => processedMessages.ShouldContain(message1, message2);
    }

    [Subject("Message processing")]
    public class when_running_a_resequncer_with_messages_in_the_incorrect_order_previously_passed_to_it : WithMessageProcessorSubject<Resequencer>
    {
        static MessagePayload message1;
        static MessagePayload message2;
        static List<MessagePayload> processedMessages;

        Establish context = () =>
        {
            processedMessages = new List<MessagePayload>();
            Subject.MessageProcessed += payload => processedMessages.Add(payload);

            message1 = new MessagePayload();
            message1.SetSequence(4);
            Subject.InputMessage(message1);

            message2 = new MessagePayload();
            message2.SetSequence(2);
            Subject.InputMessage(message2);
        };

        Because of = () => Subject.Start();

        It should_not_pass_the_messages_through = () => processedMessages.ShouldNotContain(message1, message2);
    }
}