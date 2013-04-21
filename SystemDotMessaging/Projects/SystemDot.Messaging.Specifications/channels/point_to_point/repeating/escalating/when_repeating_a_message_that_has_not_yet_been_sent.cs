using System;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Parallelism;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.point_to_point.repeating.escalating
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_that_has_not_yet_been_sent : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";

        
        static int message;
        static TestSystemTime systemTime;

        Establish context = () =>
        {
            Register<IMessageSender, NonSentMarkingMessageSender>();

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
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_not_repeat_the_message = () => Server.SentMessages.Count.ShouldEqual(1);
    }
}