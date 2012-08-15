using System;
using SystemDot.Messaging.Messages.Processing;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.processing.request_reply
{
    [Subject("Message processing")]
    public class when_passing_messages_through_a_reply_channel_reserver : WithSubject<ReplyChannelReserver>
    {
        static Guid channelIdentifier;
        static object message;
        static object processedMessage;
        
        Establish context = () =>
        {
            message = new object();
            channelIdentifier = Guid.NewGuid();
        
            Subject = new ReplyChannelReserver(channelIdentifier, The<ThreadLocalChannelReserve>());
            Subject.MessageProcessed += i => processedMessage = i;
        };

        Because of = () => Subject.InputMessage(message);

        It should_pass_the_message_through = () => processedMessage.ShouldBeTheSameAs(message);

        It should_reserve_the_channel = () => The<ThreadLocalChannelReserve>().ReservedChannel.ShouldEqual(channelIdentifier);
    }
}