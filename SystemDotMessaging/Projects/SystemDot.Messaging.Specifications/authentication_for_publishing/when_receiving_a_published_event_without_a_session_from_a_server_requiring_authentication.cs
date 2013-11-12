using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.authentication;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication_for_publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_published_event_without_a_session_from_a_server_requiring_authentication : WithHttpServerConfigurationSubject
    {
        const string SubscriberServer = "SubscriberServer";
        const string PublisherServer = "PublisherServer";
        const string SubscriberChannel = "SubscriberChannel";
        const string PublisherChannel = "PublisherChannel";

        static MessagePayload publishedEvent;
        static TestMessageHandler<long> handler;

        Establish context = () =>
        {
            handler = new TestMessageHandler<long>();

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(SubscriberServer)
                .AuthenticateToServer(PublisherServer)
                .WithRequest<TestAuthenticationRequest>()
                .OpenChannel(SubscriberChannel).ForSubscribingTo(PublisherChannel + "@" + PublisherServer)
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

            WebRequestor.AddMessages(new MessagePayload()
                .SetAuthenticationRequestChannels()
                .SetMessageBody(new TestAuthenticationResponse())
                .SetFromServer(PublisherServer)
                .SetToServer(SubscriberServer)
                .SetAuthenticationSession());

            Bus.SendDirect(new TestAuthenticationRequest(), new TestMessageHandler<TestAuthenticationResponse>(), e => { });
            WebRequestor.RequestsMade.Clear();

            publishedEvent = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel(PublisherChannel)
                .SetFromServer(PublisherServer)
                .SetToChannel(SubscriberChannel)
                .SetToServer(SubscriberServer)
                .SetChannelType(PersistenceUseType.SubscriberSend)
                .Sequenced();
        };

        Because of = () => SendMessageToServer(publishedEvent);

        It should_not_handle_the_message = () => handler.HandledMessages.ShouldBeEmpty();
    }
}