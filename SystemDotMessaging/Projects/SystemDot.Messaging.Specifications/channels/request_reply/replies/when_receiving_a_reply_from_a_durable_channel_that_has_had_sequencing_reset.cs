using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.request_reply.replies
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_reply_from_a_durable_channel_that_has_had_sequencing_reset : WithMessageConfigurationSubject
    {
        const int Message1 = 1;
        const int Message2 = 2;
        const string ReceiverAddress = "ReceiverAddress";
        const string SenderAddress = "SenderAddress";

        static TestMessageHandler<int> handler;
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderAddress)
                .ForRequestReplySendingTo(ReceiverAddress).WithDurability()
                .Initialise();

            messagePayload = new MessagePayload()
                .MakeSequencedReceivable(Message1, ReceiverAddress, SenderAddress, PersistenceUseType.ReplySend);

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            Server.ReceiveMessage(messagePayload);

            messagePayload = new MessagePayload()
                .MakeReceivable(Message2, ReceiverAddress, SenderAddress, PersistenceUseType.ReplySend);
            messagePayload.SetFirstSequence(5);
            messagePayload.SetSequenceOriginSetOn(DateTime.Now);
            messagePayload.SetSequence(5);
        };

        Because of = () => Server.ReceiveMessage(messagePayload);

        It should_pass_the_message_after_the_reset_through = () => handler.HandledMessages.ShouldContain(Message2);
    }
}