using System;
using System.Linq;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Storage.Changes;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.publishing.receiving
{
    [Subject(SpecificationGroup.Description)]
    public class when_recieving_on_a_subscriber_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "TestChannel";
        const string PublisherName = "TestPublisher";
        static int message;
        static MessagePayload payload;
        static TestMessageHandler<int> handler;
        static MessagePersistenceId originalPersistenceId;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForSubscribingTo(PublisherName)
                .Initialise();

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            message = 1;
            payload = new MessagePayload().MakeSequencedReceivable(message, PublisherName, ChannelName, PersistenceUseType.SubscriberSend);
            originalPersistenceId = payload.GetPersistenceId();
        };

        Because of = () => Server.ReceiveMessage(payload);

        It should_push_the_message_to_any_registered_handlers = () => handler.LastHandledMessage.ShouldEqual(message);

        It should_send_an_acknowledgement_for_the_message = () =>
            Server.SentMessages.ExcludeSubscriptionRequests().ShouldContain(a => a.GetAcknowledgementId() == originalPersistenceId);
    }
}