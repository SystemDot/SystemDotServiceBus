using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.filtering_by_name
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_request_with_the_wrong_name_down_a_channel : WithMessageConfigurationSubject
    {
        const string ReceiverAddress = "ReceiverAddress";
        static MessagePayload payload;
        static TestMessageHandler<TestNamePatternMessage> handler;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress)
                .ForPointToPointReceiving()
                .OnlyForMessages(FilteredBy.NamePattern("SomethingElse"))
                .Initialise();

            payload = new MessagePayload().MakeSequencedReceivable(
                new TestNamePatternMessage(),
                "SenderAddress",
                ReceiverAddress,
                PersistenceUseType.PointToPointReceive);

            handler = new TestMessageHandler<TestNamePatternMessage>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_not_pass_the_message_through = () => handler.HandledMessages.ShouldBeEmpty();
    }
}