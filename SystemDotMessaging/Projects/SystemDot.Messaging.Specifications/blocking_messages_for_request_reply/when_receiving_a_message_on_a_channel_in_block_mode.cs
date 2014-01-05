using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.blocking_messages_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_on_a_channel_in_block_mode : WithMessageConfigurationSubject
    {
        const string ReceiverChannel = "ReceiverChannel";
        static TestMessageHandler<long> handler;

        Establish context = () =>
        {
            handler = new TestMessageHandler<long>();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverChannel).ForRequestReplyReceiving().InBlockMessagesMode()
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();
        };

        Because of = () => GetServer().ReceiveMessage(
            new MessagePayload()
                .SetMessageBody(1)
                .SetToChannel(ReceiverChannel)
                .SetFromChannel("SenderChannel")
                .SetChannelType(PersistenceUseType.RequestSend)
                .Sequenced());

        It should_not_handle_the_message = () => handler.LastHandledMessage.ShouldNotEqual(1);

        It should_not_send_an_acknowledgement = () => GetServer().SentMessages.ShouldBeEmpty();
    }
}
