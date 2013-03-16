using System.Collections.Generic;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_a_request_reply_channel : WithMessageConfigurationSubject
    {
        const string SenderAddress = "SenderAddress";
        const string ReceiverAddress = "ReceiverAddress";

        static List<ChannelBuilt> channelBuiltEvents;

        Establish context = () =>
        {
            channelBuiltEvents = new List<ChannelBuilt>();
            Messenger.Register<ChannelBuilt>(m => channelBuiltEvents.Add(m));
        };

        Because of = () => Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderAddress)
                .ForRequestReplySendingTo(ReceiverAddress)
                .Initialise();

        It should_notify_that_the_reply_receive_channel_was_built = () => 
            channelBuiltEvents.ShouldContain(m => 
                m.UseType == PersistenceUseType.ReplyReceive
                && m.CacheAddress == BuildAddress(SenderAddress)
                && m.FromAddress == BuildAddress(ReceiverAddress)
                && m.ToAddress == BuildAddress(SenderAddress));

        It should_notify_that_the_request_send_channel_was_built = () =>
           channelBuiltEvents.ShouldContain(m =>
               m.UseType == PersistenceUseType.RequestSend
               && m.CacheAddress == BuildAddress(SenderAddress)
                && m.FromAddress == BuildAddress(SenderAddress)
                && m.ToAddress == BuildAddress(ReceiverAddress));
    }
}