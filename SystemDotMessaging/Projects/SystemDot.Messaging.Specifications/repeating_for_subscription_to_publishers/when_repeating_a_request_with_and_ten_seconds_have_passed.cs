using System;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.repeating_for_subscription_to_publishers
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_request_with_and_ten_seconds_have_passed : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string PublisherName = "PublisherName";

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForSubscribingTo(PublisherName)
                .Initialise();

            SystemTime.AdvanceTime(TimeSpan.FromSeconds(10));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_repeat_the_message = () => GetServer().SentMessages.ExcludeAcknowledgements().Count.ShouldEqual(2);
    }
}