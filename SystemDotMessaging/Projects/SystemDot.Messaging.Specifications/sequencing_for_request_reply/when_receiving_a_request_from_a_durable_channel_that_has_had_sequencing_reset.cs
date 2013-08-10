using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.sequencing_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_request_from_a_durable_channel_that_has_had_sequencing_reset : WithMessageConfigurationSubject
    {
        const Int64 Message1 = 1;
        const Int64 Message2 = 2;
        const string ReceiverAddress = "ReceiverAddress";
        const string SenderAddress = "SenderAddress";

        static TestMessageHandler<Int64> handler;
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress)
                .ForRequestReplyReceiving().WithDurability()
                .Initialise();

            messagePayload = new MessagePayload()
                .MakeSequencedReceivable(Message1, SenderAddress, ReceiverAddress, PersistenceUseType.RequestSend);

            handler = new TestMessageHandler<Int64>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            GetServer().ReceiveMessage(messagePayload);

            messagePayload = new MessagePayload()
                .MakeReceivable(Message2, SenderAddress, ReceiverAddress, PersistenceUseType.RequestSend);
            messagePayload.SetFirstSequence(5);
            messagePayload.SetSequenceOriginSetOn(DateTime.Now);
            messagePayload.SetSequence(5);
        };

        Because of = () => GetServer().ReceiveMessage(messagePayload);

        It should_pass_the_message_after_the_reset_through = () => handler.HandledMessages.ShouldContain(Message2);
    }
}