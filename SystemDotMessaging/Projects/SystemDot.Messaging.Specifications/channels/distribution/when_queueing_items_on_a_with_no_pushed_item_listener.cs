using System;
using SystemDot.Messaging.Distribution;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.distribution
{
    [Subject(SpecificationGroup.Description)]
    public class when_queueing_items_on_a_with_no_pushed_item_listener
    {
        static Exception exception;
        static Queue<object> queue;
        static object message;

        Establish context = () =>
        {
            message = new object();
            queue = new Queue<object>(new TestTaskStarter());
        };

        Because of = () => exception = Catch.Exception(() => queue.InputMessage(message));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}