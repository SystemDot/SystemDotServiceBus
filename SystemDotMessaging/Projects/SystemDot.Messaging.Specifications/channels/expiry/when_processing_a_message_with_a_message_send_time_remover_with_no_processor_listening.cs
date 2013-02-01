using System;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Packaging;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.expiry
{
    [Subject("Message expiry")]
    public class when_processing_a_message_with_a_message_send_time_remover_with_no_processor_listening : WithMessageProcessorSubject<MessageSendTimeRemover>
    {
        static Exception exception;

        Because of = () => exception = Catch.Exception(() => Subject.InputMessage(new MessagePayload()));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}