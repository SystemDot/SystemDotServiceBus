using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.receiving
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_on_a_durable_channel : WithMessageConfigurationSubject
    {
        const string ReceiverAddress = "ReceiverAddress";
        const string SenderAddress = "SenderAddress";

        static int message;
        static MessagePayload payload;
        static TestMessageHandler<int> handler;
        static MessageRemovedFromCache @event;

        Establish context = () =>
        {
            Messenger.Register<MessageRemovedFromCache>(e => @event = e);

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

        It should_notify_that_the_message_was_removed_from_the_cache = () =>
            @event.ShouldMatch(e => e.MessageId == payload.Id
                && e.Address == payload.GetToAddress()
                && e.UseType == PersistenceUseType.PointToPointReceive);
    }
}