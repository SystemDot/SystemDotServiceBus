using System;
using System.Linq;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.authentication;
using Machine.Specifications;
using SystemDot.Messaging.Authentication;

namespace SystemDot.Messaging.Specifications.authentication_for_publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_publishing_an_event_after_subscribing_with_the_expected_session_on_a_server_requiring_authentication : WithHttpServerConfigurationSubject
    {
        const string PublisherServer = "PublisherServer";
        const string SubscriberServer = "SubscriberServer";
        const string PublisherChannel = "PublisherChannel";

        static AuthenticationSession session;

        Establish context = () =>
        {
            var authenticationRequest = new MessagePayload()
                .SetAuthenticationRequestChannels()
                .SetMessageBody(new TestAuthenticationRequest())
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

            session = SendMessageToServer(authenticationRequest).First().GetAuthenticationSession();

            var subscriptionRequest = new MessagePayload()
                .SetFromChannel("SubscriberChannel")
                .SetFromServer(SubscriberServer)
                .SetToChannel(PublisherChannel)
                .SetToServer(PublisherServer)
                .SetSubscriptionRequest();

            subscriptionRequest.SetAuthenticationSession(session);

            SendMessageToServer(subscriptionRequest);
            WebRequestor.RequestsMade.Clear();
        };

        Because of = () => Bus.Publish(1);

        It should_publish_the_event_to_the_subscriber_along_with_the_session_in_the_payload_headers = () => 
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().GetAuthenticationSession().ShouldEqual(session);
    }
}