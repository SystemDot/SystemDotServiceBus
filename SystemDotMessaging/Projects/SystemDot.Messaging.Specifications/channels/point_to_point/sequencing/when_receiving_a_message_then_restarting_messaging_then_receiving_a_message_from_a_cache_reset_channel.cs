using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.point_to_point.sequencing
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_then_restarting_messaging_then_receiving_a_message_from_a_cache_reset_channel
        : WithMessageConfigurationSubject
    {
        const string ReceiverAddress = "ReceiverAddress";
        const string SenderAddress = "SenderAddress";
        const int Message = 1;

        static IChangeStore changeStore;
        static TestMessageHandler<int> handler;
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore(new PlatformAgnosticSerialiser());
            ConfigureAndRegister<IChangeStore>(changeStore);

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress).ForPointToPointReceiving().WithDurability()
                .Initialise();

            messagePayload = new MessagePayload()
                .MakeReceivable(2, SenderAddress, ReceiverAddress, PersistenceUseType.PointToPointSend);

            messagePayload.SetSequence(2);
            messagePayload.SetFirstSequence(1);
            messagePayload.SetSequenceOriginSetOn(DateTime.Now.AddHours(-1));

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            Server.ReceiveMessage(messagePayload);

            ResetIoc();
            Initialise();

            ConfigureAndRegister<IChangeStore>(changeStore);

            handler.HandledMessages.Clear();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            Configuration.Configure.Messaging()
            .UsingInProcessTransport()
            .OpenChannel(ReceiverAddress).ForPointToPointReceiving().WithDurability()
            .Initialise();

            messagePayload = new MessagePayload()
                .MakeReceivable(Message, SenderAddress, ReceiverAddress, PersistenceUseType.PointToPointSend);

            messagePayload.SetSequence(1);
            messagePayload.SetFirstSequence(1);
            messagePayload.SetSequenceOriginSetOn(DateTime.Now);
        };

        Because of = () => Server.ReceiveMessage(messagePayload);

        It should_have_cleared_the_messages_from_before_the_reset_from_persistence = () =>
            handler.HandledMessages.ShouldContainOnly(Message);
       
    }
}