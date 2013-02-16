using System;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Parallelism;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.repeating
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_and_four_seconds_have_not_passed
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

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(4).Subtract(TimeSpan.FromTicks(1)));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_not_repeat_the_message = () => Server.SentMessages.Count.ShouldEqual(1);
    }
}