using SystemDot.Messaging.Specifications.filtering_by_name;
using SystemDot.Messaging.Specifications.publishing;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.filtering_by_name_for_publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_publishing_with_the_wrong_name_on_a_publish_subscribe_channel
        : WithPublisherSubject
    {
        const string PublisherChannel = "PublisherChannel";

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(PublisherChannel).ForPublishing()
                .OnlyForMessages().WithNamePattern("SomeOtherThing")
                .Initialise();

            Subscribe(BuildAddress("SubscriberChannel"), BuildAddress(PublisherChannel));
        };

        Because of = () => Bus.Publish(new TestNamePatternMessage());

        It should_not_pass_the_message_through = () => GetServer().SentMessages.ExcludeAcknowledgements().Should().BeEmpty();
    }
}