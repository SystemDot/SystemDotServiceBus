using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Parallelism;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.expiry
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_that_is_time_expired_with_an_action_to_execute_on_expiry_set 
        : WithMessageConfigurationSubject
    {
        static bool expiryActionExecuted;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("ReceiverAddress")
                .ForPointToPointSendingTo("SenderAddress")
                .WithMessageExpiry(MessageExpiry.ByTime(TimeSpan.FromSeconds(3)), () => expiryActionExecuted = true)
                .Initialise();

            Bus.Send(1);

            SystemTime.AdvanceTime(TimeSpan.FromSeconds(4));
            GetServer().SentMessages.Clear();
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_execute_the_expiry_action = () => expiryActionExecuted.ShouldBeTrue();

        It should_pass_the_message_through = () => GetServer().SentMessages.ShouldBeEmpty();
    }
}