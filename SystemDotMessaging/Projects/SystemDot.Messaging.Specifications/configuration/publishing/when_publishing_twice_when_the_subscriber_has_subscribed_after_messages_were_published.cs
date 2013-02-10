using System.Linq;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_publishing_twice_when_the_subscriber_has_subscribed_after_messages_were_published : WithPublisherSubject
    {
        const string ChannelName = "Test";
        const string SubscriberName = "TestSubscriber";

        static IBus bus;
        static int message;

        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPublishing()
                .Initialise();

            message = 1;

            bus.Publish(message);

            Subscribe(BuildAddress(SubscriberName), BuildAddress(ChannelName));
        };

        Because of = () =>
        {
            bus.Publish(message);
            bus.Publish(message);
        };

        It should_mark_the_first_sequence_number_as_the_sequence_of_the_first_mesage_through_the_channel = () =>
            MessageServer.SentMessages.ExcludeAcknowledgements().Last().GetFirstSequence().ShouldEqual(2);

    }
}