using SystemDot.Messaging.Aggregation;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.request_reply.replies
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_replies_inside_an_aggregation : WithMessageConfigurationSubject
    {
        const int Message1 = 1;
        const int Message2 = 2;
        const string SenderAddress = "SenderAddress";
        const string ReceiverAddress = "ReceiverAddress";

        static TestMessageHandler<int> handler;
        static MessagePayload messagePayload1;
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderAddress).ForRequestReplySendingTo(ReceiverAddress)
                .Initialise();

            var aggregateMessage = new AggregateMessage();
            aggregateMessage.Messages.Add(Message1);
            aggregateMessage.Messages.Add(Message2);

            messagePayload1 = new MessagePayload()
                .MakeReceiveable(aggregateMessage, ReceiverAddress, SenderAddress, PersistenceUseType.ReplyReceive);
            
            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () => Server.ReceiveMessage(messagePayload1);

        It should_pass_both_the_messages_from_the_aggregation_through_seperately = () => 
            handler.HandledMessages.ShouldContain(Message1, Message2);
    }
}