using System;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.repeating_escalating
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_two_messages_and_four_seconds_have_passed
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForPointToPointSendingTo(SenderChannelName)
                .Initialise();

            Bus.Send(1);
            Bus.Send(2);

            SystemTime.AdvanceTime(TimeSpan.FromSeconds(4));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_repeat_the_messages = () => GetServer().SentMessages.Count.ShouldEqual(4);
    }
}