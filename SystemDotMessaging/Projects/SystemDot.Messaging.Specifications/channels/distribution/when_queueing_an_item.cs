using SystemDot.Messaging.Distribution;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.distribution
{
    [Subject(SpecificationGroup.Description)]
    public class when_queueing_an_item
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

        Because of = () => queue.InputMessage(item);

        It should_send_out_the_queued_item = () => pushedItem.ShouldBeTheSameAs(item);
    }
}