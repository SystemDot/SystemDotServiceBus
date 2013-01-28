using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.configuration.point_to_point.receiving
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_on_a_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderAddress = "TestSenderAddress";

        static int message;
        static MessagePayload payload;
        static TestMessageHandler<int> handler;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                    .OpenChannel(ChannelName)
                    .ForPointToPointReceiving()
                .Initialise();

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            message = 1;
            payload = new MessagePayload().MakeReceiveable(
                message, 
                SenderAddress, 
                ChannelName, 
                PersistenceUseType.PointToPointReceive);
        };

        Because of = () => MessageReciever.ReceiveMessage(payload);

        It should_push_the_message_to_any_registered_handlers = () => handler.HandledMessage.ShouldEqual(message);

        It should_send_an_acknowledgement_for_the_message = () =>
            MessageSender.SentMessages.ShouldContain(a => a.GetAcknowledgementId() == payload.GetSourcePersistenceId());
    }
}