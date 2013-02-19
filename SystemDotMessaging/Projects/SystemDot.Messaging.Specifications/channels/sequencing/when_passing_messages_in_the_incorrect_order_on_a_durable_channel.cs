using System.Collections.Generic;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.sequencing
{
    [Subject(SpecificationGroup.Description)]
    public class when_passing_messages_in_the_incorrect_order_on_a_durable_channel : WithMessageConfigurationSubject
    {
        const int Message1 = 1;
        const int Message2 = 2;

        static TestMessageHandler<int> handler;
        static MessagePayload messagePayload1;
        static MessagePayload messagePayload2;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("ReceiverAddress").ForPointToPointReceiving().WithDurability()
                .Initialise();

            messagePayload1 = new MessagePayload()
                .MakeReceiveable(Message1, "SenderAddress", "ReceiverAddress", PersistenceUseType.PointToPointSend);
            messagePayload1.SetSequence(2);

            messagePayload2 = new MessagePayload()
                .MakeReceiveable(Message2, "SenderAddress", "ReceiverAddress", PersistenceUseType.PointToPointSend);
            messagePayload2.SetSequence(4);

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

        };

        Because of = () =>
        {
            Server.ReceiveMessage(messagePayload1);
            Server.ReceiveMessage(messagePayload2);
        };


        It should_not_pass_the_messages_through = () => handler.HandledMessages.ShouldNotContain(Message1, Message2);
    }
}