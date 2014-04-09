using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.blocking_messages_for_subscribers
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_on_a_channel_in_non_block_mode : WithMessageConfigurationSubject
    {
        const string SubscriberChannel = "SubscriberChannel";
        const string PublisherChannel = "PublisherChannel";
        static TestMessageHandler<long> handler;

        Establish context = () =>
        {
            handler = new TestMessageHandler<long>();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SubscriberChannel).ForSubscribingTo(PublisherChannel).BlockMessagesIf(false)
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

        It should_handle_the_message = () => handler.LastHandledMessage.ShouldBeEquivalentTo(1);
    }
}