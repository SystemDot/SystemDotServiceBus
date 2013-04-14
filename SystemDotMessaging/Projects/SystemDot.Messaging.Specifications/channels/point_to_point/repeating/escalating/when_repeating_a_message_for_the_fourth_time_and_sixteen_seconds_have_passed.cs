using System;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Parallelism;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.point_to_point.repeating.escalating
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_for_the_fourth_time_and_sixteen_seconds_have_passed
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";

        static IBus bus;
        static int message;
        static TestSystemTime systemTime;

        Establish context = () =>
        {
            systemTime = new TestSystemTime(DateTime.Now);
            ConfigureAndRegister<ISystemTime>(systemTime);

            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForPointToPointSendingTo(SenderChannelName)
                .Initialise();

            message = 1;

            bus.Send(message);

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(4));
            The<ITaskRepeater>().Start();

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(8));
            The<ITaskRepeater>().Start();

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(16));
            The<ITaskRepeater>().Start();

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(16));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_repeat_the_message = () => Server.SentMessages.Count.ShouldEqual(5);
    }
}