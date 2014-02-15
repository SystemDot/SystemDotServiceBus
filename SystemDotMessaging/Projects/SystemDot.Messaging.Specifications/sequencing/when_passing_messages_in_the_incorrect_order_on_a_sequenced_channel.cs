using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.sequencing
{
    [Subject(SpecificationGroup.Description)]
    public class when_passing_messages_in_the_incorrect_order_on_a_sequenced_channel : WithMessageConfigurationSubject
    {
        const int Message1 = 1;
        const int Message2 = 2;

        static TestMessageHandler<int> handler;
        static MessagePayload messagePayload1;
        static MessagePayload messagePayload2;

        Establish context = () =>
        {
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("ReceiverAddress").ForPointToPointReceiving().Sequenced()
                .Initialise();

            messagePayload1 = new MessagePayload()
                .MakeReceivable(Message1, "SenderAddress", "ReceiverAddress", PersistenceUseType.PointToPointSend);
            messagePayload1.SetSequence(2);
            messagePayload1.SetFirstSequence(1);
            messagePayload1.SetSequenceOriginSetOn(DateTime.Today);

            messagePayload2 = new MessagePayload()
                .MakeReceivable(Message2, "SenderAddress", "ReceiverAddress", PersistenceUseType.PointToPointSend);
            messagePayload2.SetSequence(4);
            messagePayload2.SetFirstSequence(1);
            messagePayload2.SetSequenceOriginSetOn(DateTime.Today);

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlingEndpoint>().RegisterHandler(handler);

        };

        Because of = () =>
        {
            GetServer().ReceiveMessage(messagePayload1);
            GetServer().ReceiveMessage(messagePayload2);
        };

        It should_not_pass_the_messages_through = () => 
            handler.HandledMessages.Should().NotContain(m => m == Message1 || m == Message2);
    }
}