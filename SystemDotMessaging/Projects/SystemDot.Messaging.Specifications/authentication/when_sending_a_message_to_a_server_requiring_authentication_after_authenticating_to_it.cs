using System;
using SystemDot.Messaging.Packaging;
using Machine.Specifications;
using SystemDot.Messaging.Authentication;

namespace SystemDot.Messaging.Specifications.authentication
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_to_a_server_requiring_authentication_after_authenticating_to_it : WithHttpConfigurationSubject
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
                .MakeAuthenticationRequest<TestAuthenticationResponse>()
                .SetFromServer(ReceiverServerName)
                .SetToServer(SenderServer);

            authenticationResponse.SetAuthenticationSession(Guid.NewGuid());

            WebRequestor.AddMessages(authenticationResponse);

            Bus.SendDirect(new TestAuthenticationRequest(), new TestMessageHandler<TestAuthenticationResponse>(), e => { });

            WebRequestor.RequestsMade.Clear();
        };

        Because of = () => Bus.Send(1);

        It should_send_the_message_in_a_payload_containing_the_expected_authentication_session_for_the_server = () => 
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetAuthenticationSession().ShouldEqual(authenticationResponse.GetAuthenticationSession());
    }
}