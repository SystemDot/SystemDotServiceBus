using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_with_an_unauthenticated_server : WithHttpServerConfigurationSubject
    {
        const string ReceiverServer = "ReceiverServer";
        const string SenderServer = "SenderServer";
        const string ReceiverChannel = "ReceiverChannel";
        const long Message = 1;

        static MessagePayload payload;
        static TestMessageHandler<long> handler;

        Establish context = () =>
        {
            handler = new TestMessageHandler<long>();

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                    .AsAServer(ReceiverServer)
                    .RequiresAuthentication()
                        .AcceptsRequest<TestAuthenticationRequest>()
                        .AuthenticatesOnReply<TestAuthenticationResponse>()
                    .OpenChannel(ReceiverChannel)
                        .ForPointToPointReceiving()
                    .RegisterHandlers(r => r.RegisterHandler(new TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse>()))
                    .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

            SendMessagesToServer(new MessagePayload()
                .SetAuthenticationRequestChannels()
                .SetMessageBody(new TestAuthenticationRequest())
                .SetFromServer(SenderServer)
                .SetToServer(ReceiverServer));

            payload = new MessagePayload()
                .SetMessageBody(Message)
                .SetFromChannel("SenderChannel")
                .SetFromServer("OtherSenderServer")
                .SetToChannel(ReceiverChannel)
                .SetToServer(ReceiverServer)
                .SetChannelType(PersistenceUseType.PointToPointSend)
                .Sequenced();

            payload.SetAuthenticationSession();
        };

        Because of = () => SendMessagesToServer(payload);

        It should_not_handle_the_message_in_the_registered_handler = () => handler.HandledMessages.ShouldBeEmpty();
    }
}