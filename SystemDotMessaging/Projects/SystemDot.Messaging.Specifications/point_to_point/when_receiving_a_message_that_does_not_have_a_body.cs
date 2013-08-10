using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.point_to_point
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_that_does_not_have_a_body
        : WithMessageConfigurationSubject
    {
        const string ReceiverAddress = "ReceiverAddress";

        static MessagePayload payload;
        static TestMessageHandler<int> handler;
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress)
                    .ForPointToPointReceiving()
                .Initialise();

            payload = new MessagePayload().MakeSequencedReceivable(
                1,
                "SenderAddress",
                ReceiverAddress,
                PersistenceUseType.PointToPointReceive);

            payload.RemoveHeader(typeof(BodyHeader));

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_not_handle_the_message = () => handler.LastHandledMessage.ShouldEqual(0);
    }
}
