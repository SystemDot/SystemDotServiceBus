using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Simple;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.point_to_point
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
            Messenger.RegisterHandler<MessageRemovedFromCache>(e => @event = e);

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress)
                .ForPointToPointReceiving()
                .WithDurability()
                .Initialise();

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlingEndpoint>().RegisterHandler(handler);

            message = 1;

            payload = new MessagePayload().MakeSequencedReceivable(
                message,
                SenderAddress,
                ReceiverAddress,
                PersistenceUseType.PointToPointReceive);
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_notify_that_the_message_was_removed_from_the_cache = () =>
            @event.ShouldMatch(e => e.MessageId == payload.Id
                && e.Address == payload.GetToAddress()
                && e.UseType == PersistenceUseType.PointToPointReceive);
    }
}