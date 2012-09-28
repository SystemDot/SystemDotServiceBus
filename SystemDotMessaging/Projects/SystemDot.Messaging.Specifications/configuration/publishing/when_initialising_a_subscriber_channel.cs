using System.Linq;
using SystemDot.Messaging.Channels.Publishing;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject("Publishing configuration")]
    public class when_initialising_a_subscriber_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "TestChannel";
        const string PublisherName = "TestPublisher";
        
        Because of = () => Configuration.Configure.Messaging()
            .UsingInProcessTransport()
            .OpenChannel(ChannelName).ForSubscribingTo(PublisherName)
            .Initialise();

        It should_send_a_request_for_non_persistent_subscriber_channel = () => 
            MessageSender.SentMessages.Single().GetSubscriptionRequestSchema().IsPersistent.ShouldBeFalse();

        It should_send_a_request_for_a_subscriber_channel_with_the_correct_address = () =>
            MessageSender.SentMessages.Single().GetSubscriptionRequestSchema().SubscriberAddress.ShouldEqual(BuildAddress(ChannelName));
    }
}