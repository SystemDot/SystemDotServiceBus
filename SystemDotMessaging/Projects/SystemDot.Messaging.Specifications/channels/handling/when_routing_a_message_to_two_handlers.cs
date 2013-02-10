using SystemDot.Messaging.Handling;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.handling
{
    [Subject("Message handling")]
    public class when_routing_a_message_to_two_handlers
    {
        static MessageHandlerRouter router;
        static TestMessageHandler<string> handler1;
        static TestMessageHandler<string> handler2;

        Establish context = () =>
        {
            router = new MessageHandlerRouter();
       
            handler1 = new TestMessageHandler<string>();
            handler2 = new TestMessageHandler<string>();
       
            router.RegisterHandler(handler1);
            router.RegisterHandler(handler2);
        };

        Because of = () => router.InputMessage("Test");

        It should_handle_the_message_in_the_first_handler = () => handler1.LastHandledMessage.ShouldEqual("Test");

        It should_handle_the_message_in_the_second_handler = () => handler2.LastHandledMessage.ShouldEqual("Test");
    }
}