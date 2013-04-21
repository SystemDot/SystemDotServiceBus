using System;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Parallelism;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.request_reply.repeating
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_request_with_and_four_seconds_have_passed
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
                .ForRequestReplySendingTo(SenderChannelName)
                .Initialise();

            message = 1;

            Bus.Send(message);

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(4));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_repeat_the_message = () => Server.SentMessages.Count.ShouldEqual(2);
    }
}