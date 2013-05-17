using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Parallelism;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.expiry
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_that_is_time_expired_with_an_action_to_execute_on_expiry_set : WithMessageConfigurationSubject
    {
        static bool expiryActionExecuted;

        Establish context = () =>
        {
            var systemTime = new TestSystemTime(DateTime.Now);
            ConfigureAndRegister<ISystemTime>(systemTime);

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("ReceiverAddress")
                .ForPointToPointSendingTo("SenderAddress")
                .WithMessageExpiry(MessageExpiry.ByTime(TimeSpan.FromSeconds(3)), () => expiryActionExecuted = true)
                .Initialise();

            Bus.Send(1);

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(4));
            Server.SentMessages.Clear();
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_execute_the_expiry_action = () => expiryActionExecuted.ShouldBeTrue();

        It should_pass_the_message_through = () => Server.SentMessages.ShouldBeEmpty();
    }

    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_that_is_not_time_expired_with_an_action_to_execute_on_expiry_set : WithMessageConfigurationSubject
    {
        static bool expiryActionExecuted;

        Establish context = () => Configuration.Configure.Messaging()
            .UsingInProcessTransport()
            .OpenChannel("ReceiverAddress")
            .ForPointToPointSendingTo("SenderAddress")
            .WithMessageExpiry(MessageExpiry.ByTime(TimeSpan.FromSeconds(3)), () => expiryActionExecuted = true)
            .Initialise();

        Because of = () => Bus.Send(1);

        It should_not_execute_the_expiry_action = () => expiryActionExecuted.ShouldBeFalse();

        It should_pass_the_message_through = () => Server.SentMessages.ShouldNotBeEmpty();
    }
}