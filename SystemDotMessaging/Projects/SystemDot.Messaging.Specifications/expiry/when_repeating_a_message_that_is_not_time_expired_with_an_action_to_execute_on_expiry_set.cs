using System;
using SystemDot.Messaging.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.expiry
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_that_is_not_time_expired_with_an_action_to_execute_on_expiry_set 
        : WithMessageConfigurationSubject
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