using SystemDot.Messaging.Ioc;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.ioc
{
    [Subject("Ioc")]
    public class when_registering_an_instance_in_the_container
    {
        Because of = () => IocContainer.RegisterInstance<ITestComponent, TestComponent>();

        It should_be_able_to_be_resolved = () => IocContainer.Resolve<ITestComponent>().ShouldBeOfType<TestComponent>();
    }
}