using SystemDot.Messaging.Pipes;
using SystemDot.Messaging.Recieving;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_handling
{
    public class when_routing_a_message_from_a_pipe_to_two_handlers
    {
        static Pipe pipe;
        static TestMessageHandler<string> handler1;
        static TestMessageHandler<string> handler2;

        Establish context = () =>
        {
            pipe = new Pipe();
            var router = new MessageHandlerRouter(pipe);
       
            handler1 = new TestMessageHandler<string>();
            handler2 = new TestMessageHandler<string>();
       
            router.RegisterHandler(handler1);
            router.RegisterHandler(handler2);
        };

        Because of = () => pipe.Publish("Test");

        It should_handle_the_message_in_the_first_handler = () => handler1.HandledMessage.ShouldEqual("Test");

        It should_handle_the_message_in_the_second_handler = () => handler2.HandledMessage.ShouldEqual("Test");
    }
}