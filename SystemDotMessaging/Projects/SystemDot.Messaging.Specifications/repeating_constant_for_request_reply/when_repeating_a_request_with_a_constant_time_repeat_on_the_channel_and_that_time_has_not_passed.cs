using System;
using SystemDot.Parallelism;
using FluentAssertions;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.repeating_constant_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_request_with_a_constant_time_repeat_on_the_channel_and_that_time_has_not_passed
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";
        static int message;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplySendingTo(SenderChannelName)
                .RepeatMessages().Every(TimeSpan.FromSeconds(10))
                .Initialise();

            message = 1;

            Bus.Send(message);

            SystemTime.AdvanceTime(TimeSpan.FromSeconds(10).Subtract(TimeSpan.FromTicks(1)));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_not_repeat_the_message = () => GetServer().SentMessages.Count.ShouldBeEquivalentTo(1);
    }
}