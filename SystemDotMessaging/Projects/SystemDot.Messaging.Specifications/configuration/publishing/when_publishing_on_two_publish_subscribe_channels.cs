using System.Linq;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_publishing_on_two_publish_subscribe_channels 
        : WithPublisherSubject
    {
        const string Channel1Name = "Test1";
        const string Subscriber1Name = "TestSubscriber1";
        const string Channel2Name = "Test2";
        const string Subscriber2Name = "TestSubscriber2";
        
        static IBus bus;
        static int message;
        
        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(Channel1Name).ForPublishing()
                .OpenChannel(Channel2Name).ForPublishing()
                .Initialise();

            message = 1;

            Subscribe(BuildAddress(Subscriber1Name), BuildAddress(Channel1Name));
            Subscribe(BuildAddress(Subscriber2Name), BuildAddress(Channel2Name));
        };

        Because of = () => bus.Publish(message);

        It should_publish_a_message_with_the_correct_content_through_both_channels = () =>
            MessageServer.SentMessages.ExcludeAcknowledgements()
                .Count(m => m.DeserialiseTo<int>() == message).ShouldEqual(2);
    }
}