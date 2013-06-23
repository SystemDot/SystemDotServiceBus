using System;
using System.Linq;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.batching_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_requests_inside_a_batch_and_replying_to_each : WithMessageConfigurationSubject
    {
        const Int64 Message1 = 1;
        const Int64 Message2 = 2;
        const string SenderAddress = "SenderAddress";
        const string ReceiverAddress = "ReceiverAddress";

        static TestReplyMessageHandler<Int64> handler;
        static MessagePayload messagePayload;
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress).ForRequestReplyRecieving()
                .Initialise();

            var aggregateMessage = new BatchMessage();
            aggregateMessage.Messages.Add(Message1);
            aggregateMessage.Messages.Add(Message2);

            messagePayload = new MessagePayload()
                .MakeSequencedReceivable(aggregateMessage, SenderAddress, ReceiverAddress, PersistenceUseType.RequestReceive);

            handler = new TestReplyMessageHandler<Int64>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () => Server.ReceiveMessage(messagePayload);

        It should_send_a_batch_containing_both_replied_messages = () =>
            Server.SentMessages.ExcludeAcknowledgements().Single()
                .DeserialiseTo<BatchMessage>().Messages.ShouldContain(Message1, Message2);
    }
}