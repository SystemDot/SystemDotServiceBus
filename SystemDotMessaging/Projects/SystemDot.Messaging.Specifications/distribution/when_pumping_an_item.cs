using SystemDot.Messaging.Distribution;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.distribution
{
    [Subject(SpecificationGroup.Description)]
    public class when_pumping_an_item
    {
        static Pump<object> pump;
        static object item;
        static object pushedItem;

        Establish context = () =>
        {
            item = new object();
            
            pump = new Pump<object>(new TestTaskStarter());
            pump.MessageProcessed += m => pushedItem = m;
        };

        Because of = () => pump.InputMessage(item);

        It should_pump_the_item = () => pushedItem.Should().BeSameAs(item);
    }
}