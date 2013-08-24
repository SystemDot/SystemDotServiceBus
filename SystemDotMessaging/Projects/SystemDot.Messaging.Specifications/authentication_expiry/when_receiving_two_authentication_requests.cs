using System;
using System.Linq;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.authentication;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication_expiry
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_two_authentication_requests : WithHttpServerConfigurationSubject
    {
        const string ReceiverServer = "ReceiverServer";
        const string SenderServer = "SenderServer";
        const string ReceiverChannel = "ReceiverChannel";
        const int GracePeriodInMinutes = 10;
        const int ExpiryInMinutes = 20;
        const int Message = 1;

        static MessagePayload authenticationRequestPayload;
        static TestMessageHandler<long> handler;
        static AuthenticationSession currentSession;
        static AuthenticationSession firstSession;

        Establish context = () =>
        {
            handler = new TestMessageHandler<long>();

            authenticationRequestPayload = new MessagePayload()
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
                            .Expires(ExpiryPlan.ExpiresAfter(TimeSpan.FromMinutes(ExpiryInMinutes)).WithGracePeriodOf(TimeSpan.FromMinutes(GracePeriodInMinutes)))
                    .OpenChannel(ReceiverChannel)
                        .ForPointToPointReceiving()
                .RegisterHandlers(r => r.RegisterHandler(new TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse>()))
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

            firstSession = SendMessagesToServer(authenticationRequestPayload).Single().GetAuthenticationSession();
            SystemTime.AdvanceTime(TimeSpan.FromMinutes(1));
        };

        Because of = () => currentSession = SendMessagesToServer(authenticationRequestPayload).Single().GetAuthenticationSession();

        It should_change_the_current_session = () => currentSession.ShouldNotEqual(firstSession);
    }
}