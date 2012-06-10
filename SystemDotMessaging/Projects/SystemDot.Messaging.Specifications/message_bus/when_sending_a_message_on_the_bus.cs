using SystemDot.Messaging.Pipes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_bus
{
    public class when_sending_a_message_on_the_bus
    {
        static Pipe pipe; 
        static object message; 
        static object published; 

        Establish context = () =>
        {
            pipe = new Pipe();
            pipe.MessagePublished += m => published = m; 
            MessageBus.Initialise(pipe);

            message = new object();
        };

        Because of = () => MessageBus.Send(message);

        It should_send_the_message_to_the_bus_output_pipe = () => published.ShouldBeTheSameAs(message);
    }
}
