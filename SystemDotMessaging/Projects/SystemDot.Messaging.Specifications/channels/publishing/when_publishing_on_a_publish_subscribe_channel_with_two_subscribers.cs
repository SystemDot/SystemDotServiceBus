using System.Linq;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_publishing_on_a_publish_subscribe_channel_with_two_subscribers
        : WithPublisherSubject
    {
        const string ChannelName = "Test";
        const string SubscriberName1 = "TestSubscriber1";
        const string SubscriberName2 = "TestSubscriber2";

        
        static int message;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPublishing()
                .Initialise();

            message = 1;
            Subscribe(BuildAddress(SubscriberName1), BuildAddress(ChannelName));
            Subscribe(BuildAddress(SubscriberName2), BuildAddress(ChannelName));
        };

        Because of = () => Bus.Publish(message);

        It should_use_a_different_copy_of_the_message_payload_on_each_subscriber = () => 
            Server.SentMessages
                .ExcludeAcknowledgements()
                .First()
                .Headers.Values.OfType<AddressHeader>()
                .Count()
                .ShouldEqual(2);
    }
}