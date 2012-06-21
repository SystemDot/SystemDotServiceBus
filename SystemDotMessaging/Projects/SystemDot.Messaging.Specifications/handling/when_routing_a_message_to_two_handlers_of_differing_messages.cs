using SystemDot.Messaging.Channels.Messages.Consuming;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.handling
{
    [Subject("Message handling")]
    public class when_routing_a_message_to_two_handlers_of_differing_messages
    {
        static MessageHandlerRouter router;
        static TestMessageHandler<string> handler1;
        static TestMessageHandler<int> handler2;

        Establish context = () =>
        {
            router = new MessageHandlerRouter();
            
            handler1 = new TestMessageHandler<string>();
            handler2 = new TestMessageHandler<int>();

            router.RegisterHandler(handler1);
            router.RegisterHandler(handler2);            
        };

        Because of = () => router.InputMessage("Test");

        It should_handle_the_message_only_in_the_handler_for_the_meesage_type = () => 
            handler1.HandledMessage.ShouldEqual("Test");
    }
}