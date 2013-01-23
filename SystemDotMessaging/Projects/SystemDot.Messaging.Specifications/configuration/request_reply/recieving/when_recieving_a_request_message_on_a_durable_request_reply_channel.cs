using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.recieving
{
    [Subject("Request reply configuration")]
    public class when_recieving_a_request_message_on_a_durable_request_reply_channel : WithNoRepeaterMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderAddress = "TestSenderAddress";

        static MessagePayload payload;
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForRequestReplyRecieving()
                    .WithDurability()
                .Initialise();

            payload = new MessagePayload().MakeReceiveable(1, SenderAddress, ChannelName, PersistenceUseType.RequestSend);
        };

        Because of = () => MessageReciever.RecieveMessage(payload);

        It should_have_persisted_the_message = () =>
            Resolve<IChangeStore>()
                .GetAddedMessages(PersistenceUseType.RequestReceive, BuildAddress(SenderAddress))               
                .ShouldContain(payload);
    }
}