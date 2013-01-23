using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing.receiving
{
    [Subject(SpecificationGroup.Description)]
    public class when_recieving_the_second_message_on_a_durable_channel_that_had_subscribed_after_messages_were_published : WithNoRepeaterMessageConfigurationSubject
    {
        const string ChannelName = "TestChannel";
        const string PublisherName = "TestPublisher";
        static MessagePayload payload;
        static TestMessageHandler<int> handler;
        static int message;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForSubscribingTo(PublisherName)
                .WithDurability()
                .Initialise();

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            payload = new MessagePayload().MakeReceiveable(1, PublisherName, ChannelName, PersistenceUseType.SubscriberSend);
            payload.SetFirstSequence(2);
            payload.SetSequence(2);

            MessageReciever.RecieveMessage(payload);

            message = 2;
            payload = new MessagePayload().MakeReceiveable(message, PublisherName, ChannelName, PersistenceUseType.SubscriberSend);
            payload.SetFirstSequence(2);
            payload.SetSequence(3);
        };

        Because of = () => MessageReciever.RecieveMessage(payload);

        It should_push_the_message_to_any_registered_handlers = () => handler.HandledMessage.ShouldEqual(message);
    }
}