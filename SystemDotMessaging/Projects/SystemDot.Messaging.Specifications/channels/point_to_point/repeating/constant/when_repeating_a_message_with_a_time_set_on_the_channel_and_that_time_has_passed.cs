using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Parallelism;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.point_to_point.repeating.constant
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_with_a_time_set_on_the_channel_and_that_time_has_passed 
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string ReceiverName = "TestReceiver";

        
        static int message;
        static TestSystemTime systemTime;

        Establish context = () =>
        {
            systemTime = new TestSystemTime(DateTime.Now);
            ConfigureAndRegister<ISystemTime>(systemTime);

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForPointToPointSendingTo(ReceiverName)
                    .WithMessageRepeating(RepeatMessages.Every(TimeSpan.FromSeconds(10)))
                .Initialise();

            message = 1;

            Bus.Send(message);

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(10));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_repeat_the_message = () => Server.SentMessages.Count.ShouldEqual(2);
    }


}