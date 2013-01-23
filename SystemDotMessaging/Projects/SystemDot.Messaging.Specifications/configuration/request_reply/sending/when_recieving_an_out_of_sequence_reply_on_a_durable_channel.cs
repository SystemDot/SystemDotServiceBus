using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;
using SystemDot.Messaging.Channels.Sequencing;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.sending
{
    [Subject(SpecificationGroup.Description)]
    public class when_recieving_an_out_of_sequence_reply_on_a_durable_channel 
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string RecieverAddress = "TestRecieverAddress";

        static int message;
        static MessagePayload payload;
        static TestMessageHandler<int> handler;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForRequestReplySendingTo(RecieverAddress)
                    .WithDurability()
                .Initialise();

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            message = 1;
            payload = new MessagePayload().MakeReceiveable(message, RecieverAddress, ChannelName, PersistenceUseType.RequestSend);
            payload.SetSequence(2);
        };

        Because of = () => MessageReciever.RecieveMessage(payload);

        It should_not_push_the_message_to_any_registered_handlers = () => handler.HandledMessage.ShouldEqual(0);
    }
}