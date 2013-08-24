using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_without_first_authenticating : WithHttpServerConfigurationSubject
    {
        const string ReceiverServer = "ReceiverServer";
        const string ReceiverChannel = "ReceiverChannel";

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
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

            payload = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel("SenderChannel")
                .SetFromServer("SenderServer")
                .SetToChannel(ReceiverChannel)
                .SetToServer(ReceiverServer)
                .SetChannelType(PersistenceUseType.PointToPointSend)
                .Sequenced();
        };

        Because of = () => SendMessagesToServer(payload);

        It should_not_handle_the_message = () => handler.HandledMessages.Count.ShouldEqual(0);
    }
}