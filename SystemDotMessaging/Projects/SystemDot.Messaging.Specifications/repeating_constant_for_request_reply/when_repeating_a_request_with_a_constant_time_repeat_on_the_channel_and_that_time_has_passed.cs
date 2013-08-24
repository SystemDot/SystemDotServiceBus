using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Parallelism;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.repeating_constant_for_request_reply      
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_request_with_a_constant_time_repeat_on_the_channel_and_that_time_has_passed 
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
                    .ForPointToPointSendingTo(SenderChannelName)
                    .WithMessageRepeating(RepeatMessages.Every(TimeSpan.FromSeconds(10)))
                .Initialise();

            message = 1;

            Bus.Send(message);

            SystemTime.AdvanceTime(TimeSpan.FromSeconds(10));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_repeat_the_message = () => GetServer().SentMessages.Count.ShouldEqual(2);
    }
}