using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Processing;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.bus
{
    [Subject("Message bus")]
    public class when_replying_with_a_message_on_the_bus : WithSubject<MessageBus>
    {
        static object message;
        static object processedMessage;
        static ChannelStartPoint channelStartPoint;
        static EndpointAddress replyAddress;

        Establish context = () =>
        {
            replyAddress = new EndpointAddress("Channel", "Server");
            channelStartPoint = new ChannelStartPoint();
            The<ReplyChannelLookup>().RegisterChannel(new EndpointAddress("Channel1", "Server1"), new ChannelStartPoint());
            The<ReplyChannelLookup>().RegisterChannel(replyAddress, channelStartPoint);
            The<ReplyChannelLookup>().SetCurrentChannel(replyAddress);

            channelStartPoint.MessageProcessed += m => processedMessage = m; 
            message = new object();
        };

        Because of = () => Subject.Reply(message);

        It should_send_the_message_on_the_correct_channel = () => processedMessage.ShouldBeTheSameAs(message);
    }
}
