using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.recieving
{
    [Subject("Request reply configuration")]
    public class when_replying_with_a_message_on_a_durable_request_reply_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";

        static IBus bus;
        static int message;

        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                
                .OpenChannel(ChannelName).ForRequestReplyRecieving().WithDurability()
                .Initialise();

            Resolve<ReplyAddressLookup>().SetCurrentRecieverAddress(BuildAddress(ChannelName));
            Resolve<ReplyAddressLookup>().SetCurrentSenderAddress(BuildAddress(SenderChannelName));

            message = 1;
        };

        Because of = () => bus.Reply(message);

        It should_persist_the_message = () =>
            Resolve<IPersistence>().GetMessages(BuildAddress(ChannelName)).ShouldNotBeEmpty();
    }
}