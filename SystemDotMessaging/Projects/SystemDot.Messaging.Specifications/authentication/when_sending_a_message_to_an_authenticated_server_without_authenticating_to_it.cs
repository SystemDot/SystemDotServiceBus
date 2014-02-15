using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Packaging;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.authentication
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_to_an_authenticated_server_without_authenticating_to_it : WithHttpConfigurationSubject
    {
        const string ReceiverServerName = "ReceiverServer";
        const string SenderServer = "SenderServer";

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(SenderServer)
                .AuthenticateToServer(ReceiverServerName)
                .WithRequest<TestAuthenticationRequest>()
                .OpenChannel("SenderChannel").ForPointToPointSendingTo("ReceiverChannel@" + ReceiverServerName)
                .Initialise();

            WebRequestor.RequestsMade.Clear();
        };

        Because of = () => Bus.Send(1);

        It should_not_send_the_message = () => WebRequestor.RequestsMade.Should().BeEmpty();
    }
}