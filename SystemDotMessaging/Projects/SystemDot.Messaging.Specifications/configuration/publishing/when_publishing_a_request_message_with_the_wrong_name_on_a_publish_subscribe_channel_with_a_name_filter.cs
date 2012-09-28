using SystemDot.Messaging.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject("Publishing configuration")]
    public class when_publishing_a_request_message_with_the_wrong_name_on_a_publish_subscribe_channel_with_a_name_filter 
        : WithPublisherSubject
    {
        const string ChannelName = "Test";
        const string SubscriberName = "TestSubscriber";
        static IBus bus;
        
        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPublishing()
                .OnlyForMessages(FilteredBy.NamePattern("SomeOtherThing"))
                .Initialise();

            Subscribe(BuildAddress(SubscriberName), BuildAddress(ChannelName));
        };

        Because of = () => bus.Publish(new TestNamePatternMessage());

        It should_not_pass_the_message_through = () => MessageSender.SentMessages.ExcludeAcknowledgements().ShouldBeEmpty();
    }
}