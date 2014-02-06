using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.sequencing
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_from_a_sequenced_channel_that_has_had_sequencing_reset : WithMessageConfigurationSubject
    {
        const Int64 Message1 = 1;
        const Int64 Message2 = 2;
        const string ReceiverAddress = "ReceiverAddress";
        const string SenderAddress = "SenderAddress";

        static TestMessageHandler<Int64> handler;
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress).ForPointToPointReceiving().Sequenced()
                .Initialise();

            messagePayload = new MessagePayload()
                .MakeSequencedReceivable(Message1, SenderAddress, ReceiverAddress, PersistenceUseType.PointToPointSend);

            handler = new TestMessageHandler<Int64>();
            Resolve<MessageHandlingEndpoint>().RegisterHandler(handler);

            GetServer().ReceiveMessage(messagePayload);

            messagePayload = new MessagePayload()
                .MakeReceivable(Message2, SenderAddress, ReceiverAddress, PersistenceUseType.PointToPointSend);
            messagePayload.SetFirstSequence(5);
            messagePayload.SetSequenceOriginSetOn(DateTime.Now);
            messagePayload.SetSequence(5);
        };

        Because of = () => GetServer().ReceiveMessage(messagePayload);

        It should_pass_the_message_after_the_reset_through = () => handler.HandledMessages.ShouldContain(Message2);
    }
}