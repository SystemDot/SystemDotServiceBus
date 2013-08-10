using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_request_on_a_durable_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderAddress = "TestSenderAddress";

        static MessagePayload payload;
        
        Establish context = () =>
        {
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForRequestReplyReceiving()
                    .WithDurability()
                .Initialise();

            payload = new MessagePayload().MakeSequencedReceivable(
                1, 
                SenderAddress, 
                ChannelName, 
                PersistenceUseType.RequestSend);
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_have_persisted_the_message = () =>
            Resolve<IChangeStore>()
                .GetReceiveMessages(PersistenceUseType.RequestReceive, BuildAddress(SenderAddress))               
                .ShouldContain(payload);
    }
}