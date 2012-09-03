using SystemDot.Ioc;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Specifications.ioc
{
    [Subject("Ioc")]
    public class when_registering_an_instance_with_parameters_in_the_container : WithSubject<IocContainer>
    {
        Establish context = () =>
        {
            Subject.RegisterInstance<ITestComponent, TestComponent>();
            Subject.RegisterInstance<IAnotherTestComponent>(() => new AnotherTestComponent(Subject.Resolve<IThirdTestComponent>(), new AnotherInheritingComponent()));
            Subject.RegisterInstance<IThirdTestComponent, ThirdTestComponent>();
        };

        Because of = () => Subject.RegisterInstance<ITestComponentWithParameters, TestComponentWithParameters>();

        It should_be_able_to_be_resolved = () => 
            Subject.Resolve<ITestComponentWithParameters>().ShouldBeOfType(typeof(TestComponentWithParameters));

        It should_have_the_correct_first_parameter = () => 
            Subject.Resolve<ITestComponentWithParameters>().As<TestComponentWithParameters>()
                .FirstParameter.ShouldBeOfType<TestComponent>();

        It should_have_the_correct_second_parameter = () => 
            Subject.Resolve<ITestComponentWithParameters>().As<TestComponentWithParameters>()
                .SecondParameter.ShouldBeOfType<AnotherTestComponent>();

        It should_have_the_correct_dependency_in_the_second_parameter = () =>
            Subject.Resolve<ITestComponentWithParameters>().As<TestComponentWithParameters>()
                .SecondParameter.As<AnotherTestComponent>().FirstParameter.ShouldBeOfType<ThirdTestComponent>();
    }
}