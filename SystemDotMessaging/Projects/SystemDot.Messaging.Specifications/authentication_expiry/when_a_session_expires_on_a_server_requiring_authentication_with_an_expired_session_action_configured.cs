using System;
using System.Linq;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.authentication;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.authentication_expiry
{
    [Subject(SpecificationGroup.Description)]
    public class when_a_session_expires_on_a_server_requiring_authentication_with_an_expired_session_action_configured : WithHttpServerConfigurationSubject
    {
        const string ReceiverServer = "ReceiverServer";
        const int ExpiryInMinutes = 20;

        static AuthenticationSession session;
        static AuthenticationSession sessionReceivedInExpiryAction;

        Establish context = () =>
        {
            var authenticationRequestPayload = new MessagePayload()
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
                        .ExpiresAfter(TimeSpan.FromMinutes(ExpiryInMinutes))
                        .OnExpiry(s => sessionReceivedInExpiryAction = s)
                    .OpenChannel("ReceiverChannel")
                        .ForPointToPointReceiving()
                    .RegisterHandlers(r => r.RegisterHandler(new TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse>()))
                .Initialise();

            session = SendMessageToServer(authenticationRequestPayload).Single().GetAuthenticationSession();
            
            SystemTime.AdvanceTime(TimeSpan.FromMinutes(ExpiryInMinutes));
        };

        Because of = () => SystemTime.AdvanceTime(TimeSpan.FromTicks(1));

        It should_run_the_action = () => sessionReceivedInExpiryAction.Should().Be(session);
    }


}