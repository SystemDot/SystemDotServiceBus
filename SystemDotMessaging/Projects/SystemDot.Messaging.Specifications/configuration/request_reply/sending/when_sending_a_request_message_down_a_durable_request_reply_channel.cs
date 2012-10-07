using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.sending
{
    [Subject("Request reply configuration")]
    public class when_sending_a_request_message_down_a_durable_request_reply_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string RecieverAddress = "TestRecieverAddress";
        static IBus bus;
        static int message;

        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForRequestReplySendingTo(RecieverAddress).WithDurability()
                .Initialise();

            message = 1;
        };

        Because of = () => bus.Send(message);

        It should_persist_the_message = () =>
            Resolve<IPersistence>().GetMessages(BuildAddress(ChannelName)).ShouldNotBeEmpty();
    }
}