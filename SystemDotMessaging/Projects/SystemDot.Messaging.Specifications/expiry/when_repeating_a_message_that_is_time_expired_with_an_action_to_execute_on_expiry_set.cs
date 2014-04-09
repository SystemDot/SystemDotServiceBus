using System;
using SystemDot.Parallelism;
using FluentAssertions;
using Machine.Specifications;using FluentAssertions;

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
                .ExpireMessages().After(TimeSpan.FromSeconds(3))
                .OnMessagingExpiry(() => expiryActionExecuted = true)
                .Initialise();

            Bus.Send(1);

            SystemTime.AdvanceTime(TimeSpan.FromSeconds(4));
            GetServer().SentMessages.Clear();
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_execute_the_expiry_action = () => expiryActionExecuted.Should().BeTrue();

        It should_pass_the_message_through = () => GetServer().SentMessages.Should().BeEmpty();
    }
}