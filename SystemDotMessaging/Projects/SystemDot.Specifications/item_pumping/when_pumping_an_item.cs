using SystemDot.Pipes;
using Machine.Specifications;

namespace SystemDot.Specifications.item_pumping
{
    [Subject("Item pumping")]
    public class when_pumping_an_item
    {
        static Pump<object> pump;
        static object item;
        static object pushedItem;

        Establish context = () =>
        {
            item = new object();
            
            pump = new Pump<object>(new TestThreadPool());
            pump.ItemPushed += m => pushedItem = m;
        };

        Because of = () => pump.Push(item);

        It should_pump_the_item = () => pushedItem.ShouldBeTheSameAs(item);
    }
}