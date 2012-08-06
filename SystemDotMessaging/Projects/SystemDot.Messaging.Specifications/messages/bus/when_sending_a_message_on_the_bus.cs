using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.bus
{
    [Subject("Message bus")]
    public class when_sending_a_message_on_the_bus : WithSubject<MessageBus>
    {
        static object message;
        static object processedMessage; 

        Establish context = () =>
        {
            Subject.MessageSent += m => processedMessage = m; 
            message = new object();
        };

        Because of = () => Subject.Send(message);

        It should_send_the_message = () => processedMessage.ShouldBeTheSameAs(message);
    }
}
