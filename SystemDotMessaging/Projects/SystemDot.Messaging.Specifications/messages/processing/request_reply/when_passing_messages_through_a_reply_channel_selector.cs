using System;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Processing;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.processing.request_reply
{
    [Subject("Message processing")]
    public class when_passing_messages_through_a_reply_channel_selector : WithSubject<ReplyChannelSelector>
    {
        static EndpointAddress address;
        static ChannelStartPoint channelStartPoint;
        static object message;
        static object processedMessage;
        
        Establish context = () =>
        {
            message = new object();
            address = new EndpointAddress("Channel", "Server");
            channelStartPoint = new ChannelStartPoint();

            The<ReplyChannelLookup>().RegisterChannel(new EndpointAddress("Channel1", "Server1"), new ChannelStartPoint());
            The<ReplyChannelLookup>().RegisterChannel(address, channelStartPoint);

            Subject = new ReplyChannelSelector(address, The<ReplyChannelLookup>());
            Subject.MessageProcessed += i => processedMessage = i;
        };

        Because of = () => Subject.InputMessage(message);

        It should_pass_the_message_through = () => processedMessage.ShouldBeTheSameAs(message);

        It should_have_set_the_current_reply_channel = () => The<ReplyChannelLookup>().GetCurrentChannel().ShouldBeTheSameAs(channelStartPoint);
    }
}