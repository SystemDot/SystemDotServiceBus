using SystemDot.Messaging.Packaging;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_with_both_sender_and_receiver_on_the_same_server
        : WithHttpServerConfigurationSubject
    {
        const string RecieverChannel = "Receiver";

        static TestMessageHandler<long> handler;

        Establish context = () =>
        {
            handler = new TestMessageHandler<long>();

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer("Server")
                .RequiresAuthentication()
                    .AcceptsRequest<TestAuthenticationRequest>()
                    .AuthenticatesOnReply<TestAuthenticationResponse>()
                .OpenChannel("Sender").ForPointToPointSendingTo(RecieverChannel)
                .OpenChannel(RecieverChannel).ForPointToPointReceiving()
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

            Bus.Send(1);
        };

        Because of = () => SendMessageToServer(WebRequestor.DeserialiseSingleRequest<MessagePayload>());

        It should_handle_the_event = () => handler.LastHandledMessage.ShouldEqual(1);
    }
}