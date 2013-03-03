using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.RequestReply;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.channels.request_reply.requests
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_request_on_a_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderAddress = "TestSenderAddress";
        const int Message = 1;

        static MessagePayload payload;
        static TestMessageHandler<int> handler;
        static ChannelBuilt channelBuiltHandler;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                    .OpenChannel(ChannelName)
                    .ForRequestReplyRecieving()
                .Initialise();

            Messenger.Register<ChannelBuilt>(m => channelBuiltHandler = m);

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            payload = new MessagePayload().MakeSequencedReceivable(
                Message, 
                SenderAddress, 
                ChannelName, 
                PersistenceUseType.RequestSend);
        };

        Because of = () => Server.ReceiveMessage(payload);

        It should_push_the_message_to_any_registered_handlers = () => handler.LastHandledMessage.ShouldEqual(Message);

        It should_notify_that_the_channel_was_built = () => 
            channelBuiltHandler.ShouldMatch(m => 
                m.UseType == PersistenceUseType.RequestReceive
                && m.Address == BuildAddress(SenderAddress));

        It should_send_an_acknowledgement_for_the_message = () =>
            Server.SentMessages.ShouldContain(a => a.GetAcknowledgementId() == payload.GetSourcePersistenceId());

        It should_store_the_sender_address_for_the_reply_to_use = () =>
            Resolve<ReplyAddressLookup>().GetCurrentSenderAddress().ShouldEqual(BuildAddress(SenderAddress));
    }
}