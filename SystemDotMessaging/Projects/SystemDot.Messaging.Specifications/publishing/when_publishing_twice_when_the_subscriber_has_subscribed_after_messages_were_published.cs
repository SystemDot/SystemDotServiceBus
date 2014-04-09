using System.Linq;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_publishing_twice_when_the_subscriber_has_subscribed_after_messages_were_published : WithPublisherSubject
    {
        const string ChannelName = "Test";
        const string SubscriberName = "TestSubscriber";

        
        static int message;

        Establish context = () =>
        {
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPublishing()
                .Initialise();

            message = 1;

            Bus.Publish(message);

            Subscribe(BuildAddress(SubscriberName), BuildAddress(ChannelName));
        };

        Because of = () =>
        {
            Bus.Publish(message);
            Bus.Publish(message);
        };

        It should_mark_the_first_sequence_number_as_the_sequence_of_the_first_mesage_through_the_channel = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().Last().GetFirstSequence().ShouldBeEquivalentTo(2);

    }
}