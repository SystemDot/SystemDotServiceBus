using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.channels.publishing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.request_reply.replies
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
                    .ForRequestReplyRecieving()
                        .WithMessageExpiry(MessageExpiry.ByTime(TimeSpan.FromMinutes(0)))
                .Initialise();

            Server.ReceiveMessage(new MessagePayload().MakeSequencedReceivable(
                1, 
                SenderChannelName, 
                ChannelName, 
                PersistenceUseType.RequestSend));

            message = 1;
        };

        Because of = () => Bus.Reply(message);

        It should_not_send_the_message = () => Server.SentMessages.ExcludeAcknowledgements().ShouldBeEmpty();
    }
}