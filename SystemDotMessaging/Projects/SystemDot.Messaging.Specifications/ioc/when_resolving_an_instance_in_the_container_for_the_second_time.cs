using SystemDot.Messaging.Ioc;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.ioc
{
    [Subject("Ioc")]
    public class when_resolving_an_instance_in_the_container_for_the_second_time
    {
        static ITestComponent component1;
        static ITestComponent component2;

        Establish context = () =>
        {
            IocContainer.RegisterInstance<ITestComponent, TestComponent>();
            component1 = IocContainer.Resolve<ITestComponent>();
        };

        Because of = () => component2 = IocContainer.Resolve<ITestComponent>();

        It should_be_the_same_object = () => component2.ShouldBeTheSameAs(component1);
    }
}