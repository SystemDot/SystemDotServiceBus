using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.authentication;
using SystemDot.Parallelism;
using Machine.Specifications;
using SystemDot.Messaging.Authentication;

namespace SystemDot.Messaging.Specifications.authentication_for_publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_a_subscription_attempt_to_a_server_has_been_repeated_after_authenticating_to_it : WithHttpConfigurationSubject
    {
        const string PublisherServer = "PublisherServer";
        const string SubscriberServer = "SubscriberServer";
        static MessagePayload authenticationResponse;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(SubscriberServer)
                    .AuthenticateToServer(PublisherServer)
                        .WithRequest<TestAuthenticationRequest>()
                    .OpenChannel("SubscriberChannel").ForSubscribingTo("PublisherChannel@" + PublisherServer)
                .Initialise();

            authenticationResponse = new MessagePayload()
                .SetAuthenticationRequestChannels()
                .SetMessageBody(new TestAuthenticationResponse())
                .SetFromServer(PublisherServer)
                .SetToServer(SubscriberServer)
                .SetAuthenticationSession();

            WebRequestor.AddMessages(authenticationResponse);

            Bus.SendDirect(new TestAuthenticationRequest(), new TestMessageHandler<TestAuthenticationResponse>(), e => { });
            WebRequestor.RequestsMade.Clear();
        };

        Because of = () =>
        {
            SystemTime.AdvanceTime(TimeSpan.FromSeconds(10));
            The<ITaskRepeater>().Start();
        };

        It should_send_the_subscription_in_a_payload_containing_the_expected_authentication_session_for_the_server = () => 
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetAuthenticationSession()
                    .Id.ShouldEqual(authenticationResponse.GetAuthenticationSession().Id);
    }
}