using System.Linq;
using SystemDot.Messaging.Channels.Publishing;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing.subscription
{
    [Subject(SpecificationGroup.Description)]
    public class when_initialising_a_durable_subscriber_channel : WithNoRepeaterMessageConfigurationSubject
    {
        const string ChannelName = "TestChannel";
        const string PublisherName = "TestPublisher";

        Because of = () => Configuration.Configure.Messaging()
            .UsingInProcessTransport()
            .OpenChannel(ChannelName).ForSubscribingTo(PublisherName).WithDurability()
            .Initialise();

        It should_send_a_request_for_non_persistent_subscriber_channel = () =>
            MessageSender.SentMessages.Single().GetSubscriptionRequestSchema().IsDurable.ShouldBeTrue();
    }
}