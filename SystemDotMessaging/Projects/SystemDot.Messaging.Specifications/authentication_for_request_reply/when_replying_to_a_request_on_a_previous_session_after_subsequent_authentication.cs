using System;
using System.Linq;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.authentication;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_replying_to_a_request_on_a_previous_session_after_subsequent_authentication : WithHttpServerConfigurationSubject
    {
        const string ReceiverServer = "ReceiverServer";
        const string SenderServer = "SenderServer";
        const string ReceiverChannel = "ReceiverChannel";
        const long Message = 1;

        static TestReplyMessageHandler<long> handler;
        static MessagePayload request;
        static AuthenticationSession session;

        Establish context = () =>
        {
            handler = new TestReplyMessageHandler<long>();

            var authenticationRequest = new MessagePayload()
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
                .OpenChannel(ReceiverChannel)
                .ForRequestReplyReceiving()
                .RegisterHandlers(r => r.RegisterHandler(new TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse>()))
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

            session = SendMessagesToServer(authenticationRequest).First().GetAuthenticationSession();
            
            SystemTime.AdvanceTime(TimeSpan.FromSeconds(1));
            
            SendMessagesToServer(authenticationRequest);

            request = new MessagePayload()
                .SetMessageBody(Message)
                .SetFromChannel("SenderChannel")
                .SetFromServer(SenderServer)
                .SetToChannel(ReceiverChannel)
                .SetToServer(ReceiverServer)
                .SetChannelType(PersistenceUseType.RequestSend)
                .Sequenced();

            request.SetAuthenticationSession(session);
        };

        Because of = () => SendMessagesToServer(request);

        It should_send_the_reply_in_a_payload_with_the_correct_authentication_session = () =>
            WebRequestor.RequestsMade.DeserialiseToPayloads().Last().GetAuthenticationSession().ShouldEqual(session);
    }
}