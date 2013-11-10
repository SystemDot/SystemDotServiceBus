using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Specifications.blocking_queue
{
    public class when_queuing_two_items
    {
        static UniqueBlockingQueue<string> queue;
        static IEnumerable<string> dequeued;
        
        Establish context = () =>
        {
            queue = new UniqueBlockingQueue<string>(new TimeSpan(0, 0, 1));
            queue.Enqueue("Test1");
            queue.Enqueue("Test2");
        };

        private Because of = () =>
        {
            dequeued = queue.DequeueAll();
        };

        It should_be_able_to_dequeue_the_first_item_in_the_queue_first = () => dequeued.First().ShouldEqual("Test1");

        It should_be_able_to_dequeue_the_last_item_in_the_queue_last = () => dequeued.Last().ShouldEqual("Test2");
    }
}