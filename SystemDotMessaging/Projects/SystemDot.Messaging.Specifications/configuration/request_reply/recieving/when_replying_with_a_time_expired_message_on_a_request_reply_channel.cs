using System;
using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.RequestReply;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.recieving
{
    [Subject("Request reply configuration")]
    public class when_replying_with_a_time_expired_message_on_a_request_reply_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";

        static IBus bus;
        static int message;

        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForRequestReplyRecieving()
                        .WithMessageExpiry(MessageExpiry.ByTime(TimeSpan.FromMinutes(0)))
                .Initialise();

            Resolve<ReplyAddressLookup>().SetCurrentRecieverAddress(BuildAddress(ChannelName));
            Resolve<ReplyAddressLookup>().SetCurrentSenderAddress(BuildAddress(SenderChannelName));

            message = 1;
        };

        Because of = () => bus.Reply(message);

        It should_not_send_the_message = () => MessageSender.SentMessages.ShouldBeEmpty();
    }
}