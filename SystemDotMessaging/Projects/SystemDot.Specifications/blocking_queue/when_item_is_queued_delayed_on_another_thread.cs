using System;
using System.Collections.Generic;
using System.Threading;
using SystemDot.Parallelism;
using Machine.Specifications;
using ThreadPool = System.Threading.ThreadPool;

namespace SystemDot.Specifications.blocking_queue
{
    public class when_item_is_queued_delayed_on_another_thread	
    {
        static UniqueBlockingQueue<string> queue;
        static DateTime start;
        static IEnumerable<string> dequeued;
        
        Establish context = () =>
        {
            queue = new UniqueBlockingQueue<string>(new TimeSpan(0, 0, 1));
            start = DateTime.MaxValue;           
        };

        Because of = () =>
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                Thread.Sleep(100);
                start = DateTime.Now;
                queue.Enqueue("foo");
            });

            dequeued = queue.DequeueAll();  
        };

        It should_block_the_current_thread_until_the_item_arrives = () => start.ShouldBeLessThanOrEqualTo(DateTime.Now);
    }
}