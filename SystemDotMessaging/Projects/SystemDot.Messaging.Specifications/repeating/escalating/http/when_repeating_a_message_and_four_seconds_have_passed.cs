using System;
using SystemDot.Http;
using SystemDot.Http.Builders;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.repeating.escalating.http
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_and_four_seconds_have_passed : WithHttpConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";
        const string ServerInstance = "ServerInstance";
        
        Establish context = () =>
        {
            WebRequestor.ExpectAddress(ServerInstance, Environment.MachineName);

            var systemTime = new TestSystemTime(DateTime.Now);
            ConfigureAndRegister<ISystemTime>(systemTime);

            Messaging.Configuration.Configure.Messaging()
                .UsingHttpTransport().AsAServer(ServerInstance)
                .OpenChannel(ChannelName)
                .ForPointToPointSendingTo(SenderChannelName)
                .Initialise();

            Bus.Send(1);

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(4));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_repeat_the_message = () => WebRequestor.RequestsMade.Count.ShouldEqual(2);
    }
}