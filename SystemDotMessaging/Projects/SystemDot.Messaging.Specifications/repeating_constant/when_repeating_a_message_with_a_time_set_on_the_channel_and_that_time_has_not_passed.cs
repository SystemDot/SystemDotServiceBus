using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.repeating_constant
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_with_a_time_set_on_the_channel_and_that_time_has_not_passed
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string ReceiverName = "TestReceiver";

        
        static int message;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForPointToPointSendingTo(ReceiverName)
                .RepeatMessages().Every(TimeSpan.FromSeconds(10))
                .Initialise();

            message = 1;

            Bus.Send(message);

            SystemTime.AdvanceTime(TimeSpan.FromSeconds(10).Subtract(TimeSpan.FromTicks(1)));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_not_repeat_the_message = () => GetServer().SentMessages.Count.ShouldEqual(1);
    }
}