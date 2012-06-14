using SystemDot.Messaging.Recieving;
using SystemDot.Pipes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_handling
{
    [Subject("Message handling")]
    public class when_routing_a_message_from_a_pipe_to_two_handlers
    {
        static Pipe<object> pipe;
        static TestMessageHandler<string> handler1;
        static TestMessageHandler<string> handler2;

        Establish context = () =>
        {
            pipe = new Pipe<object>();
            var router = new MessageHandlerRouter(pipe);
       
            handler1 = new TestMessageHandler<string>();
            handler2 = new TestMessageHandler<string>();
       
            router.RegisterHandler(handler1);
            router.RegisterHandler(handler2);
        };

        Because of = () => pipe.Push("Test");

        It should_handle_the_message_in_the_first_handler = () => handler1.HandledMessage.ShouldEqual("Test");

        It should_handle_the_message_in_the_second_handler = () => handler2.HandledMessage.ShouldEqual("Test");
    }
}