using System.Linq;
using SystemDot.Messaging.Publishing;
using SystemDot.Messaging.Publishing.Builders;
using SystemDot.Messaging.RequestReply.Builders;
using SystemDot.Messaging.Simple;
using SystemDot.Messaging.Storage;
using FluentAssertions;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.publishing_subscription
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_a_subscriber_channel : WithMessageConfigurationSubject
    {
        const string SubscriberAddress = "SubscriberAddress";
        const string PublisherAddress = "PublisherAddress";
        
        static SubscriberReceiveChannelBuilt channelBuiltEvent;
        
        Because of = () =>
        {
            Messenger.RegisterHandler<SubscriberReceiveChannelBuilt>(e => channelBuiltEvent = e);

            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SubscriberAddress).ForSubscribingTo(PublisherAddress)
                .Initialise();
        };

        It should_notify_that_the_channel_was_built = () =>
            channelBuiltEvent.Should().Match<SubscriberReceiveChannelBuilt>(m =>
                m.CacheAddress == BuildAddress(SubscriberAddress)
                && m.SubscriberAddress == BuildAddress(SubscriberAddress)
                && m.PublisherAddress == BuildAddress(PublisherAddress));

        It should_mark_the_message_with_the_persistence_id = () =>
            GetServer().SentMessages.Single().GetPersistenceId()
                .ShouldBeEquivalentTo(new MessagePersistenceId(
                    GetServer().SentMessages.Single().Id,
                    BuildAddress(PublisherAddress),
                    PersistenceUseType.SubscriberRequestSend));

        It should_set_original_persistence_id_to_the_persistence_id_of_the_message_with_the_persistence_id = () =>
           GetServer().SentMessages
               .Single()
               .GetSourcePersistenceId()
               .ShouldBeEquivalentTo(GetServer().SentMessages.Single().GetPersistenceId());

        It should_send_a_request_for_non_persistent_subscriber_channel = () => 
            GetServer().SentMessages.Single().GetSubscriptionRequestSchema().IsDurable.Should().BeFalse();

        It should_send_a_request_for_a_subscriber_channel_with_the_correct_address = () =>
            GetServer().SentMessages.Single().GetSubscriptionRequestSchema()
                .SubscriberAddress.ShouldBeEquivalentTo(BuildAddress(SubscriberAddress));
    }
}