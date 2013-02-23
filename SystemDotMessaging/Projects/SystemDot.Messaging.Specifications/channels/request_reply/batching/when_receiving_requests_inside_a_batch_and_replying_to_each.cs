using System.Linq;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;
using SystemDot.Messaging.Specifications.channels.publishing;

namespace SystemDot.Messaging.Specifications.channels.request_reply.batching
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_requests_inside_a_batch_and_replying_to_each : WithMessageConfigurationSubject
    {
        const int Message1 = 1;
        const int Message2 = 2;
        const string SenderAddress = "SenderAddress";
        const string ReceiverAddress = "ReceiverAddress";

        static TestReplyMessageHandler<int> handler;
        static MessagePayload messagePayload;
        
        Establish context = () =>
        {
            IBus bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress).ForRequestReplyRecieving()
                .Initialise();

            var aggregateMessage = new BatchMessage();
            aggregateMessage.Messages.Add(Message1);
            aggregateMessage.Messages.Add(Message2);

            messagePayload = new MessagePayload()
                .MakeReceiveable(aggregateMessage, SenderAddress, ReceiverAddress, PersistenceUseType.RequestReceive);

            handler = new TestReplyMessageHandler<int>(bus);
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () => Server.ReceiveMessage(messagePayload);

        It should_send_a_batch_containing_both_replied_messages = () =>
            Server.SentMessages.ExcludeAcknowledgements().Single()
                .DeserialiseTo<BatchMessage>().Messages.ShouldContain(Message1, Message2);
    }
}