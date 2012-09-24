using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.bus
{
    [Subject("Message bus")]
    public class when_publishing_a_message_on_the_bus : WithSubject<MessageBus>
    {
        static object message;
        static object processedMessage; 

        Establish context = () =>
        {
            Subject.MessagePublished += m => processedMessage = m; 
            message = new object();
        };

        Because of = () => Subject.Publish(message);

        It should_publish_the_message = () => processedMessage.ShouldBeTheSameAs(message);
    }
}
