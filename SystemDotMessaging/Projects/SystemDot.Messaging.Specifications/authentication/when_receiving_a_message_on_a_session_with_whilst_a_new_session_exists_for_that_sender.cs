using System;
using System.Linq;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_on_a_session_with_whilst_a_new_session_exists_for_that_sender : WithHttpServerConfigurationSubject
    {
        const string ReceiverServer = "ReceiverServer";
        const string SenderServer = "SenderServer";
        const string ReceiverChannel = "ReceiverChannel";
        const int ExpiryInMinutes = 20;
        const int Message = 1;

        static MessagePayload payload;
        static TestMessageHandler<long> handler;

        Establish context = () =>
        {
            handler = new TestMessageHandler<long>();

            var authenticationRequestPayload = new MessagePayload()
                .SetAuthenticationRequestChannels()
                .SetMessageBody(new TestAuthenticationRequest())
                .SetFromServer(SenderServer)
                .SetToServer(ReceiverServer);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                    .AsAServer(ReceiverServer)
                    .RequiresAuthentication()
                        .AcceptsRequest<TestAuthenticationRequest>()
                        .AuthenticatesOnReply<TestAuthenticationResponse>()
                    .ExpiresAfter(TimeSpan.FromMinutes(ExpiryInMinutes))
                .OpenChannel(ReceiverChannel)
                    .ForPointToPointReceiving()
                .RegisterHandlers(r => r.RegisterHandler(new TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse>()))
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

            AuthenticationSession firstSession = SendMessageToServer(authenticationRequestPayload).Single().GetAuthenticationSession();
            SendMessageToServer(authenticationRequestPayload);

            payload = new MessagePayload()
                .SetMessageBody(Message)
                .SetFromChannel("SenderChannel")
                .SetFromServer(SenderServer)
                .SetToChannel(ReceiverChannel)
                .SetToServer(ReceiverServer)
                .SetChannelType(PersistenceUseType.PointToPointSend)
                .Sequenced();

            payload.SetAuthenticationSession(firstSession);
        };

        Because of = () => SendMessageToServer(payload);

        It should_handle_the_message_in_the_registered_handler = () => handler.LastHandledMessage.ShouldEqual(Message);
    }
}