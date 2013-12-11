using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.restarting_messaging
{
    [Subject(SpecificationGroup.Description)]
    public class when_restarting_after_handling_a_reply_on_a_non_durable_channel_that_fails 
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string ReceiverAddress = "TestReceiveAddress";
        const int Message = 1;
        
        static TestMessageHandler<int> handler;
        static ChangeStore changeStore;
            
        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore();
            ConfigureAndRegister<ChangeStore>(changeStore);
            
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplySendingTo(ReceiverAddress)
                .RegisterHandlers(r => r.RegisterHandler(new FailingMessageHandler<int>()))
                .Initialise();

            Catch.Exception(() => GetServer().ReceiveMessage(
                new MessagePayload().MakeSequencedReceivable(
                    Message,
                    ReceiverAddress, 
                    ChannelName, 
                    PersistenceUseType.ReplyReceive)));
           
            Reset();
            ReInitialise();

            ConfigureAndRegister<ChangeStore>(changeStore);
            handler = new TestMessageHandler<int>();
        };

        Because of = () => 
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplySendingTo(ReceiverAddress)
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

        It should_repeat_the_message_when_restarted = () => handler.LastHandledMessage.ShouldNotEqual(Message);
    }
}