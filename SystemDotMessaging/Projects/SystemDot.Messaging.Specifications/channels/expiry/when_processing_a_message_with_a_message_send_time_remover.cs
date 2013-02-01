using System;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Repeating;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.expiry
{
    [Subject("Message expiry")]
    public class when_processing_a_message_with_a_message_send_time_remover : WithMessageProcessorSubject<MessageSendTimeRemover>
    {
        static MessagePayload message;
        static MessagePayload processedMessage;

        Establish context = () =>
        {
            message = new MessagePayload();
            message.SetLastTimeSent(DateTime.Now);
            Subject.MessageProcessed += m => processedMessage = m;
        };

        Because of = () => Subject.InputMessage(message);

        It should_process_the_message = () => processedMessage.ShouldBeTheSameAs(message);

        It should_reset_the_last_time_sent = () => processedMessage.HasHeader<LastSentHeader>().ShouldBeFalse();
    }
}