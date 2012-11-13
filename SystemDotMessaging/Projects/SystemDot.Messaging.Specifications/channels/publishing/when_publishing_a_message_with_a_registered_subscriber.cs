using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Channels.UnitOfWork;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    [Subject("Message publishing")]
    public class when_publishing_a_message_with_a_registered_subscriber : WithSubject<Publisher>
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
            
            Subject.MessageProcessed += m => processedMessage = m;

            message = new MessagePayload();
        };

        Because of = () => Subject.InputMessage(message);

        It should_pass_an_equivelent_message_to_the_subscriber = () => 
            The<IMessageSender>()
                .As<TestMessageSender>()
                .SentMessages.ShouldContain(message);

        It should_process_the_message = () => processedMessage.ShouldBeTheSameAs(message);        
    }
}


