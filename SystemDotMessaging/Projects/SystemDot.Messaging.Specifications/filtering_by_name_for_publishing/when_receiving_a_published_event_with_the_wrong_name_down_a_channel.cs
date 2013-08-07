using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.filtering_by_name;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.filtering_by_name_for_publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_published_event_with_the_wrong_name_down_a_channel : WithMessageConfigurationSubject
    {
        const string SubscriberAddress = "SubscriberAddress";
        const string PublisherAddress = "PublisherAddress";

        static MessagePayload payload;
        static TestMessageHandler<TestNamePatternMessage> handler;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SubscriberAddress)
                .ForSubscribingTo(PublisherAddress)
                .OnlyForMessages(FilteredBy.NamePattern("SomethingElse"))
                .Initialise();

            payload = new MessagePayload().MakeSequencedReceivable(
                new TestNamePatternMessage(),
                PublisherAddress,
                SubscriberAddress,
                PersistenceUseType.SubscriberReceive);

            handler = new TestMessageHandler<TestNamePatternMessage>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_not_pass_the_message_through = () => handler.HandledMessages.ShouldBeEmpty();
    }
}