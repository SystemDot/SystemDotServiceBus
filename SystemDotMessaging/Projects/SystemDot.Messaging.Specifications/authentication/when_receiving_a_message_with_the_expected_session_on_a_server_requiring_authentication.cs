using System;
using System.Linq;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_with_the_expected_session_on_a_server_requiring_authentication : WithHttpServerConfigurationSubject
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

            var authenticationRequestPayload = new MessagePayload()
                .MakeAuthenticationRequest<TestAuthenticationRequest>()
                .SetFromServer(SenderServer)
                .SetToServer(ReceiverServer);

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

            Guid authenticationSession = SendMessagesToServer(authenticationRequestPayload).Single().GetAuthenticationSession();

            payload = new MessagePayload()
                .SetMessageBody(Message)
                .SetFromChannel("SenderChannel")
                .SetFromServer(SenderServer)
                .SetToChannel(ReceiverChannel)
                .SetToServer(ReceiverServer)
                .SetChannelType(PersistenceUseType.PointToPointSend)
                .Sequenced();

            payload.SetAuthenticationSession(authenticationSession);
        };

        Because of = () => SendMessagesToServer(payload);

        It should_handle_the_message_in_the_registered_handler = () => handler.LastHandledMessage.ShouldEqual(Message);
    }
}