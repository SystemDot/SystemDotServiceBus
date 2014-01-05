using SystemDot.Messaging.Direct;
using SystemDot.Messaging.Packaging;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.blocking_messages_for_direct_channels
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_on_a_channel_in_block_mode : WithMessageConfigurationSubject
    {
        const string ReceiverChannel = "ReceiverChannel";
        static TestMessageHandler<long> handler;
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            handler = new TestMessageHandler<long>();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenDirectChannel(ReceiverChannel).ForRequestReplyReceiving().InBlockMessagesMode()
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

            messagePayload = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel("SenderChannel")
                .SetToChannel(ReceiverChannel);

            messagePayload.SetIsDirectChannelMessage();
        };

        Because of = () => GetServer().ReceiveMessage(messagePayload);

        It should_not_handle_the_message = () => handler.LastHandledMessage.ShouldNotEqual(1);

        It should_not_send_an_acknowledgement = () => GetServer().SentMessages.ShouldBeEmpty();
    }
}
