using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_without_a_session_with_both_sender_and_receiver_on_the_same_server 
        : WithHttpServerConfigurationSubject
    {
        const string ServerName = "Server";
        const string SenderChannel = "Sender";
        const string RecieverChannel = "Receiver";

        static MessagePayload payload;
        static TestMessageHandler<long> handler;

        Establish context = () =>
        {
            handler = new TestMessageHandler<long>();

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(ServerName)
                .RequiresAuthentication()
                    .AcceptsRequest<TestAuthenticationRequest>()
                    .AuthenticatesOnReply<TestAuthenticationResponse>()
                .OpenChannel(SenderChannel).ForPointToPointSendingTo(RecieverChannel)
                .OpenChannel(RecieverChannel).ForPointToPointReceiving()
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

            payload = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel(SenderChannel)
                .SetFromServer(ServerName)
                .SetToChannel(RecieverChannel)
                .SetToServer(ServerName)
                .SetChannelType(PersistenceUseType.PointToPointSend)
                .Sequenced();
        };

        Because of = () => SendMessageToServer(payload);

        It should_not_handle_the_event = () => handler.HandledMessages.ShouldBeEmpty();
    }
}