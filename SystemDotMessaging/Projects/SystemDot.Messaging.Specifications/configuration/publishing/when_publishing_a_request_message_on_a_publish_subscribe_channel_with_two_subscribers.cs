using System.Linq;
using SystemDot.Messaging.Channels.Packaging.Headers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject("Publishing configuration")]
    public class when_publishing_a_request_message_on_a_publish_subscribe_channel_with_two_subscribers
        : WithPublisherSubject
    {
        const string ChannelName = "Test";
        const string SubscriberName1 = "TestSubscriber1";
        const string SubscriberName2 = "TestSubscriber2";

        static IBus bus;
        static int message;

        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPublishing()
                .Initialise();

            message = 1;
            Subscribe(BuildAddress(SubscriberName1), BuildAddress(ChannelName));
            Subscribe(BuildAddress(SubscriberName2), BuildAddress(ChannelName));
        };

        Because of = () => bus.Publish(message);

        It should_use_a_different_copy_of_the_message_payload_on_each_subscriber = () => 
            MessageSender.SentMessages
                .ExcludeAcknowledgements()
                .First()
                .Headers.OfType<AddressHeader>()
                .Count()
                .ShouldEqual(2);
    }
}