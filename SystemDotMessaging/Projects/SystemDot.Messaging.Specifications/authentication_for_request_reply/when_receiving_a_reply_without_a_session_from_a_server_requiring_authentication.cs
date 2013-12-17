using System;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.authentication;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_reply_without_a_session_from_a_server_requiring_authentication : WithHttpServerConfigurationSubject
    {
        const string ReceiverServer = "ReceiverServer";
        const string SenderServer = "SenderServer";
        const string SenderChannel = "SenderChannel";
        const string ReceiverChannel = "ReceiverChannel";

        static MessagePayload response;
        static TestMessageHandler<long> handler;

        Establish context = () =>
        {
            handler = new TestMessageHandler<long>();

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(SenderServer)
                .AuthenticateToServer(ReceiverServer)
                .WithRequest<TestAuthenticationRequest>()
                .OpenChannel(SenderChannel)
                    .ForRequestReplySendingTo(ReceiverChannel + "@" + ReceiverServer)
                        .OnException().ContinueProcessingMessages()
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

            WebRequestor.AddMessages(new MessagePayload()
                .SetAuthenticationRequestChannels()
                .SetMessageBody(new TestAuthenticationResponse())
                .SetFromServer(ReceiverServer)
                .SetToServer(SenderServer)
                .SetAuthenticationSession());

            Bus.SendDirect(new TestAuthenticationRequest(), new TestMessageHandler<TestAuthenticationResponse>(), e => { });

            WebRequestor.RequestsMade.Clear();

            response = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel(ReceiverChannel)
                .SetFromServer(ReceiverServer)
                .SetToChannel(SenderChannel)
                .SetToServer(SenderServer)
                .SetChannelType(PersistenceUseType.ReplySend)
                .Sequenced();
        };

        Because of = () => SendMessageToServer(response);

        It should_not_handle_the_message = () => handler.HandledMessages.ShouldBeEmpty();
    }
}