using SystemDot.Messaging.Direct;
using SystemDot.Messaging.Packaging;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_an_authentication_request : WithHttpServerConfigurationSubject
    {
        const string ReceiverServer = "ReceiverServer";

        static MessagePayload payload;
        static TestMessageHandler<TestAuthenticationRequest> handler;

        Establish context = () =>
        {
            handler = new TestMessageHandler<TestAuthenticationRequest>();

            payload = new MessagePayload()
                .SetAuthenticationRequestChannels()
                .SetMessageBody(new TestAuthenticationRequest())
                .SetFromServer("SenderServer")
                .SetToServer(ReceiverServer);

            payload.SetIsDirectChannelMessage();

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(ReceiverServer)
                .RequiresAuthentication()
                    .AcceptsRequest<TestAuthenticationRequest>()
                    .AuthenticatesOnReply<TestAuthenticationResponse>()
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();
        };

        Because of = () => SendMessageToServer(payload);

        It should_handle_the_request_in_the_handler_configured = () => handler.LastHandledMessage.ShouldBeOfType<TestAuthenticationRequest>();
    }
}