using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.expiry
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_that_is_repeat_attempt_expired : WithMessageConfigurationSubject
    {
        Establish context = () =>
        {
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("ReceiverAddress")
                    .ForPointToPointSendingTo("SenderAddress")
                    .ExpireMessages().AfterRepeatAttempts(1)
                .Initialise();

            Bus.Send(1);

            GetServer().SentMessages.Clear();
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_pass_the_message_through = () => GetServer().SentMessages.ShouldBeEmpty();
    }
}