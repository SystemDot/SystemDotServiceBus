using System;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Processing;
using Machine.Fakes;
using Machine.Specifications;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Specifications.messages.processing.request_reply
{
    [Subject("Message processing")]
    public class when_passing_messages_through_a_reply_channel_selector : WithSubject<ReplyChannelSelector>
    {
        static EndpointAddress address;
        static MessagePayload message;
        static MessagePayload processedMessage;
        
        Establish context = () =>
        {
            address = new EndpointAddress("Channel", "Server");
            
            Subject = new ReplyChannelSelector(The<ReplyChannelLookup>());
            Subject.MessageProcessed += i => processedMessage = i;

            message = new MessagePayload();
            message.SetFromAddress(address);
        };

        Because of = () => Subject.InputMessage(message);

        It should_pass_the_message_through = () => processedMessage.ShouldBeTheSameAs(message);

        It should_have_set_the_current_reply_channel = () =>
            The<ReplyChannelLookup>().GetCurrentChannel().ShouldBeTheSameAs(address);
    }
}