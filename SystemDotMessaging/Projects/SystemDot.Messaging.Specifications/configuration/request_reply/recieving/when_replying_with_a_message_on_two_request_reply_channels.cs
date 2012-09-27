using SystemDot.Messaging.Channels.RequestReply;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.recieving
{
    [Subject("Request reply configuration")]
    public class when_replying_with_a_message_on_two_request_reply_channels : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";

        static IBus bus;
        static int message;

        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForRequestReplyRecieving()
                .OpenChannel(ChannelName).ForRequestReplyRecieving()
                .Initialise();

            Resolve<ReplyAddressLookup>().SetCurrentRecieverAddress(BuildAddress(ChannelName));
            Resolve<ReplyAddressLookup>().SetCurrentSenderAddress(BuildAddress(SenderChannelName));

            message = 1;
        };

        Because of = () => bus.Reply(message);

        It should_send_both_messages_with_the_correct_to_address_through_the_channels = () =>
             MessageSender.SentMessages.Count.ShouldEqual(2);
    }
}