using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.handling
{
    [Subject(SpecificationGroup.Description)]
    public class when_routing_a_message_to_two_handlers : WithMessageConfigurationSubject
    {
        const string Message = "Test";
        const string ReceiverAddress = "ReceiverAddress";

        static MessagePayload payload;
        static TestMessageHandler<string> handler1;
        static TestMessageHandler<string> handler2;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress).ForPointToPointReceiving()
                .Initialise();

            handler1 = new TestMessageHandler<string>();
            Resolve<MessageHandlingEndpoint>().RegisterHandler(handler1); 
            
            handler2 = new TestMessageHandler<string>();
            Resolve<MessageHandlingEndpoint>().RegisterHandler(handler2);

            payload = new MessagePayload().MakeSequencedReceivable(
                Message,
                "SenderAddress",
                ReceiverAddress,
                PersistenceUseType.PointToPointReceive);
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_handle_the_message_in_the_first_handler = () => handler1.LastHandledMessage.ShouldBeEquivalentTo(Message);

        It should_handle_the_message_in_the_second_handler = () => handler2.LastHandledMessage.ShouldBeEquivalentTo(Message);
    }
}