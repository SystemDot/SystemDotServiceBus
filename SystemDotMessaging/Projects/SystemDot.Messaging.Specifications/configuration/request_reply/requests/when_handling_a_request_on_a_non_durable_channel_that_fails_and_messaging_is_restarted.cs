using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.requests
{
    [Subject(SpecificationGroup.Description)]
    public class when_handling_a_request_on_a_non_durable_channel_that_fails_and_messaging_is_restarted 
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderAddress = "TestSenderAddress";
        const int Message = 1;
        
        static TestMessageHandler<int> handler;
        static IChangeStore changeStore;
            
        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore(new PlatformAgnosticSerialiser());
            ConfigureAndRegister<IChangeStore>(changeStore);
            
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplyRecieving()
                .RegisterHandlers(r => r.RegisterHandler(new FailingMessageHandler<int>()))
                .Initialise();

            Catch.Exception(() => MessageReciever.ReceiveMessage(
                new MessagePayload().MakeSequencedReceivable(
                    Message, 
                    SenderAddress, 
                    ChannelName, 
                    PersistenceUseType.RequestSend)));
           
            ResetIoc();
            Initialise();

            ConfigureAndRegister<IChangeStore>(changeStore);
            handler = new TestMessageHandler<int>();
        };

        Because of = () => 
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplyRecieving()
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

        It should_not_repeat_the_message_when_restarted = () => handler.HandledMessage.ShouldNotEqual(Message);
    }
}