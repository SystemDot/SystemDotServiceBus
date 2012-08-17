using Machine.Specifications;

namespace SystemDot.Specifications.ioc
{
    [Subject("Ioc")]
    public class when_setting_a_transient_component_creator_with_two_constructors_argument_in_the_environment
    {
        Because of = () => 
            IocContainer.Register<ITestComponent, string, int>(
                (s, i) => new TestComponent(s, i));

        It should_be_created_and_able_to_retrieved_with_the_correct_first_constructor_argument = () => 
            IocContainer.Resolve<ITestComponent, string, int>("Test", 0)
                .ConstructorArgument1.ShouldEqual("Test");

        It should_be_created_and_able_to_retrieved_with_the_correct_second_constructor_argument = () =>
            IocContainer.Resolve<ITestComponent, string, int>(string.Empty, 1)
                .ConstructorArgument2.ShouldEqual(1);

    }
}