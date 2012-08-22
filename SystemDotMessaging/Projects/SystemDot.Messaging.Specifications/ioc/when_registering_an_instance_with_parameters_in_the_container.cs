using SystemDot.Messaging.Ioc;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.ioc
{
    [Subject("Ioc")]
    public class when_registering_an_instance_with_parameters_in_the_container
    {
        
        Establish context = () =>
        {
            IocContainer.RegisterInstance<ITestComponent, TestComponent>();
            IocContainer.RegisterInstance<IAnotherTestComponent>(() => new AnotherTestComponent(IocContainer.Resolve<IThirdTestComponent>(), new AnotherInheritingComponent()));
            IocContainer.RegisterInstance<IThirdTestComponent, ThirdTestComponent>();
            
        };

        Because of = () => IocContainer.RegisterInstance<ITestComponentWithParameters, TestComponentWithParameters>();

        It should_be_able_to_be_resolved = () => 
            IocContainer.Resolve<ITestComponentWithParameters>().ShouldBeOfType(typeof(TestComponentWithParameters));

        It should_have_the_correct_first_parameter = () => 
            IocContainer.Resolve<ITestComponentWithParameters>().As<TestComponentWithParameters>()
                .FirstParameter.ShouldBeOfType<TestComponent>();

        It should_have_the_correct_second_parameter = () => 
            IocContainer.Resolve<ITestComponentWithParameters>().As<TestComponentWithParameters>()
                .SecondParameter.ShouldBeOfType<AnotherTestComponent>();

        It should_have_the_correct_dependency_in_the_second_parameter = () =>
            IocContainer.Resolve<ITestComponentWithParameters>().As<TestComponentWithParameters>()
                .SecondParameter.As<AnotherTestComponent>().FirstParameter.ShouldBeOfType<ThirdTestComponent>();
    }
}