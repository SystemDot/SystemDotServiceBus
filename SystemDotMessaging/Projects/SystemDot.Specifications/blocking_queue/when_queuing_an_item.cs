using System;
using System.Collections.Generic;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Specifications.blocking_queue
{
    public class when_queuing_an_item
    {
        static UniqueBlockingQueue<string> queue;
        static IEnumerable<string> dequeued;

        Establish context = () =>
        {
            queue = new UniqueBlockingQueue<string>(new TimeSpan(0, 0, 1));
            queue.Enqueue("Test");
        };

        Because of = () => dequeued = queue.DequeueAll();

        It should_be_able_to_be_dequeued_from_the_queue = () => dequeued.ShouldContain("Test");
    }
}