using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.authentication;
using SystemDot.Parallelism;
using SystemDot.Specifications;
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
        static TestSystemTime systemTime;

        Establish context = () =>
        {
            systemTime = new TestSystemTime(DateTime.Now);
            ConfigureAndRegister<ISystemTime>(systemTime);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(SubscriberServer)
                    .AuthenticateToServer(PublisherServer)
                        .WithRequest<TestAuthenticationRequest>()
                    .OpenChannel("SubscriberChannel").ForSubscribingTo("PublisherChannel@" + PublisherServer)
                .Initialise();

            authenticationResponse = new MessagePayload()
                .MakeAuthenticationResponse<TestAuthenticationResponse>()
                .SetFromServer(PublisherServer)
                .SetToServer(SubscriberServer);

            WebRequestor.AddMessages(authenticationResponse);

            Bus.SendDirect(new TestAuthenticationRequest(), new TestMessageHandler<TestAuthenticationResponse>(), e => { });
            WebRequestor.RequestsMade.Clear();
        };

        Because of = () =>
        {
            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(4));
            The<ITaskRepeater>().Start();
        };

        It should_send_the_subscription_in_a_payload_containing_the_expected_authentication_session_for_the_server = () => 
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetAuthenticationSession().Id.ShouldEqual(authenticationResponse.GetAuthenticationSession().Id);
    }
}