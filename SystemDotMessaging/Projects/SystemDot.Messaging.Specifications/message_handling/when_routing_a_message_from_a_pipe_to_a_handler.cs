using SystemDot.Messaging.Pipes;
using SystemDot.Messaging.Recieving;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_handling
{
    public class when_routing_a_message_from_a_pipe_to_a_handler
    {
        static Pipe pipe;
        static TestMessageHandler<string> messageHandler;

        Establish context = () =>
        {
            pipe = new Pipe();
            var router = new MessageHandlerRouter(pipe);
 
            messageHandler = new TestMessageHandler<string>();
            router.RegisterHandler(messageHandler);
        };

        Because of = () => pipe.Publish("Test");

        It should_handle_the_message_in_the_handler = () => messageHandler.HandledMessage.ShouldEqual("Test");
    }
}