using System;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.repeating_for_subscription_to_publishers
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_request_for_the_third_time_and_sixteen_seconds_have_not_passed
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string PublisherName = "PublisherName";
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForSubscribingTo(PublisherName)
                .Initialise();

            SystemTime.AdvanceTime(TimeSpan.FromSeconds(4));

            The<ITaskRepeater>().Start();

            SystemTime.AdvanceTime(TimeSpan.FromSeconds(8));

            The<ITaskRepeater>().Start();

            SystemTime.AdvanceTime(TimeSpan.FromSeconds(16).Subtract(TimeSpan.FromTicks(1)));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_repeat_the_message = () => GetServer().SentMessages.Count.ShouldEqual(3);
    }
}