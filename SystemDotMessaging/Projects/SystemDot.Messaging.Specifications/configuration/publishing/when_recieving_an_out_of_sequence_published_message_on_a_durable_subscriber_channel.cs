using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Sequencing;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject("Publishing configuration")]
    public class when_recieving_an_out_of_sequence_published_message_on_a_durable_subscriber_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "TestChannel";
        const string PublisherName = "TestPublisher";
        static int message;
        static MessagePayload payload;
        static TestMessageHandler<int> handler;

        Establish context = () =>
            {
                Configuration.Configure.Messaging()
                    .UsingInProcessTransport()
                    .OpenChannel(ChannelName).ForSubscribingTo(PublisherName).WithDurability()
                    .Initialise();

                handler = new TestMessageHandler<int>();
                Resolve<MessageHandlerRouter>().RegisterHandler(handler);

                message = 1;
                payload = CreateRecieveablePayload(message, PublisherName, ChannelName);
                payload.SetSequence(2);
            };

        Because of = () => MessageReciever.RecieveMessage(payload);

        It should_not_push_the_message_to_any_registered_handlers = () => handler.HandledMessage.ShouldEqual(0);
    }
}