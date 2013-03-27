using SystemDot.Messaging.RequestReply.Builders;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_a_request_reply_channel : WithMessageConfigurationSubject
    {
        const string SenderAddress = "SenderAddress";
        const string ReceiverAddress = "ReceiverAddress";

        static RequestSendChannelBuilt requestSendChannelBuiltEvent;
        static ReplyReceiveChannelBuilt replyReceiveChannelBuiltEvent;

        Establish context = () =>
        {
            Messenger.Register<ReplyReceiveChannelBuilt>(m => replyReceiveChannelBuiltEvent = m);
            Messenger.Register<RequestSendChannelBuilt>(m => requestSendChannelBuiltEvent = m);
        };

        Because of = () => Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderAddress)
                .ForRequestReplySendingTo(ReceiverAddress)
                .Initialise();

        It should_notify_that_the_reply_receive_channel_was_built = () =>
            replyReceiveChannelBuiltEvent.ShouldMatch(m => 
                m.CacheAddress == BuildAddress(SenderAddress)
                && m.ReceiverAddress == BuildAddress(ReceiverAddress)
                && m.SenderAddress == BuildAddress(SenderAddress));

        It should_notify_that_the_request_send_channel_was_built = () =>
           requestSendChannelBuiltEvent.ShouldMatch(m =>
               m.CacheAddress == BuildAddress(SenderAddress)
               && m.SenderAddress == BuildAddress(SenderAddress)
               && m.ReceiverAddress == BuildAddress(ReceiverAddress));
    }
}