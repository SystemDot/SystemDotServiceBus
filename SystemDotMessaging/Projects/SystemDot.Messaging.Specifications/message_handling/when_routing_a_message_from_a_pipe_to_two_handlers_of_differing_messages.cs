using SystemDot.Messaging.Recieving;
using SystemDot.Pipes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_handling
{
    public class when_routing_a_message_from_a_pipe_to_two_handlers_of_differing_messages
    {
        static Pipe<object> channel;
        static TestMessageHandler<string> handler1;
        static TestMessageHandler<int> handler2;

        Establish context = () =>
        {
            channel = new Pipe<object>();
            var router = new MessageHandlerRouter(channel);
            
            handler1 = new TestMessageHandler<string>();
            handler2 = new TestMessageHandler<int>();

            router.RegisterHandler(handler1);
            router.RegisterHandler(handler2);            
        };

        Because of = () => channel.Push("Test");

        It should_handle_the_message_in_the_first_handler = () => handler1.HandledMessage.ShouldEqual("Test");
    }
}