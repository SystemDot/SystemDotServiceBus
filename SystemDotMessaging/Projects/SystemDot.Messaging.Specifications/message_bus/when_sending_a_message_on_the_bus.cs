using SystemDot.Pipes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_bus
{
    public class when_sending_a_message_on_the_bus
    {
        static Pipe<object> pipe; 
        static object message;
        static object pushedMessage; 

        Establish context = () =>
        {
            pipe = new Pipe<object>();
            pipe.ItemPushed += m => pushedMessage = m; 
            MessageBus.Initialise(pipe);

            message = new object();
        };

        Because of = () => MessageBus.Send(message);

        It should_send_the_message_to_the_bus_output_pipe = () => pushedMessage.ShouldBeTheSameAs(message);
    }
}
