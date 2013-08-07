using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.handling
{
    [Subject(SpecificationGroup.Description)]
    public class when_routing_a_message_to_two_handlers_of_differing_messages : WithMessageConfigurationSubject
    {
        const string Message = "Test";

        static string receiverAddress;
        static MessagePayload payload;
        static TestMessageHandler<string> handler1;
        static TestMessageHandler<int> handler2;

        Establish context = () =>
        {
            receiverAddress = "ReceiverAddress";
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(receiverAddress).ForPointToPointReceiving()
                .Initialise();

            handler1 = new TestMessageHandler<string>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler1);

            handler2 = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler2);

            payload = new MessagePayload().MakeSequencedReceivable(
                Message,
                "SenderAddress",
                receiverAddress,
                PersistenceUseType.PointToPointReceive);
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_handle_the_message_only_in_the_handler_for_the_message_type = () => 
            handler1.LastHandledMessage.ShouldEqual(Message);
    }
}