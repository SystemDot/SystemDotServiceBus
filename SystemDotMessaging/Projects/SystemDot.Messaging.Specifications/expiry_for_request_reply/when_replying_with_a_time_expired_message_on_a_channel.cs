using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.expiry_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_replying_with_a_time_expired_message_on_a_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";
        
        static int message;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForRequestReplyReceiving()
                        .ExpireMessages().After(TimeSpan.FromMinutes(0))
                .Initialise();

            GetServer().ReceiveMessage(new MessagePayload().MakeSequencedReceivable(
                1, 
                SenderChannelName, 
                ChannelName, 
                PersistenceUseType.RequestSend));

            message = 1;
        };

        Because of = () => Bus.Reply(message);

        It should_not_send_the_message = () => GetServer().SentMessages.ExcludeAcknowledgements().ShouldBeEmpty();
    }
}