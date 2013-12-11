using System;
using System.Linq;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.restarting_messaging
{
    [Subject(SpecificationGroup.Description)]
    public class when_restarting_on_a_durable_sequenced_channel_after_a_checkpoint_and_then_receiving_a_previous_sequence_request
        : WithMessageConfigurationSubject
    {
        const string ReceiverChannel = "ReceiverChannel";
        const string SenderChannel = "SenderChannel";
        const Int64 MessageWithLowerSequence = 1;

        static TestMessageHandler<Int64> handler;
        static ChangeStore changeStore;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore();
            ConfigureAndRegister<ChangeStore>(changeStore);
            ConfigureAndRegister<ICheckpointStrategy>(new CheckpointAfterOneChangeCheckpointStrategy());

            handler = new TestMessageHandler<Int64>();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverChannel)
                .ForRequestReplyReceiving()
                .WithDurability()
                .Sequenced()
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

            GetServer().ReceiveMessage(new MessagePayload()
                .SetMessageBody(2)
                .SetFromChannel(SenderChannel)
                .SetToChannel(ReceiverChannel)
                .SetChannelType(PersistenceUseType.RequestSend)
                .WithSequenceOf(2));

            Reset();
            ReInitialise();
             
            ConfigureAndRegister<ChangeStore>(changeStore);

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverChannel)
                .ForRequestReplyReceiving()
                .WithDurability()
                .Sequenced()
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();
        };

        Because of = () => GetServer().ReceiveMessage(
            new MessagePayload()
                .SetMessageBody(MessageWithLowerSequence)
                .SetFromChannel(SenderChannel)
                .SetToChannel(ReceiverChannel)
                .SetChannelType(PersistenceUseType.RequestSend)
                .WithSequenceOf(1));

        It should_not_handle_the_message_with_the_lower_sequence = () => 
            handler.LastHandledMessage.ShouldNotEqual(MessageWithLowerSequence);
    }
}