using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.point_to_point.receiving
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_on_a_durable_channel : WithMessageConfigurationSubject
    {
        const string ReceiverAddress = "ReceiverAddress";
        const string SenderAddress = "SenderAddress";

        static int message;
        static MessagePayload payload;
        static TestMessageHandler<int> handler;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress)
                .ForPointToPointReceiving()
                .WithDurability()
                .Initialise();

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            message = 1;
            payload = new MessagePayload().MakeSequencedReceivable(
                message,
                SenderAddress,
                ReceiverAddress,
                PersistenceUseType.PointToPointReceive);
        };

        Because of = () => Server.ReceiveMessage(payload);

        It should_persist_the_message = () =>
            Resolve<IChangeStore>()
                .GetReceiveMessages(PersistenceUseType.PointToPointReceive, BuildAddress(ReceiverAddress))
                .ShouldNotBeEmpty();
    }
}