using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.recieving
{
    [Subject("Request reply configuration")]
    public class when_replying_with_a_message_on_a_durable_request_reply_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";

        static IBus bus;
        static int message;
        static TestPersistence persistence;

        Establish context = () =>
        {
            var persistenceFactory = new TestPersistenceFactory();
            ConfigureAndRegister<IPersistenceFactory>(persistenceFactory);

            persistence = new TestPersistence();
            persistenceFactory.AddPersistence(PersistenceUseType.RequestReceive, new TestPersistence());
            persistenceFactory.AddPersistence(PersistenceUseType.ReplySend, persistence);

            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()                
                .OpenChannel(ChannelName).ForRequestReplyRecieving().WithDurability()
                .Initialise();

            Resolve<ReplyAddressLookup>().SetCurrentRecieverAddress(BuildAddress(ChannelName));
            Resolve<ReplyAddressLookup>().SetCurrentSenderAddress(BuildAddress(SenderChannelName));

            message = 1;
        };

        Because of = () => bus.Reply(message);

        It should_persist_the_message = () => persistence.GetMessages().ShouldNotBeEmpty();
    }
}