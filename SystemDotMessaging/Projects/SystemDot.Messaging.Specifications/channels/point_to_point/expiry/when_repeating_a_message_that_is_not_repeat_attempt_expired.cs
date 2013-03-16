using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.point_to_point.expiry
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_that_is_not_repeat_attempt_expired : WithMessageConfigurationSubject
    {
        static IBus bus;

        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("ReceiverAddress")
                .ForPointToPointSendingTo("SenderAddress")
                .WithMessageExpiry(MessageExpiry.ByRepeatAttempt(1))
                .Initialise();
        };

        Because of = () => bus.Send(1);

        It should_pass_the_message_through = () => Server.SentMessages.ShouldNotBeEmpty();
    }
}