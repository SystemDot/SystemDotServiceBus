using System;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.expiry
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_that_is_not_time_expired : WithMessageConfigurationSubject
    {
        Establish context = () => Configuration.Configure.Messaging()
            .UsingInProcessTransport()
            .OpenChannel("ReceiverAddress")
            .ForPointToPointSendingTo("SenderAddress")
            .ExpireMessages().After(TimeSpan.FromSeconds(4))
            .Initialise();

        Because of = () => Bus.Send(1);

        It should_pass_the_message_through = () => GetServer().SentMessages.ShouldNotBeEmpty();
    }
}