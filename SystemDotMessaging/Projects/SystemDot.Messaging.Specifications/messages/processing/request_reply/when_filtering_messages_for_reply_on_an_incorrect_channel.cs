using System;
using SystemDot.Messaging.Messages.Processing;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.processing.request_reply
{
    [Subject("Message processing")]
    public class when_filtering_messages_for_reply_on_an_incorrect_channel : WithSubject<MessageReplyFilter>
    {
        static object message;
        static object processedMessage;
        
        Establish context = () =>
            {
                message = new object();

                Subject = new MessageReplyFilter(Guid.NewGuid(), new ThreadLocalChannelReserve { ReservedChannel = Guid.NewGuid() });
                Subject.MessageProcessed += i => processedMessage = i;
            };

        Because of = () => Subject.InputMessage(message);

        It should_not_pass_the_message_through = () => processedMessage.ShouldBeNull();
    }
}