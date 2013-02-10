using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.RequestReply;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.requests
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_request_on_a_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderAddress = "TestSenderAddress";

        static int message;
        static MessagePayload payload;
        static TestMessageHandler<int> handler;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                    .OpenChannel(ChannelName)
                    .ForRequestReplyRecieving()
                .Initialise();

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            message = 1;
            payload = new MessagePayload().MakeReceiveable(
                message, 
                SenderAddress, 
                ChannelName, 
                PersistenceUseType.RequestSend);
        };

        Because of = () => MessageServer.ReceiveMessage(payload);

        It should_push_the_message_to_any_registered_handlers = () => handler.LastHandledMessage.ShouldEqual(message);

        It should_send_an_acknowledgement_for_the_message = () =>
            MessageServer.SentMessages.ShouldContain(a => a.GetAcknowledgementId() == payload.GetSourcePersistenceId());

        It should_store_the_sender_address_for_the_reply_to_use = () =>
            Resolve<ReplyAddressLookup>().GetCurrentSenderAddress().ShouldEqual(BuildAddress(SenderAddress));
    }
}