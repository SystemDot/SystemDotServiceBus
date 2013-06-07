using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.hooks
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_an_event_on_a_subscriber_channel_with_a_hook : WithPublisherSubject
    {
        const string SubscriberAddress = "SubscriberAddress";
        const string PublisherAddress = "PublisherAddress";
        const int Message = 1;
        
        static MessagePayload payload;
        static TestMessageProcessorHook hook;

        Establish context = () =>
        {
            hook = new TestMessageProcessorHook();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SubscriberAddress).ForSubscribingTo(PublisherAddress)
                .WithHook(hook)
                .Initialise();

            payload = new MessagePayload().MakeSequencedReceivable(
                Message,
                PublisherAddress,
                SubscriberAddress,
                PersistenceUseType.SubscriberSend);
        };

        Because of = () => Server.ReceiveMessage(payload);

        It should_run_the_message_through_the_hook = () => hook.Message.ShouldEqual(Message);
    }
}