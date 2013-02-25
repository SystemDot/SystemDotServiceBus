using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.point_to_point.sequencing
{
    [Subject(SpecificationGroup.Description)]
    public class when_passing_messages_in_the_correct_order_on_a_durable_channel_and_then_restarting_messaging 
        : WithMessageConfigurationSubject
    {
        const string ReceiverAddress = "ReceiverAddress";
        const string SenderAddress = "SenderAddress";

        static IChangeStore changeStore;
        static TestMessageHandler<int> handler;
       
        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore(new PlatformAgnosticSerialiser());
            ConfigureAndRegister<IChangeStore>(changeStore);

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress).ForPointToPointReceiving().WithDurability()
                .Initialise();

            var messagePayload = new MessagePayload()
                .MakeSequencedReceivable(1, SenderAddress, ReceiverAddress, PersistenceUseType.PointToPointSend);
           
            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            Server.ReceiveMessage(messagePayload);

            ResetIoc();
            Initialise();

            ConfigureAndRegister<IChangeStore>(changeStore);
            
            handler.HandledMessages.Clear();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () => Configuration.Configure.Messaging()
            .UsingInProcessTransport()
            .OpenChannel(ReceiverAddress).ForPointToPointReceiving().WithDurability()
            .Initialise();

        It should_have_removed_the_message_from_persistence = () => handler.HandledMessages.ShouldBeEmpty();
    }
}