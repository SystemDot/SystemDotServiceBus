using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.authentication;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication_for_publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_publishing_an_event_after_subscribing_without_a_session_on_a_server_requiring_authentication : WithHttpServerConfigurationSubject
    {
        const string PublisherServer = "PublisherServer";
        const string SubscriberServer = "SubscriberServer";
        const string PublisherChannel = "PublisherChannel";

        Establish context = () =>
        {
            var authenticationRequest = new MessagePayload()
                .MakeAuthenticationRequest<TestAuthenticationRequest>()
                .SetFromServer(SubscriberServer)
                .SetToServer(PublisherServer);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                    .AsAServer(PublisherServer)
                    .RequiresAuthentication()
                        .AcceptsRequest<TestAuthenticationRequest>()
                        .AuthenticatesOnReply<TestAuthenticationResponse>()
                    .OpenChannel(PublisherChannel)
                        .ForPublishing()
                    .RegisterHandlers(r => r.RegisterHandler(new TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse>()))
                .Initialise();

            SendMessagesToServer(authenticationRequest);

            var subscriptionRequest = new MessagePayload()
                .SetFromChannel("SubscriberChannel")
                .SetFromServer(SubscriberServer)
                .SetToChannel(PublisherChannel)
                .SetToServer(PublisherServer)
                .SetSubscriptionRequest();

            SendMessagesToServer(subscriptionRequest);
            WebRequestor.RequestsMade.Clear();
        };

        Because of = () => Bus.Publish(1);

        It should_not_publish_the_event_to_the_subscriber = () => WebRequestor.RequestsMade.ShouldBeEmpty();
    }
}