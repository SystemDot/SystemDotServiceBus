using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Packaging;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_to_a_server_requiring_authentication_after_failing_to_authenticate_to_it : WithHttpConfigurationSubject
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
                .MakeAuthenticationRequest<FailingAuthenticationResponse>()
                .SetFromServer(ReceiverServerName)
                .SetFromMachine(null)
                .SetToServer(SenderServer);
            
            WebRequestor.AddMessages(authenticationResponse);

            Bus.SendDirect(new TestAuthenticationRequest(), new TestMessageHandler<FailingAuthenticationResponse>(), e => { });

            WebRequestor.RequestsMade.Clear();
        };

        Because of = () => Bus.Send(1);

        It should_send_the_message_in_a_payload_with_no_authentication_session_for_the_server = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().HasAuthenticationSession().ShouldBeFalse();
    }
}