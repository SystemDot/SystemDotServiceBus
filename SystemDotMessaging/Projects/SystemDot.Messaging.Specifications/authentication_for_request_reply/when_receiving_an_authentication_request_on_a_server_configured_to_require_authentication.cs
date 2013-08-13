using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_an_authentication_request_on_a_server_configured_to_require_authentication : WithHttpServerConfigurationSubject
    {
        const string AuthenticationChannelName = "Authentication";
        const string ReceiverServer = "ReceiverServer";

        static MessagePayload payload;
        static TestMessageHandler<TestAuthenticationRequest> handler;

        Establish context = () =>
        {
            handler = new TestMessageHandler<TestAuthenticationRequest>();

            payload = new MessagePayload()
                .SetMessageBody(new TestAuthenticationRequest())
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

        It should_handle_the_request_in_the_handler_configured = () => handler.LastHandledMessage.ShouldBeOfType<TestAuthenticationRequest>();
    }
}