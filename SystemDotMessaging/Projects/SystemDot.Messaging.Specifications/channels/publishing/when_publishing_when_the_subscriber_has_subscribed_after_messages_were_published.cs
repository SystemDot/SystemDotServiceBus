using System.Linq;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_publishing_when_the_subscriber_has_subscribed_after_messages_were_published : WithPublisherSubject
    {
        const string ChannelName = "Test";
        const string SubscriberName = "TestSubscriber";

        
        static int message;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPublishing()
                .Initialise();

            message = 1;

            Bus.Publish(message);

            Subscribe(BuildAddress(SubscriberName), BuildAddress(ChannelName));
        };

        Because of = () => Bus.Publish(message);

        It should_mark_the_first_sequence_number_in_the_subscriber_as_two = () =>
            Server.SentMessages.ExcludeAcknowledgements().First().GetFirstSequence().ShouldEqual(2);

    }
}