using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.authentication;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_request_to_a_server_after_failing_to_authenticate_to_it : WithHttpConfigurationSubject
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
                .OpenChannel("SenderChannel").ForRequestReplySendingTo("ReceiverChannel@" + ReceiverServerName)
                .Initialise();

            authenticationResponse = new MessagePayload()
                .SetAuthenticationRequestChannels()
                .SetMessageBody(new FailingAuthenticationResponse())
                .SetFromServer(ReceiverServerName)
                .SetToServer(SenderServer);
            
            WebRequestor.AddMessages(authenticationResponse);

            Bus.SendDirect(new TestAuthenticationRequest(), new TestMessageHandler<FailingAuthenticationResponse>(), e => { });

            WebRequestor.RequestsMade.Clear();
        };

        Because of = () => Bus.Send(1);

        It should_not_send_the_message = () => WebRequestor.RequestsMade.ShouldBeEmpty();
    }
}