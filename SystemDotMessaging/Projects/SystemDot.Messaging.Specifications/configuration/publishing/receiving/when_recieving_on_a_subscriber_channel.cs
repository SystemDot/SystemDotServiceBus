using System;
using System.Linq;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Storage.Changes;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.configuration.publishing.receiving
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
            payload = CreateReceiveablePayload(message, PublisherName, ChannelName, PersistenceUseType.SubscriberSend);
            originalPersistenceId = payload.GetPersistenceId();
            payload.SetSequence(1); 
            payload.SetFirstSequence(1);
        };

        Because of = () => MessageReciever.RecieveMessage(payload);

        It should_push_the_message_to_any_registered_handlers = () => handler.HandledMessage.ShouldEqual(message);

        It should_send_an_acknowledgement_for_the_message = () =>
            MessageSender.SentMessages.ExcludeSubscriptionRequests().ShouldContain(a => a.GetAcknowledgementId() == originalPersistenceId);

        It should_mark_the_message_with_the_time_the_message_is_sent = () =>
            Resolve<InMemoryChangeStore>()
                .GetAddedMessages(PersistenceUseType.SubscriberReceive, BuildAddress(ChannelName))
                .First()
                .GetLastTimeSent().ShouldBeGreaterThan(DateTime.MinValue);

        It should_mark_the_message_with_the_amount_of_times_the_message_has_been_sent = () =>
           Resolve<InMemoryChangeStore>()
                .GetAddedMessages(PersistenceUseType.SubscriberReceive, BuildAddress(ChannelName))
                .First()
                .GetAmountSent().ShouldEqual(1);
    }
}