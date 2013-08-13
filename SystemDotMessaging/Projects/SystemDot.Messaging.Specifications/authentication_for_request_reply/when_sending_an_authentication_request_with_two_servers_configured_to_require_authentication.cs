using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_an_authentication_request_with_two_servers_configured_to_require_authentication : WithHttpConfigurationSubject
    {
        const string OtherReceiverServerName = "OtherReceiverServerName";

        Establish context = () =>
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer("SenderServer")
                .AuthenticateToServer(OtherReceiverServerName).WithRequest<OtherTestAuthenticationRequest>()
                .AuthenticateToServer("ReceiverServer").WithRequest<TestAuthenticationRequest>()
                .Initialise();

        Because of = () => Bus.SendDirect(new OtherTestAuthenticationRequest());

        It should_only_send_the_request_down_the_authentication_channel_for_which_it_is_associated = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().GetToAddress().Server.Name.ShouldEqual(OtherReceiverServerName);
    }
}