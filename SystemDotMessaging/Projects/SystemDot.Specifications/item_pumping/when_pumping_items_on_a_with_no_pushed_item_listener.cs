using System;
using SystemDot.Pipes;
using Machine.Specifications;

namespace SystemDot.Specifications.item_pumping
{
    [Subject("Item pumping")]
    public class when_pumping_items_on_a_with_no_pushed_item_listener
    {
        static Exception exception; 
        static Pump<object> pump;
        static object message;
        
        Establish context = () =>
        {
            message = new object();
            
            pump = new Pump<object>(new TestThreadPool());
        };

        Because of = () => exception = Catch.Exception(() => pump.Push(message));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}