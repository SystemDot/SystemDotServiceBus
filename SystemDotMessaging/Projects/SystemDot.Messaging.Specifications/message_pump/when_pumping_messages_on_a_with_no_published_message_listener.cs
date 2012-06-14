﻿using System;
using SystemDot.Messaging.Pipes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_pump
{
    public class when_pumping_messages_on_a_with_no_published_message_listener
    {
        static Exception exception; 
        static MessagePump pump;
        static object message;
        
        Establish context = () =>
        {
            message = new object();
            
            pump = new MessagePump(new TestThreadPool());
        };

        Because of = () => exception = Catch.Exception(() => pump.Publish(message));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}