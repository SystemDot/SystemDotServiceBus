using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.restarting_messaging
{
    [Subject(SpecificationGroup.Description)]
    public class when_restarting_messaging_after_handling_a_request_on_a_durable_channel_that_fails 
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
                .WithDurability()
                .RegisterHandlers(r => r.RegisterHandler(new FailingMessageHandler<int>()))
                .Initialise();

            Catch.Exception(() => Server.ReceiveMessage(
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
                .WithDurability()
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

        It should_repeat_the_message_when_restarted = () => handler.LastHandledMessage.ShouldEqual(Message);
    }
}