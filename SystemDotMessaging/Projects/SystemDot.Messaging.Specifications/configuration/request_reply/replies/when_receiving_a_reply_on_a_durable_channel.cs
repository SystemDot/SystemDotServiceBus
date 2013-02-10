using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.replies
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_reply_on_a_durable_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string RecieverAddress = "TestRecieverAddress";

        static MessagePayload payload;
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForRequestReplySendingTo(RecieverAddress)
                    .WithDurability()
                .Initialise();

            payload = new MessagePayload().MakeReceiveable(
                1, 
                RecieverAddress, 
                ChannelName, 
                PersistenceUseType.ReplySend);
        };

        Because of = () => MessageServer.ReceiveMessage(payload);

        It should_have_persisted_the_message = () => 
            Resolve<IChangeStore>()
                .GetAddedMessages(PersistenceUseType.ReplyReceive, BuildAddress(ChannelName))
                .ShouldContain(payload);
    }
}