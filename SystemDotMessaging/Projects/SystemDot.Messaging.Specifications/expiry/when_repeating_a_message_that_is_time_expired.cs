using System;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.expiry
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_that_is_time_expired : WithMessageConfigurationSubject
    {
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("ReceiverAddress")
                .ForPointToPointSendingTo("SenderAddress")
                .ExpireMessages().After(TimeSpan.FromSeconds(3))
                .Initialise();

            Bus.Send(1);

            SystemTime.AdvanceTime(TimeSpan.FromSeconds(4));
            GetServer().SentMessages.Clear();
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_pass_the_message_through = () => GetServer().SentMessages.ShouldBeEmpty();
    }
}