using SystemDot.Messaging.Channels.Local.Publishing;
using SystemDot.Messaging.MessageTransportation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_publishing
{
    [Subject("Message publishing")]
    public class when_publishing_a_message_to_a_distributor_with_a_registered_subscriber : WithSubject<PublisherDistributor>
    {
        static MessagePayload inputMessage;
        static Subscriber subscriber;
        static MessagePayload publishedMessage;
        static MessagePayload message;

        Establish context = () =>
        {
            Configure<MessagePayloadCopier>(new MessagePayloadCopier());
            subscriber = new Subscriber();
            subscriber.MessageProcessed += m => publishedMessage = m;
            Subject.Subscribe(subscriber);

            message = new MessagePayload(new Address("TestAddress"));
            inputMessage = message;
        };

        Because of = () => Subject.InputMessage(inputMessage);

        It should_pass_an_equivelent_message_to_the_subscriber = () => publishedMessage.Address.ShouldEqual(message.Address);

        It should_copy_the_message_to_the_subscriber = () => publishedMessage.ShouldNotBeTheSameAs(message);

    }
}