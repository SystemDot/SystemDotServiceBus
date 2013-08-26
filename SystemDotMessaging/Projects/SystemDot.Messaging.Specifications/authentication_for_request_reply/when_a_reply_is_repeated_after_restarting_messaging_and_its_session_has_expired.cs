using System;
using System.Linq;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.authentication;
using SystemDot.Messaging.Storage;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_a_reply_is_repeated_after_restarting_messaging_and_its_session_has_expired
        : WithHttpServerConfigurationSubject
    {
        const string ReceiverServer = "ReceiverServer";
        const string SenderServer = "SenderServer";
        const string ReceiverChannel = "ReceiverChannel";
        const long Message = 1;

        static TestReplyMessageHandler<long> handler;
        static IChangeStore changeStore;
        static AuthenticationSession session;

        Establish context = () =>
        {
            handler = new TestReplyMessageHandler<long>();

            changeStore = new InMemoryChangeStore(new JsonSerialiser());
            ConfigureAndRegister(changeStore);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(ReceiverServer)
                .RequiresAuthentication()
                .AcceptsRequest<TestAuthenticationRequest>()
                .AuthenticatesOnReply<TestAuthenticationResponse>()
                .ExpiresAfter(TimeSpan.FromHours(1))
                .OpenChannel(ReceiverChannel).ForRequestReplyReceiving().WithDurability()
                .RegisterHandlers(r => r.RegisterHandler(new TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse>()))
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

            var authenticationRequest = new MessagePayload()
                .SetAuthenticationRequestChannels()
                .SetMessageBody(new TestAuthenticationRequest())
                .SetFromServer(SenderServer)
                .SetToServer(ReceiverServer);

            session = SendMessagesToServer(authenticationRequest).First().GetAuthenticationSession();

            var request = new MessagePayload()
                .SetMessageBody(Message)
                .SetFromChannel("SenderChannel")
                .SetFromServer(SenderServer)
                .SetToChannel(ReceiverChannel)
                .SetToServer(ReceiverServer)
                .SetChannelType(PersistenceUseType.RequestSend)
                .Sequenced();

            request.SetAuthenticationSession(session);
            SendMessagesToServer(request);

            Reset();
            ReInitialise();

            ConfigureAndRegister(changeStore);
            SystemTime.AdvanceTime(TimeSpan.FromDays(1));

            WebRequestor.RequestsMade.Clear();
        };

        Because of = () =>
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(ReceiverServer)
                .RequiresAuthentication()
                .AcceptsRequest<TestAuthenticationRequest>()
                .AuthenticatesOnReply<TestAuthenticationResponse>()
                .ExpiresAfter(TimeSpan.FromHours(1))
                .OpenChannel(ReceiverChannel).ForRequestReplyReceiving().WithDurability()
                .RegisterHandlers(r => r.RegisterHandler(new TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse>()))
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

        It should_not_send_the_reply = () => WebRequestor.RequestsMade.ShouldBeEmpty();
    }
}