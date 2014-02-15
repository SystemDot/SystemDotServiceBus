using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Storage.Changes;
using Machine.Specifications;
using FluentAssertions;

namespace SystemDot.Messaging.Specifications.sequencing
{
    [Subject(SpecificationGroup.Description)]
    public class when_passing_messages_in_the_correct_order_on_a_durable_channel_and_then_restarting_messaging 
        : WithMessageConfigurationSubject
    {
        const string ReceiverAddress = "ReceiverAddress";
        const string SenderAddress = "SenderAddress";

        static ChangeStore changeStore;
        static TestMessageHandler<int> handler;
       
        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore();
            ConfigureAndRegister<ChangeStore>(changeStore);

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress).ForPointToPointReceiving().WithDurability()
                .Initialise();

            var messagePayload = new MessagePayload()
                .MakeSequencedReceivable(1, SenderAddress, ReceiverAddress, PersistenceUseType.PointToPointSend);
           
            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlingEndpoint>().RegisterHandler(handler);

            GetServer().ReceiveMessage(messagePayload);

            Reset();
            ReInitialise();

            ConfigureAndRegister<ChangeStore>(changeStore);
            
            handler.HandledMessages.Clear();
            Resolve<MessageHandlingEndpoint>().RegisterHandler(handler);
        };

        Because of = () => Messaging.Configuration.Configure.Messaging()
            .UsingInProcessTransport()
            .OpenChannel(ReceiverAddress).ForPointToPointReceiving().WithDurability()
            .Initialise();

        It should_have_removed_the_message_from_persistence = () => handler.HandledMessages.Should().BeEmpty();
    }
}