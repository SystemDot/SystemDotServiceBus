using System;
using SystemDot.Parallelism;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.repeating_escalating_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_request_for_the_third_time_and_sixteen_seconds_have_not_passed
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
                .Initialise();

            message = 1;

            Bus.Send(message);

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