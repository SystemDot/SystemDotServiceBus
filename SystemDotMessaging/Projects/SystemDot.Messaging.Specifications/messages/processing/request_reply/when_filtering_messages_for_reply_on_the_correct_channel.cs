using System;
using SystemDot.Messaging.Messages.Processing;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.processing.request_reply
{
    [Subject("Message processing")]
    public class when_filtering_messages_for_reply_on_the_correct_channel : WithSubject<MessageReplyFilter>
    {
        static object message;
        static object processedMessage;
        static Guid channelId;
        
        Establish context = () =>
        {
            message = new object();

            channelId = Guid.NewGuid();
            
            Subject = new MessageReplyFilter(channelId, new ThreadLocalChannelReserve { ReservedChannel = channelId });
            Subject.MessageProcessed += i => processedMessage = i;
        };

        Because of = () => Subject.InputMessage(message);

        It should_pass_the_message_through = () => processedMessage.ShouldBeTheSameAs(message);
    }
}