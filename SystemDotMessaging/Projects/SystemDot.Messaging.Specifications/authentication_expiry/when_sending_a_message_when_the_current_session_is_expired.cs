using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.authentication;
using Machine.Specifications;
using SystemDot.Messaging.Authentication;

namespace SystemDot.Messaging.Specifications.authentication_expiry
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_when_the_current_session_is_expired : WithHttpConfigurationSubject
    {
        const string ReceiverServerName = "ReceiverServer";
        const string SenderServer = "SenderServer";

        static MessagePayload authenticationResponse;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(SenderServer)
                    .AuthenticateToServer(ReceiverServerName)
                        .WithRequest<TestAuthenticationRequest>()
                    .OpenChannel("SenderChannel").ForPointToPointSendingTo("ReceiverChannel@" + ReceiverServerName)
                .Initialise();

            authenticationResponse = new MessagePayload()
                .SetAuthenticationRequestChannels()
                .SetMessageBody(new TestAuthenticationRequest())
                .SetFromServer(ReceiverServerName)
                .SetToServer(SenderServer)
                .SetAuthenticationSession();

            authenticationResponse.GetAuthenticationSession().ExpiresOn = SystemTime.GetCurrentDate().AddDays(-1);

            WebRequestor.AddMessages(authenticationResponse);

            Bus.SendDirect(new TestAuthenticationRequest(), new TestMessageHandler<TestAuthenticationResponse>(), e => { });

            WebRequestor.RequestsMade.Clear();
        };

        Because of = () => Bus.Send(1);

        It should_not_send_the_message = () => WebRequestor.RequestsMade.ShouldBeEmpty();
    }
}