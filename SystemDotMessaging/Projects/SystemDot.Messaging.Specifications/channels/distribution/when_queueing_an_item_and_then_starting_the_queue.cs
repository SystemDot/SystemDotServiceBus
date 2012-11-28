using SystemDot.Messaging.Channels.Distribution;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.distribution
{
    [Subject("Message distribution")]
    public class when_queueing_an_item_and_then_starting_the_queue
    {
        static Queue<object> queue;
        static object item;
        static object pushedItem;

        Establish context = () =>
        {
            item = new object();

            queue = new Queue<object>(new TestTaskStarter());
            queue.MessageProcessed += m => pushedItem = m;
        };

        Because of = () =>
        {
            queue.InputMessage(item);
            queue.Start();
        };

        It should_send_out_the_queued_item = () => pushedItem.ShouldBeTheSameAs(item);
    }
}