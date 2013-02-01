using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Publishing;
using SystemDot.Messaging.Publishing.Builders;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    [Subject("Message publishing")]
    public class when_publishing_a_message_with_a_previously_registered_subscriber : WithSubject<Publisher>
    {
        static SubscriptionSchema subscriber;
        static MessagePayload message;
        static MessagePayload processedMessage;

        Establish context = () =>
        {
            Configure(new EndpointAddress("Publisher", "Server"));
            Configure<IChangeStore>(new InMemoryChangeStore(new PlatformAgnosticSerialiser()));
            Configure<ISerialiser>(new PlatformAgnosticSerialiser());
            Configure<IMessageSender>(new TestMessageSender());
            Configure<ISubscriberSendChannelBuilder>(new TestSubscriberSendChannelBuilder(The<IMessageSender>()));

            subscriber = new SubscriptionSchema { SubscriberAddress = new EndpointAddress("Subsrcriber", "Server") };
            Subject.Subscribe(subscriber);

            message = new MessagePayload();

            Subject = new Publisher(The<EndpointAddress>(), The<ISubscriberSendChannelBuilder>(), The<IChangeStore>());
            Subject.MessageProcessed += m => processedMessage = m;
        };

        Because of = () => Subject.InputMessage(message);

        It should_pass_the_message_to_the_subscriber = () =>
            The<IMessageSender>()
                .As<TestMessageSender>()
                .SentMessages.ShouldContain(message);
    }
}