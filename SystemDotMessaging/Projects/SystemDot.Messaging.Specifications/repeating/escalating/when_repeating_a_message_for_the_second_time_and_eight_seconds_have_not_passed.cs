using System;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Parallelism;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.repeating.escalating
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_for_the_second_time_and_eight_seconds_have_not_passed
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";

        
        static int message;
        static TestSystemTime systemTime;

        Establish context = () =>
        {
            systemTime = new TestSystemTime(DateTime.Now);
            ConfigureAndRegister<ISystemTime>(systemTime);

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForPointToPointSendingTo(SenderChannelName)
                .Initialise();

            message = 1;

            Bus.Send(message);

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(4));

            The<ITaskRepeater>().Start();

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(8).Subtract(TimeSpan.FromTicks(1)));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_not_repeat_the_message = () => Server.SentMessages.Count.ShouldEqual(2);
    }
}