using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Storage;
using Machine.Specifications;
using SystemDot.Messaging.Specifications.configuration.publishing;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.recieving
{
    [Subject("Request reply configuration")]
    public class when_replying_with_a_message_on_two_request_reply_channels : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";

        static IBus bus;

        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForRequestReplyRecieving()
                .OpenChannel(ChannelName).ForRequestReplyRecieving()
                .Initialise();

            MessageReciever.RecieveMessage(CreateReceiveablePayload(1, SenderChannelName, ChannelName, PersistenceUseType.RequestSend));
        };

        Because of = () => bus.Reply(1);

        It should_send_both_messages_with_the_correct_to_address_through_the_channels = () =>
             MessageSender.SentMessages.ExcludeAcknowledgements().Count.ShouldEqual(2);
    }
}