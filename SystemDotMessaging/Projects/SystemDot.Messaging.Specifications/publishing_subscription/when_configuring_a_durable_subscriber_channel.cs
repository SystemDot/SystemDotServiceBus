using System.Linq;
using SystemDot.Messaging.Publishing;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.publishing_subscription
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_a_durable_subscriber_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "TestChannel";
        const string PublisherName = "TestPublisher";

        Because of = () => Messaging.Configuration.Configure.Messaging()
            .UsingInProcessTransport()
            .OpenChannel(ChannelName).ForSubscribingTo(PublisherName).WithDurability()
            .Initialise();

        It should_send_a_request_for_non_persistent_subscriber_channel = () =>
            GetServer().SentMessages.Single().GetSubscriptionRequestSchema().IsDurable.ShouldBeTrue();
    }
}