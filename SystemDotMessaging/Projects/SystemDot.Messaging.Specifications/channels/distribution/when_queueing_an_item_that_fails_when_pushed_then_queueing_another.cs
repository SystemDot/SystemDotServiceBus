using System;
using SystemDot.Messaging.Distribution;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.distribution
{
    [Subject(SpecificationGroup.Description)]
    public class when_queueing_an_item_that_fails_when_pushed_then_queueing_another
    {
        static Queue<object> queue;
        static object failingItem;
        static object pushedItem;
        static object item;

        Establish context = () =>
        {
            failingItem = new object();
            item = new object();

            queue = new Queue<object>(TestTaskStarter.Unlimited());

            queue.MessageProcessed += m =>
            {
                if (m == failingItem) throw new Exception();
                pushedItem = m;
            };

            Catch.Exception(() => queue.InputMessage(failingItem));
        };

        Because of = () => queue.InputMessage(item);

        It should_send_out_the_queued_item = () => pushedItem.ShouldBeTheSameAs(item);
    }
}