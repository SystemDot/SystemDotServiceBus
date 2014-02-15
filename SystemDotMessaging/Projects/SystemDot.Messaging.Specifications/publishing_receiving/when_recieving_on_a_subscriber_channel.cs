using System;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.publishing;
using Machine.Specifications;using FluentAssertions;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.publishing_receiving
{
    [Subject(SpecificationGroup.Description)]
    public class when_recieving_on_a_subscriber_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "TestChannel";
        const string PublisherName = "TestPublisher";
        static Int64 message;
        static MessagePayload payload;
        static TestMessageHandler<Int64> handler;
        static MessagePersistenceId originalPersistenceId;

        Establish context = () =>
        {
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForSubscribingTo(PublisherName)
                .Initialise();

            handler = new TestMessageHandler<Int64>();
            Resolve<MessageHandlingEndpoint>().RegisterHandler(handler);

            message = 1;
            payload = new MessagePayload().MakeSequencedReceivable(message, PublisherName, ChannelName, PersistenceUseType.SubscriberSend);
            originalPersistenceId = payload.GetPersistenceId();
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_push_the_message_to_any_registered_handlers = () => handler.LastHandledMessage.ShouldBeEquivalentTo(message);

        It should_send_an_acknowledgement_for_the_message = () =>
            GetServer().SentMessages.ExcludeSubscriptionRequests().Should().Contain(a => a.GetAcknowledgementId() == originalPersistenceId);
    }
}