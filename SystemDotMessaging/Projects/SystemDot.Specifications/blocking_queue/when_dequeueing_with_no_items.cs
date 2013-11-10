using System;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Specifications.blocking_queue
{
    public class when_dequeueing_with_no_items
    {
        static UniqueBlockingQueue<string> queue;
        static DateTime start;
        static TimeSpan blockingTimeout;

        Establish context = () =>
        {
            blockingTimeout = new TimeSpan(0, 0, 1);
            queue = new UniqueBlockingQueue<string>(blockingTimeout);
        };

        Because of = () =>
        {
            start = DateTime.Now;
            queue.DequeueAll();
        };

        It should_return_after_the_allotted_blocking_timeout = () =>
            start.Subtract(DateTime.Now).ShouldBeLessThanOrEqualTo(-blockingTimeout);
    }
}