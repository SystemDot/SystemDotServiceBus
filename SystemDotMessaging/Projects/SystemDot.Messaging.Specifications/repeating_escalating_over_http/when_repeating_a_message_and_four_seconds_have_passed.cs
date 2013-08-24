using System;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.repeating_escalating_over_http
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_and_four_seconds_have_passed : WithHttpConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";
        const string ServerInstance = "ServerInstance";
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingHttpTransport().AsAServer(ServerInstance)
                .OpenChannel(ChannelName)
                .ForPointToPointSendingTo(SenderChannelName)
                .Initialise();

            Bus.Send(1);

            SystemTime.AdvanceTime(TimeSpan.FromSeconds(4));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_repeat_the_message = () => WebRequestor.RequestsMade.Count.ShouldEqual(2);
    }
}