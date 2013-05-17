using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.expiry.request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_replying_with_a_time_expired_message_on_a_channel_with_an_action_to_execute_on_expiry_set : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";
        
        static bool expiryActionExecuted;
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplyRecieving()
                .WithMessageExpiry(MessageExpiry.ByTime(TimeSpan.FromMinutes(0)), () => expiryActionExecuted = true)
                .Initialise();

            Server.ReceiveMessage(new MessagePayload().MakeSequencedReceivable(
                1,
                SenderChannelName,
                ChannelName,
                PersistenceUseType.RequestSend));
        };

        Because of = () => Bus.Reply(1);

        It should_execute_the_expiry_action = () => expiryActionExecuted.ShouldBeTrue();

        It should_not_send_the_message = () => Server.SentMessages.ExcludeAcknowledgements().ShouldBeEmpty();
    }
}