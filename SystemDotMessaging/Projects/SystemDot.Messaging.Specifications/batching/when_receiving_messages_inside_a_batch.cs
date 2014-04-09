using System;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.batching
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_messages_inside_a_batch : WithMessageConfigurationSubject
    {
        const int Message1 = 1;
        const int Message2 = 2;
        const string ReceiverAddress = "ReceiverAddress";

        static TestMessageHandler<Int64> handler;
        static MessagePayload messagePayload;
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress).ForPointToPointReceiving()
                .Initialise();

            var aggregateMessage = new BatchMessage();
            aggregateMessage.Messages.Add(Message1);
            aggregateMessage.Messages.Add(Message2);

            messagePayload = new MessagePayload().MakeSequencedReceivable(
                aggregateMessage, 
                "SenderAddress", 
                ReceiverAddress, 
                PersistenceUseType.PointToPointSend);

            handler = new TestMessageHandler<Int64>();
            Resolve<MessageHandlingEndpoint>().RegisterHandler(handler);
        };

        Because of = () => GetServer().ReceiveMessage(messagePayload);

        It should_pass_the_first_message_from_the_aggregation_through_seperately = () =>
            handler.HandledMessages.Should().Contain(m => m == Message1);

        It should_pass_the_second_message_from_the_aggregation_through_seperately = () =>
            handler.HandledMessages.Should().Contain(m => m == Message2);
    }
}