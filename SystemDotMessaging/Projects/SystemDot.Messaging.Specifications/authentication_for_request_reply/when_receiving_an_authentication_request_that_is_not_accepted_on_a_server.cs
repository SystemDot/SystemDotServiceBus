using SystemDot.Messaging.Packaging;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_an_authentication_request_that_is_not_accepted_on_a_server : WithHttpServerConfigurationSubject
    {
        const string AuthenticationChannelName = "Authentication";
        const string ReceiverServer = "ReceiverServer";

        static MessagePayload payload;
        static TestMessageHandler<OtherTestAuthenticationRequest> handler;

        Establish context = () =>
        {
            handler = new TestMessageHandler<OtherTestAuthenticationRequest>();

            payload = new MessagePayload()
                .SetMessageBody(new OtherTestAuthenticationRequest())
                .SetToChannel(AuthenticationChannelName)
                .SetToServer(ReceiverServer);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(ReceiverServer)
                .RequiresAuthentication().AcceptsRequest<TestAuthenticationRequest>()
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();
        };

        Because of = () => SendMessagesToServer(payload);

        It should_not_handle_the_request_in_the_handler_configured = () => handler.HandledMessages.ShouldBeEmpty();
    }
}