using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_bus
{
    [Subject("Message bus")]
    public class when_sending_a_message_on_the_bus
    {
        static object message;
        static object processedMessage; 

        Establish context = () =>
        {
            var bus = new MessageBus();
            bus.MessageProcessed += m => processedMessage = m; 
            message = new object();
        };

        Because of = () => MessageBus.Send(message);

        It should_send_the_message = () => processedMessage.ShouldBeTheSameAs(message);
    }
}
