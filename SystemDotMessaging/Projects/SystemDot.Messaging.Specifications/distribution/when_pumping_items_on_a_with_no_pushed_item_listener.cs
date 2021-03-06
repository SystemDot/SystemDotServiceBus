﻿using System;
using SystemDot.Messaging.Distribution;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.distribution
{
    [Subject(SpecificationGroup.Description)]
    public class when_pumping_items_on_a_with_no_pushed_item_listener
    {
        static Exception exception; 
        static Pump<object> pump;
        static object message;
        
        Establish context = () =>
        {
            message = new object();
            
            pump = new Pump<object>(new TestTaskStarter());
        };

        Because of = () => exception = Catch.Exception(() => pump.InputMessage(message));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}