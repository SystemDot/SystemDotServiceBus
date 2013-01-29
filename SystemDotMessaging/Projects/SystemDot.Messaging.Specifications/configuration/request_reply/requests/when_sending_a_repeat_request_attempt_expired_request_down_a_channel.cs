using SystemDot.Messaging.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.requests
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_repeat_request_attempt_expired_request_down_a_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string RecieverAddress = "TestRecieverAddress";
        static IBus bus;
        static int message;

        Establish context = () =>
            {
                bus = Configuration.Configure.Messaging()
                    .UsingInProcessTransport()
                    .OpenChannel(ChannelName)
                    .ForRequestReplySendingTo(RecieverAddress)
                    .WithMessageExpiry(MessageExpiry.ByRepeatAttempt(0))
                    .Initialise();

                message = 1;
            };

        Because of = () => bus.Send(message);

        It should_not_send_the_message = () => MessageSender.SentMessages.ShouldBeEmpty();
    }
}