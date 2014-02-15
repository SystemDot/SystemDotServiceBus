using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.authentication;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.blocking_messages_for_authentication_channels
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_with_the_expected_session_in_block_mode : WithHttpServerConfigurationSubject
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

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(ReceiverServer)
                .RequiresAuthentication()
                .AcceptsRequest<TestAuthenticationRequest>()
                .AuthenticatesOnReply<TestAuthenticationResponse>()
                .BlockMessagesIf(true)
                .OpenChannel("ReceiverChannel")
                .ForPointToPointReceiving()
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();
        };

        Because of = () => SendMessageToServer(payload);

        It should_not_handle_the_message_in_the_registered_handler = () => handler.LastHandledMessage.Should().BeNull();
    }
}