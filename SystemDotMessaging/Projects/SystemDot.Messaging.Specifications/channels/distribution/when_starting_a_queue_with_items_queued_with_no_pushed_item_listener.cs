using System;
using SystemDot.Messaging.Channels.Distribution;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.distribution
{
    [Subject("Message distribution")]
    public class when_starting_a_queue_with_items_queued_with_no_pushed_item_listener
    {
        static Exception exception;
        static Queue<object> queue;
        static object message;

        Establish context = () =>
        {
            message = new object();

            queue = new Queue<object>(new TestTaskStarter());
        };

        Because of = () =>
        {
            queue.InputMessage(message);
            exception = Catch.Exception(() => queue.Start());
        };

        It should_not_fail = () => exception.ShouldBeNull();
    }
}