using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.flushing_messages_for_subscribers
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_on_a_channel_in_flush_mode : WithMessageConfigurationSubject
    {
        const string SubscriberChannel = "SubscriberChannel";
        const string PublisherChannel = "PublisherChannel";
        static TestMessageHandler<long> handler;

        Establish context = () =>
        {
            handler = new TestMessageHandler<long>();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SubscriberChannel).ForSubscribingTo(PublisherChannel).InFlushMessagesMode()
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

            GetServer().SentMessages.Clear();
        };

        Because of = () => GetServer().ReceiveMessage(
            new MessagePayload()
                .SetMessageBody(1)
                .SetToChannel(SubscriberChannel)
                .SetFromChannel(PublisherChannel)
                .SetChannelType(PersistenceUseType.SubscriberReceive)
                .Sequenced());

        It should_not_handle_the_message = () => handler.LastHandledMessage.ShouldNotEqual(1);

        It should_not_send_an_acknowledgement = () => GetServer().SentMessages.ShouldBeEmpty();
    }
}
