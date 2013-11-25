using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using SystemDot.Storage.Changes.Upcasting;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.sequencing
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_then_restarting_messaging_then_receiving_a_message_from_a_cache_reset_channel
        : WithMessageConfigurationSubject
    {
        const string ReceiverAddress = "ReceiverAddress";
        const string SenderAddress = "SenderAddress";
        const Int64 Message = 1;

        static ChangeStore changeStore;
        static TestMessageHandler<Int64> handler;
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore();
            ConfigureAndRegister<ChangeStore>(changeStore);

            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress).ForPointToPointReceiving().WithDurability()
                .Initialise();

            messagePayload = new MessagePayload()
                .MakeReceivable(2, SenderAddress, ReceiverAddress, PersistenceUseType.PointToPointSend);

            messagePayload.SetSequence(2);
            messagePayload.SetFirstSequence(1);
            messagePayload.SetSequenceOriginSetOn(DateTime.Now.AddHours(-1));

            handler = new TestMessageHandler<Int64>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            GetServer().ReceiveMessage(messagePayload);

            Reset();
            ReInitialise();

            ConfigureAndRegister<ChangeStore>(changeStore);

            handler.HandledMessages.Clear();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            Messaging.Configuration.Configure.Messaging()
            .UsingInProcessTransport()
            .OpenChannel(ReceiverAddress).ForPointToPointReceiving().WithDurability()
            .Initialise();

            messagePayload = new MessagePayload()
                .MakeReceivable(Message, SenderAddress, ReceiverAddress, PersistenceUseType.PointToPointSend);

            messagePayload.SetSequence(1);
            messagePayload.SetFirstSequence(1);
            messagePayload.SetSequenceOriginSetOn(DateTime.Now);
        };

        Because of = () => GetServer().ReceiveMessage(messagePayload);

        It should_have_cleared_the_messages_from_before_the_reset_from_persistence = () =>
            handler.HandledMessages.ShouldContainOnly(Message);
       
    }
}