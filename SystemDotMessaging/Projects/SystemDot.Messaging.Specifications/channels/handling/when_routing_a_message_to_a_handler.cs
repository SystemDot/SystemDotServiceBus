﻿using SystemDot.Messaging.Channels.Handling;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.handling
{
    [Subject("Message handling")]
    public class when_routing_a_message_to_a_handler
    {
        static TestMessageHandler<string> messageHandler;
        static MessageHandlerRouter router;

        Establish context = () =>
        {
            router = new MessageHandlerRouter();
 
            messageHandler = new TestMessageHandler<string>();
            router.RegisterHandler(messageHandler);
        };

        Because of = () => router.InputMessage("Test");

        It should_handle_the_message_in_the_handler = () => messageHandler.HandledMessage.ShouldEqual("Test");
    }
}