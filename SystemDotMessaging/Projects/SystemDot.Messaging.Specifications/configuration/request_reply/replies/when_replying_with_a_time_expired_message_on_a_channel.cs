using System;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Storage;
using Machine.Specifications;
using SystemDot.Messaging.Specifications.configuration.publishing;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.replies
{
    [Subject(SpecificationGroup.Description)]
    public class when_replying_with_a_time_expired_message_on_a_channel : WithMessageConfigurationSubject
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

            MessageReciever.ReceiveMessage(new MessagePayload().MakeReceiveable(
                1, 
                SenderChannelName, 
                ChannelName, 
                PersistenceUseType.RequestSend));

            message = 1;
        };

        Because of = () => bus.Reply(message);

        It should_not_send_the_message = () => MessageSender.SentMessages.ExcludeAcknowledgements().ShouldBeEmpty();
    }
}