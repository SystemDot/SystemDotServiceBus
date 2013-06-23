using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Specifications.filtering;
using SystemDot.Messaging.Specifications.publishing;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.filtering_for_publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_publishing_with_the_correct_name_on_a_publish_subscribe_channel_with_a_name_filter 
        : WithPublisherSubject
    {
        const string ChannelName = "Test";
        const string SubscriberName = "TestSubscriber";
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPublishing()
                .OnlyForMessages(FilteredBy.NamePattern("Name"))
                .Initialise();

            Subscribe(BuildAddress(SubscriberName), BuildAddress(ChannelName));
        };

        Because of = () => Bus.Publish(new TestNamePatternMessage());

        It should_pass_the_message_through = () => Server.SentMessages.ExcludeAcknowledgements().ShouldNotBeEmpty();
    }
}