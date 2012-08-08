using Machine.Specifications;

namespace SystemDot.Specifications.ioc
{
    [Subject("Ioc")]
    public class when_setting_a_transient_component_creator_in_the_environment
    {
        Because of = () => IocContainer.Register<ITestComponent>(() => new TestComponent());

        It should_be_created_and_able_to_retrieved = () => 
            IocContainer.Resolve<ITestComponent>().ShouldBeOfType<TestComponent>();

    }
}