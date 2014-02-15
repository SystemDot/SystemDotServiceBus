using System;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.batching_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_replies_inside_a_batch : WithMessageConfigurationSubject
    {
        const int Message1 = 1;
        const int Message2 = 2;
        const string SenderAddress = "SenderAddress";
        const string ReceiverAddress = "ReceiverAddress";

        static TestMessageHandler<Int64> handler;
        static MessagePayload messagePayload1;
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderAddress).ForRequestReplySendingTo(ReceiverAddress)
                .Initialise();

            var aggregateMessage = new BatchMessage();
            aggregateMessage.Messages.Add(Message1);
            aggregateMessage.Messages.Add(Message2);

            messagePayload1 = new MessagePayload()
                .MakeSequencedReceivable(aggregateMessage, ReceiverAddress, SenderAddress, PersistenceUseType.ReplyReceive);

            handler = new TestMessageHandler<Int64>();
            Resolve<MessageHandlingEndpoint>().RegisterHandler(handler);
        };

        Because of = () => GetServer().ReceiveMessage(messagePayload1);

        It should_pass_both_the_messages_from_the_aggregation_through_seperately = () =>
            handler.HandledMessages.Should().Contain(m => m == Message1 && m == Message2);
    }
}