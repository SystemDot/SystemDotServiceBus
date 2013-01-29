using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;
using SystemDot.Messaging.Channels.Sequencing;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.requests
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_an_out_of_sequence_request_on_a_durable_channel : WithMessageConfigurationSubject
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
                    .ForRequestReplyRecieving()
                    .WithDurability()
                .Initialise();

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            message = 1;

            payload = new MessagePayload().MakeReceiveable(
                message, 
                SenderAddress, 
                ChannelName, 
                PersistenceUseType.RequestSend);

            payload.SetSequence(2);
        };

        Because of = () => MessageReciever.ReceiveMessage(payload);

        It should_not_push_the_message_to_any_registered_handlers = () => handler.HandledMessage.ShouldEqual(0);
    }
}