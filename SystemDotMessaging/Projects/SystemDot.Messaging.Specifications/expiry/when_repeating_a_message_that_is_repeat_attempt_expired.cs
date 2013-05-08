using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Parallelism;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.expiry
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_that_is_repeat_attempt_expired : WithMessageConfigurationSubject
    {
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("ReceiverAddress")
                    .ForPointToPointSendingTo("SenderAddress")
                    .WithMessageExpiry(MessageExpiry.ByRepeatAttempt(1))
                .Initialise();

            Bus.Send(1);

            Server.SentMessages.Clear();
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_pass_the_message_through = () => Server.SentMessages.ShouldBeEmpty();
    }
}