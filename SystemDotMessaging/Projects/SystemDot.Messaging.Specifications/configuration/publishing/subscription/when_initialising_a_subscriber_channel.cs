using System.Linq;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing.subscription
{
    [Subject(SpecificationGroup.Description)]
    public class when_initialising_a_subscriber_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "TestChannel";
        const string PublisherName = "TestPublisher";
        
        Because of = () => Configuration.Configure.Messaging()
            .UsingInProcessTransport()
            .OpenChannel(ChannelName).ForSubscribingTo(PublisherName)
            .Initialise();

        It should_mark_the_message_with_the_persistence_id = () =>
            MessageSender.SentMessages.Single().GetPersistenceId()
                .ShouldEqual(new MessagePersistenceId(
                    MessageSender.SentMessages.Single().Id,
                    BuildAddress(PublisherName),
                    PersistenceUseType.SubscriberRequestSend));

        It should_set_original_persistence_id_to_the_persistence_id_of_the_message_with_the_persistence_id = () =>
           MessageSender.SentMessages
               .Single()
               .GetSourcePersistenceId()
               .ShouldEqual(MessageSender.SentMessages.Single().GetPersistenceId());

        It should_send_a_request_for_non_persistent_subscriber_channel = () => 
            MessageSender.SentMessages.Single().GetSubscriptionRequestSchema().IsDurable.ShouldBeFalse();

        It should_send_a_request_for_a_subscriber_channel_with_the_correct_address = () =>
            MessageSender.SentMessages.Single().GetSubscriptionRequestSchema().SubscriberAddress.ShouldEqual(BuildAddress(ChannelName));
    }
}