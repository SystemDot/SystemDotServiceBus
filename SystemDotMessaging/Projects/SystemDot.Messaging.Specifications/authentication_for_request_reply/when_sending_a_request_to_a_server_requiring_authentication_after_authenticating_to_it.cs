using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.authentication;
using Machine.Specifications;
using SystemDot.Messaging.Authentication;

namespace SystemDot.Messaging.Specifications.authentication_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_request_to_a_server_requiring_authentication_after_authenticating_to_it : WithHttpConfigurationSubject
    {
        const string ReceiverServer = "ReceiverServer";
        const string SenderServer = "SenderServer";

        static MessagePayload authenticationResponse;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(SenderServer)
                    .AuthenticateToServer(ReceiverServer)
                        .WithRequest<TestAuthenticationRequest>()
                    .OpenChannel("SenderChannel").ForRequestReplySendingTo("ReceiverChannel@" + ReceiverServer)
                .Initialise();

            authenticationResponse = new MessagePayload()
                .SetAuthenticationRequestChannels()
                .SetMessageBody(new TestAuthenticationResponse())
                .SetFromServer(ReceiverServer)
                .SetToServer(SenderServer)
                .SetAuthenticationSession();

            WebRequestor.AddMessages(authenticationResponse);

            Bus.SendDirect(new TestAuthenticationRequest(), new TestMessageHandler<TestAuthenticationResponse>(), e => { });

            WebRequestor.RequestsMade.Clear();
        };

        Because of = () => Bus.Send(1);

        It should_send_the_message_in_a_payload_containing_the_expected_authentication_session_for_the_server = () => 
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetAuthenticationSession().Id.ShouldEqual(authenticationResponse.GetAuthenticationSession().Id);
    }
}