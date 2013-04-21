using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.sequencing
{
    [Subject(SpecificationGroup.Description)]
    public class when_passing_messages_in_the_correct_order_on_a_durable_channel : WithMessageConfigurationSubject
    {
        const int Message1 = 1;
        const int Message2 = 2;
        const string ReceiverAddress = "ReceiverAddress";
        const string SenderAddress = "SenderAddress";

        static TestMessageHandler<int> handler;
        static MessagePayload messagePayload1;
        static MessagePayload messagePayload2;
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress).ForPointToPointReceiving().WithDurability()
                .Initialise();
            
            messagePayload1 = new MessagePayload()
                .MakeReceivable(Message1, SenderAddress, ReceiverAddress, PersistenceUseType.PointToPointSend);
            messagePayload1.SetSequence(1);
            messagePayload1.SetFirstSequence(1);
            messagePayload1.SetSequenceOriginSetOn(DateTime.Today);

            messagePayload2 = new MessagePayload()
                .MakeReceivable(Message2, SenderAddress, ReceiverAddress, PersistenceUseType.PointToPointSend);
            messagePayload2.SetSequence(2);
            messagePayload2.SetFirstSequence(2);
            messagePayload2.SetSequenceOriginSetOn(DateTime.Today);

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () =>
        {
            Server.ReceiveMessage(messagePayload1);
            Server.ReceiveMessage(messagePayload2);
        };

        It should_pass_the_messages_through = () => handler.HandledMessages.ShouldContain(Message1, Message2);
    }
}