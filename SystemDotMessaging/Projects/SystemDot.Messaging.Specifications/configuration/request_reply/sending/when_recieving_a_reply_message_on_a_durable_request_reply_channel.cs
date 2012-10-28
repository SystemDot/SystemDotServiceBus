using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.sending
{
    [Subject("Request reply configuration")]
    public class when_recieving_a_reply_message_on_a_durable_request_reply_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string RecieverAddress = "TestRecieverAddress";

        static MessagePayload payload;
        
        Establish context = () =>
        {
            ConfigureAndRegister<IDatastore>(new TestDatastore());
            
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForRequestReplySendingTo(RecieverAddress)
                    .WithDurability()
                .Initialise();

            payload = CreateRecieveablePayload(1, RecieverAddress, ChannelName, PersistenceUseType.ReplySend);
        };

        Because of = () => MessageReciever.RecieveMessage(payload);

        It should_have_persisted_the_message = () =>
           Resolve<IDatastore>().As<TestDatastore>().AddedMessages.ShouldContain(payload);
    }
}