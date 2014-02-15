using System;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.authentication;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.authentication_expiry
{
    [Subject(SpecificationGroup.Description)]
    public class when_a_session_expires_with_an_expired_session_action_configured_for_different_server : WithHttpConfigurationSubject
    {
        const string ReceiverServerName = "ReceiverServer";
        const string SenderServer = "SenderServer";
        const int ExpiryInMinutes = 20;

        static AuthenticationSession session;
        static AuthenticationSession sessionReceivedInExpiryAction;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                    .AsAServer(SenderServer)
                        .AuthenticateToServer(ReceiverServerName)
                            .WithRequest<TestAuthenticationRequest>()
                        .AuthenticateToServer("OtherReceiverServer")
                            .WithRequest<TestAuthenticationRequest>()
                            .OnExpiry(s => sessionReceivedInExpiryAction = s)
                    .OpenChannel("OtherSenderChannel").ForPointToPointSendingTo("OtherReceiverChannel@OtherReceiverServer")
                .Initialise();

            var authenticationResponse = new MessagePayload()
                .SetAuthenticationRequestChannels()
                .SetMessageBody(new TestAuthenticationRequest())
                .SetFromServer(ReceiverServerName)
                .SetToServer(SenderServer)
                .SetAuthenticationSession();

            session = authenticationResponse.GetAuthenticationSession();
            session.ExpiresAfter = TimeSpan.FromSeconds(ExpiryInMinutes);

            WebRequestor.AddMessages(authenticationResponse);

            Bus.SendDirect(new TestAuthenticationRequest(), new TestMessageHandler<TestAuthenticationResponse>(), e => { });

            WebRequestor.RequestsMade.Clear();
        };

        Because of = () =>
        {
            SystemTime.AdvanceTime(TimeSpan.FromMinutes(ExpiryInMinutes));
            SystemTime.AdvanceTime(TimeSpan.FromTicks(1));
        };

        It should_not_run_the_action = () => sessionReceivedInExpiryAction.Should().BeNull();
    }
}