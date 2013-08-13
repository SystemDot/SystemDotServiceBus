using SystemDot.Messaging.Packaging;
using Machine.Specifications;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Specifications.authentication_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_an_authentication_request_to_a_server_configured_to_require_authentication : WithHttpConfigurationSubject
    {
        const string AuthenticationChannelName = "Authentication";
        const string ReceiverServerName = "ReceiverServer";

        Establish context = () => 
            Configuration.Configure.Messaging()
            .UsingHttpTransport()
            .AsAServer("SenderServer")
                .AuthenticateToServer(ReceiverServerName).WithRequest<TestAuthenticationRequest>()
            .Initialise();

        Because of = () => Bus.SendDirect(new TestAuthenticationRequest());

        It should_send_the_request_in_a_payload = () => 
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().DeserialiseTo<TestAuthenticationRequest>().ShouldNotBeNull();
        
        It should_send_the_request_in_a_payload_with_the_correct_authentication_to_channel = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().GetToAddress().Channel.ShouldEqual(AuthenticationChannelName);

        It should_send_the_request_in_a_payload_with_the_correct_authentication_to_server = () =>
           WebRequestor.DeserialiseSingleRequest<MessagePayload>().GetToAddress().Server.Name.ShouldEqual(ReceiverServerName);

        It should_send_the_request_in_a_payload_with_the_correct_authentication_from_channel = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().GetFromAddress().Channel.ShouldEqual(AuthenticationChannelName);
    }
}