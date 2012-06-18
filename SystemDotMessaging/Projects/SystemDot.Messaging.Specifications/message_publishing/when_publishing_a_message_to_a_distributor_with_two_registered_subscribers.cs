using SystemDot.Messaging.Channels.Local.Publishing;
using SystemDot.Messaging.MessageTransportation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_publishing
{
    [Subject("Message publishing")]
    public class when_publishing_a_message_to_a_distributor_with_two_registered_subscribers : WithSubject<PublisherDistributor>
    {
        static MessagePayload inputMessage;
        static Subscriber subscriber1;
        static Subscriber subscriber2;
        static MessagePayload publishedMessage1;
        static MessagePayload publishedMessage2;
        static MessagePayload message;

        Establish context = () =>
        {
            subscriber1 = new Subscriber();
            subscriber1.MessageProcessed += m => publishedMessage1 = m;
            Subject.Subscribe(subscriber1);

            subscriber2 = new Subscriber();
            subscriber2.MessageProcessed += m => publishedMessage2 = m;
            Subject.Subscribe(subscriber2);
            
            message = new MessagePayload(new Address("TestAddress"));
            inputMessage = message;
        };

        Because of = () => Subject.InputMessage(inputMessage);

        It should_pass_a_copy_of_the_message_to_the_first_subscriber = () => publishedMessage1.Address.ShouldEqual(message.Address);

        It should_pass_a_copy_of_the_message_to_the_second_subscriber = () => publishedMessage2.Address.ShouldEqual(message.Address);

    }
}